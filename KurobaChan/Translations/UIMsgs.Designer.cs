﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KurobaChan.Translations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UIMsgs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UIMsgs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KurobaChan.Translations.UIMsgs", typeof(UIMsgs).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 您确定要退出KurobaChan吗？
        ///出于安全考虑，KurobaChan退出前将会显示所有隐藏的窗口。.
        /// </summary>
        internal static string Globals_Quit {
            get {
                return ResourceManager.GetString("Globals.Quit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 警告.
        /// </summary>
        internal static string Globals_Warning {
            get {
                return ResourceManager.GetString("Globals.Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 浏览....
        /// </summary>
        internal static string Pge_1SoftwareLocate_Browse {
            get {
                return ResourceManager.GetString("Pge_1SoftwareLocate.Browse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 请选择您要添加的外部程序的位置。
        ///如果指定程序需要使用Locale Emulator运行，此处指定的仍是程序的实际位置。稍后将需要启用Locale Emulator的支持。.
        /// </summary>
        internal static string Pge_1SoftwareLocate_Content {
            get {
                return ResourceManager.GetString("Pge_1SoftwareLocate.Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 检测到与系统当前代码页不同的程序，其可能需要使用Locale Emulator启动：
        ///程序名: {0}
        ///原始代码页: {1}
        ///若以上信息不准确，烦请在GitHub中提交一个Issue或者Pull Request。您的举措将有助于改进KurobaChan的用户体验。.
        /// </summary>
        internal static string Pge_1SoftwareLocate_LENotice {
            get {
                return ResourceManager.GetString("Pge_1SoftwareLocate.LENotice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 启用对Locale Emulator的支持(仅该程序).
        /// </summary>
        internal static string Pge_2LESupport_CheckBox {
            get {
                return ResourceManager.GetString("Pge_2LESupport.CheckBox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Locale Emulator是一个能够通过模拟区域/语言的代码页环境来运行与系统当前的全局代码页不同的程序的工具。
        ///对于跨代码页的情况(如在简体中文环境下运行代码页为Shift-JIS(932)的原生日文程序)，需要使用Locale Emulator来运行程序。
        ///该工具需要前往 https://github.com/xupefei/Locale-Emulator 下载。
        ///
        ///如果您需要使用Locale Emulator来运行此程序，为了防止误判程序主窗口，需要启用“对Locale Emulator的支持”。
        ///如果您的程序不需要使用Locale Emulator，不建议启用此选项。.
        /// </summary>
        internal static string Pge_2LESupport_Content {
            get {
                return ResourceManager.GetString("Pge_2LESupport.Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 是时候启动目标程序了。
        ///请手动启动目标程序，并进行一些操作（如必需）使程序的主窗口可见。
        /// 
        ///当主窗口可见时，请点击下方按钮以刷新窗口列表，并从列表中选择程序的主窗口。
        ///向导将会高亮选中的窗口（持续2秒），并在下方文本框中显示窗口的类名。（加粗的行是最可能的主窗口）
        ///
        ///如果您确定选择了正确的窗口，请点击“下一步”。.
        /// </summary>
        internal static string Pge_3WindowLocate_Content {
            get {
                return ResourceManager.GetString("Pge_3WindowLocate.Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 主窗口类名:.
        /// </summary>
        internal static string Pge_3WindowLocate_MainWindowClassName {
            get {
                return ResourceManager.GetString("Pge_3WindowLocate.MainWindowClassName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 测试热键钩子.
        /// </summary>
        internal static string Pge_4WindowHookTest_Button {
            get {
                return ResourceManager.GetString("Pge_4WindowHookTest.Button", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 现在我们需要测试目标程序的热键钩子(HotKey Hook)。
        ///
        ///请关闭并重新启动目标程序。
        ///当程序运行时，请点击下方按钮，然后在目标程序主窗口中按下K键。
        ///
        ///若热键钩子正常工作，则下方文本将会显示目标窗口的标题和按下的K键信息。
        ///
        ///请注意: 仅当热键钩子正常工作时，向导才能继续。.
        /// </summary>
        internal static string Pge_4WindowHookTest_Content {
            get {
                return ResourceManager.GetString("Pge_4WindowHookTest.Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 以下程序将会被添加：
        ///
        ///程序名: {0}
        ///可执行文件路径: {1}
        ///主窗口类名: {2}
        ///Locale Emulator支持状态: {3}
        ///
        ///点击“完成”以结束向导并将程序添加到KurobaChan数据库。.
        /// </summary>
        internal static string Pge_Complete_Content {
            get {
                return ResourceManager.GetString("Pge_Complete.Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 欢迎来到KurobaChan程序添加向导。
        ///
        ///该向导将允许您将一个新的外部程序添加到数据库中，以便通过KurobaChan来隐藏和显示它的主窗口。
        ///
        ///如果您已准备就绪，请点击“下一步”。
        ///
        ///注意：在向导过程中，您将会被要求至少启动一次目标程序，以便收集必要的信息。
        ///若您想要添加一个在当前环境下不适合启动的程序（例如在工作环境中添加NSFW游戏），请关闭此向导并等待合适的时机再启动该向导。.
        /// </summary>
        internal static string Pge_Welcome_Content {
            get {
                return ResourceManager.GetString("Pge_Welcome.Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 向导尚未完成。确实要退出向导吗？.
        /// </summary>
        internal static string Win_SoftwareAdd_Cancel {
            get {
                return ResourceManager.GetString("Win_SoftwareAdd.Cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 添加程序 - KurobaChan.
        /// </summary>
        internal static string Win_SoftwareAdd_Title {
            get {
                return ResourceManager.GetString("Win_SoftwareAdd.Title", resourceCulture);
            }
        }
    }
}
