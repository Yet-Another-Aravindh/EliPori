using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace Elipori
{
    public class UIAService
    {

        public AutomationElementCollection GetTaskBarElements()
        {
            var taskbarElement = AutomationElement.RootElement
                .FindFirst(TreeScope.Children,new PropertyCondition(AutomationElement.ClassNameProperty,"Shell_TrayWnd"));

            return GetChildElementCollection(taskbarElement);
        }

        public AutomationElementCollection GetCurrentWindowElements()
        {
            var activeElement = AutomationElement.RootElement.FindFirst(TreeScope.Children,                                                                                
                new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.Window),
                                 new PropertyCondition(AutomationElement.IsOffscreenProperty,false)));

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
