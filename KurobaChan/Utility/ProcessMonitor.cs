// ReSharper disable LocalizableElement, ClassWithVirtualMembersNeverInherited.Global
using System.Diagnostics;
using System.Runtime.Versioning;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace KurobaChan.Utility;

/// <summary>
/// Represents the process started (and main window shown) event arguments from <see cref="ProcessMonitor"/>.
/// </summary>
public class ProcessStartedEventArgs : EventArgs
{
	/// <summary>
	/// Process instance.
	/// </summary>
	public Process Proc { get; init; } = null!;
	
	/// <summary>
	/// Main window handle (hWnd).<br />
	/// If the main window with given class name didn't show after timed out and <see cref="ProcessMonitor"/> set to enable 'AlwaysProcessEventPass', this value will be IntPtr.Zero.<br />
	/// If the main window with given class name didn't show after timed out and <see cref="ProcessMonitor"/> set to disable 'AlwaysProcessEventPass', ProcessMonitor instance will not raise the event.
	/// </summary>
	public IntPtr HWnd { get; init; } = IntPtr.Zero;
}

/// <summary>
/// Represents the process exited event arguments from <see cref="ProcessMonitor"/>.
/// </summary>
public class ProcessExitedEventArgs : EventArgs
{
	/// <summary>
	/// Cached process ID.
	/// </summary>
	public int ProcessId { get; init; }
	
	/// <summary>
	/// The exit code of the process.
	/// </summary>
	public int ExitCode { get; init; }
}

/// <summary>
/// Represents a process information to monitor (for <see cref="ProcessMonitor"/>).
/// </summary>
public class MonitorProcessInfo
{
	public string ProcName { get; init; } = string.Empty;
	public string MainWindowClassName { get; init; } = string.Empty;
}

/// <summary>
/// Represents a process monitor which can monitor the specified process(es) on Windows OS and raise events when the process started and the main window appears or the process exited.<br />
/// This class needs <see cref="KurobaChan.Utility.NativeMethods" /> class to work properly.
/// </summary>
[SupportedOSPlatform("windows")]
public class ProcessMonitor : IDisposable
{
	/// <summary>
	/// Always pass the process event whether the main window with given class name appears or not.<br />
	/// If set to <see langword="true" /> , event <see cref="ProcessStarted"/> will still be invoked
	/// even if the main window with given class name didn't show after timed out (but with a <see cref="IntPtr.Zero"/> window handle).<br />
	/// Default is <see langword="false" /> .
	/// </summary>
	public bool AlwaysProcessEventPass
	{
		get
		{
			ObjectDisposedException.ThrowIf(disposed, typeof(ProcessMonitor));
			return alwaysInvokeEvent;
		}
		set
		{
			ObjectDisposedException.ThrowIf(disposed, typeof(ProcessMonitor));
			alwaysInvokeEvent = value;
		}
	}
	
	public bool Running => !disposed && !reloading && session is {IsActive: true};
	
	private readonly List<MonitorProcessInfo> procList;
	private TraceEventSession? session;
	private CancellationTokenSource ctsMain, ctsTimeout;
	private readonly int timeout; // in milliseconds
	private bool disposed = false;
	private bool reloading = false;
	private bool alwaysInvokeEvent = false;
	
	/// <summary>
	/// Invokes when a recorded process started and the main window with given class name appears.<br />
	/// If the corresponding main window didn't show after timed out and <see cref="AlwaysProcessEventPass"/> is set to <see langword="true" />,
	/// this event will <b>ALSO</b> be invoked, but with a <see cref="IntPtr.Zero"/> window handle.
	/// </summary>
	public event EventHandler<ProcessStartedEventArgs>? ProcessStarted;
	
	/// <summary>
	/// Invokes when a recorded process exited.
	/// </summary>
	public event EventHandler<ProcessExitedEventArgs>? ProcessExited;

	/// <summary>
	/// Initialize a new instance of <see cref="ProcessMonitor"/> with the specified processes to monitor (and a timeout).
	/// </summary>
	/// <param name="processes">The set of processes information to monitor.</param>
	/// <param name="timeout">The timeout (in milliseconds) for waiting the main window to appear when a process started.</param>
	public ProcessMonitor(IEnumerable<MonitorProcessInfo> processes, int timeout = 30000)
	{
		procList = new List<MonitorProcessInfo>();
		foreach (var p in processes)
		{
			procList.Add(new MonitorProcessInfo()
			{
				ProcName = p.ProcName.Replace(".exe", string.Empty), // remove '.exe' extension if exists.
				MainWindowClassName = p.MainWindowClassName
			});
		}
		this.timeout = timeout;
		ctsMain = new CancellationTokenSource();
		ctsTimeout = new CancellationTokenSource();
		
	}

	/// <summary>
	/// Start monitoring the processes (in a sub-thread).<br />
	/// Note: Only by manually calling this function can the <see cref="ProcessStarted"/> and <see cref="ProcessExited"/> events be raised.
	/// </summary>
	/// <exception cref="InvalidOperationException">The process monitor session is already active.</exception>
	public void Start()
	{
		ObjectDisposedException.ThrowIf(disposed, typeof(ProcessMonitor));
		if (reloading) return;
		if (session is {IsActive: true}) // cannot duplicate start
		{
			throw new InvalidOperationException("The process monitor session is already active.");
		}
		session = new TraceEventSession("KurobaChanProcessMonitorSession");
		session.EnableKernelProvider(KernelTraceEventParser.Keywords.Process);
		session.Source.Kernel.ProcessStart += OnProcessStarted;
		session.Source.Kernel.ProcessStop += OnProcessExited;
		Task.Run(() => session.Source.Process(), ctsMain.Token);
	}

