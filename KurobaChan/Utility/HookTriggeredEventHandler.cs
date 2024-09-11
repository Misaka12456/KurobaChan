namespace KurobaChan.Utility;

/// <summary>
/// False to continue the event, true to cancel(block) the event to upper level (e.g. other programs)
/// </summary>
internal delegate bool HookTriggeredEventHandler(object sender, HookTriggeredEventArgs e);