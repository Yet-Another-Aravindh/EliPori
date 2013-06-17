using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;

namespace Elipori
{

    public class UIAService
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("coredll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        public AutomationElementCollection GetTaskBarElements()
        {

            var taskbarElement = AutomationElement.RootElement
                .FindFirst(TreeScope.Children,new PropertyCondition(AutomationElement.ClassNameProperty,"Shell_TrayWnd"));

            return GetChildElementCollection(taskbarElement);
        }

        public AutomationElementCollection GetCurrentWindowElements()
        {
//            var activeElement = AutomationElement.RootElement.FindFirst(TreeScope.Children,
//                new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
//                                 new PropertyCondition(AutomationElement.IsOffscreenProperty, false)));
            var activeElement = AutomationElement.FromHandle((IntPtr)Process.GetCurrentProcess().Id);

           // ShowWindow((IntPtr)activeElement.Current.NativeWindowHandle, 3);

//            using (var writer = new StreamWriter(@"C:\elements.txt",true))
//            {
//                writer.WriteLine(activeElement.Current.Name);
//                writer.Close();
//            }
            
            return GetChildElementCollection(activeElement);
        }

        private AutomationElementCollection GetChildElementCollection(AutomationElement parentElement)
        {
           return parentElement.FindAll(TreeScope.Descendants | TreeScope.Children,
                    new AndCondition(new OrCondition( new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.Button),
                                     new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.MenuItem),
                                     new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.TreeItem)),
                                     new PropertyCondition(AutomationElement.IsEnabledProperty, true)));

        }
    }
}