	/// <summary>
	/// Stop monitoring the processes.<br />
	/// The monitor can be started again after calling this function (but before disposing the <see cref="ProcessMonitor"/> instance).<br />
	/// This function will NOT clear the process list to monitor. To use a new list on next monitor session, please explicitly call <see cref="UpdateProcessList"/> after stopping the monitor.
	/// </summary>
	/// <returns><see langword="true"/> if the monitor session is stopped successfully; otherwise, <see langword="false"/> .</returns>
	public bool Stop()
	{
		ObjectDisposedException.ThrowIf(disposed, typeof(ProcessMonitor));
		if (reloading || session is {IsActive: false} || session == null) return false; // cannot duplicate stop (but don't throw exception to let it can stop in any case)
		ctsTimeout.Cancel();
		ctsMain.Cancel();
		session.Stop();
		session.Dispose();
		session = null;
		ctsTimeout.Dispose();
		ctsMain.Dispose();
		ctsMain = new CancellationTokenSource(); // 为下次Start()准备新的CancellationToken
		ctsTimeout = new CancellationTokenSource(); // 同上
		return true;
	}
	
	/// <summary>
	/// Update the process list to monitor.
	/// </summary>
	/// <param name="processes">The new set of processes to monitor.</param>
	public void UpdateProcessList(IEnumerable<MonitorProcessInfo> processes)
	{
		ObjectDisposedException.ThrowIf(disposed, typeof(ProcessMonitor));
		// 标记reloading，但是不需要真正reload（因为标记reloading只是为了阻塞任何新的ProcessStarted/ProcessExited事件）
		reloading = true;
		try
		{
			procList.Clear();
			foreach (var p in processes)
			{
				procList.Add(new MonitorProcessInfo()
				{
					ProcName = p.ProcName.Replace(".exe", string.Empty), // remove '.exe' extension if exists.
					MainWindowClassName = p.MainWindowClassName
				});
			}
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"[Error] An unexpected error occurred while updating process list: {ex}");
			Console.ResetColor();
		}
		reloading = false;
	}

	/// <summary>
	/// Invokes when a process started.
	/// </summary>
	/// <param name="data">The ETW (Event Tracing for Windows) process trace data.</param>
	private void OnProcessStarted(ProcessTraceData data)
	{
		if (disposed || reloading) return;
		string procName = data.ProcessName; // without '.exe' extension
		try
		{
			if (Process.GetProcesses().All(p => p.Id != data.ProcessID)) return; // process not found
			var proc = Process.GetProcessById(data.ProcessID);

			foreach (var monitorProc in procList)
			{
				if (procName.Equals(monitorProc.ProcName, StringComparison.OrdinalIgnoreCase))
				{
					Task.Run(() => TryFetchMainWindowAsync(procName, proc, monitorProc.MainWindowClassName, timeout, ctsTimeout.Token), ctsTimeout.Token);
				}
			}
		}
		catch (ArgumentException ex)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($"[Debug/Trace] An error occurred while fetching process information: {ex.Message}\n" +
			                  "This may indicates that the target process is no long running, which was not an issue.");
			Console.ResetColor();
		}
	}
	
	/// <summary>
	/// Invokes when a process exited.
	/// </summary>
	/// <param name="data">The ETW process trace data.</param>
	private void OnProcessExited(ProcessTraceData data)
	{
		if (disposed || reloading) return;
		string procName = data.ProcessName; // without '.exe' extension
		foreach (var monitorProc in procList)
		{
			if (procName.Equals(monitorProc.ProcName, StringComparison.OrdinalIgnoreCase))
			{
				ProcessExited?.Invoke(this, new ProcessExitedEventArgs { ProcessId = data.ProcessID, ExitCode = data.ExitStatus });
			}
		}
	}

	private async Task TryFetchMainWindowAsync(string procName, Process process, string className, int currTimeOut, CancellationToken token = default)
	{
		if (disposed || reloading) return;
		var handle = await WaitForMainWindowAsync(process, className, currTimeOut, token);
		if (handle != IntPtr.Zero)
		{
			ProcessStarted?.Invoke(this, new ProcessStartedEventArgs {Proc = process, HWnd = handle});
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[Warning] Main window for process '{procName}' with class name '{className}' did not appear within the timeout period.");
		}
	}

	private async Task<IntPtr> WaitForMainWindowAsync(Process process, string className, int currTimeOut, CancellationToken token = default)
	{
		if (disposed || reloading) return IntPtr.Zero;
		var stopwatch = Stopwatch.StartNew();
		while (stopwatch.ElapsedMilliseconds < currTimeOut && !token.IsCancellationRequested && !process.HasExited)
		{
			var hWnds = NativeMethods.User32.FindAllWindowsByClassName(className);
			foreach (var hWnd in hWnds)
			{
				if (hWnd != IntPtr.Zero && NativeMethods.User32.GetWindowThreadProcessId(hWnd, out uint pid) > 0 && pid == process.Id)
				{
					return hWnd;
				}
			}
			await Task.Delay(500, token);
		}
		return IntPtr.Zero;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposed) return;
		disposed = true;
		if (disposing)
		{
			session?.Dispose();
			ctsMain.Dispose();
			ctsTimeout.Dispose();
		}
	}

	/// <summary>
	/// Dispose the registered ETW (Event Tracing for Windows) event listeners and all other resources used by this <see cref="ProcessMonitor"/> instance.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}