using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;

namespace Elipori
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EliporiWindow : Window
    {
        private static string pressedShortcut="";
        private static bool canContinue = true;
        private static readonly Dictionary<string, BasePattern> keyPatternMap = new Dictionary<string, BasePattern>();
        private readonly UIAService UIAService;
        private readonly List<AutomationElement> curentAutomationElementCollection = new List<AutomationElement>();

        private readonly KeyPressWatcher keyPressWatcher;
        private readonly Timer timer = new Timer();

        public EliporiWindow()
        {
            InitializeComponent();

            keyPressWatcher = new KeyPressWatcher(this);
            UIAService = new UIAService();

            // RegisterShowHideKeys();
            StartEliPori();
        }


        public void StartTimer()
        {
            timer.Interval = 5000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            curentAutomationElementCollection.Clear();
        }


        public void RegisterShowHideKeys()
        {
            //TODO: Make this configurable
            keyPressWatcher.RegisterAppShowKeyCombination(Key.LWin, Key.S);
            keyPressWatcher.RegisterAppHideKeyCombination(Key.LWin, Key.H);
        }

        private void StartEliPori()
        {
            EliPoriCanvas.Children.Clear();
            AutomationElementCollection taskBarElements = UIAService.GetTaskBarElements();
            foreach (AutomationElement element in taskBarElements)
            {
                try
                {
                    Button clickableButton = ButtonFactory.GetButtonForAutomationElement(element);
                    MapButtonPatten(element, clickableButton);
                }
                    //TODO: Replace this exception catching mechanism with something more sensible
                catch (ArgumentException)
                {
                }
            }

            AutomationElementCollection currentWindowElements = UIAService.GetCurrentWindowElements();
            foreach (AutomationElement element in currentWindowElements)
            {
                try
                {
                    Button clickableButton = ButtonFactory.GetButtonForAutomationElement(element);
                    MapButtonPatten(element,clickableButton);
                }
                    //TODO: Replace this exception catching mechanism with something more sensible
                catch (ArgumentException)
                {
                }
            }
        }

        private void MapButtonPatten(AutomationElement element, Button clickableButton)
        {
            object pattern;
            element.TryGetCurrentPattern(InvokePattern.Pattern, out pattern);
            if (pattern != null)
            {
                keyPatternMap.Add(clickableButton.Content.ToString(), (InvokePattern) pattern);
                EliPoriCanvas.Children.Add(clickableButton);
            }
            else
            {
                element.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern);
                if (pattern != null)
                {
                    keyPatternMap.Add(clickableButton.Content.ToString(), (ExpandCollapsePattern) pattern);
                    EliPoriCanvas.Children.Add(clickableButton);
                }
            }
        }

        private void ActivatePattern(string key)
        {
            BasePattern basePattern = keyPatternMap[key];
            var invokePattern = basePattern as InvokePattern;
            if (invokePattern != null)
                invokePattern.Invoke();
            else
            {
                var expandCollapsePattern = basePattern as ExpandCollapsePattern;
                if (expandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Collapsed)
                    expandCollapsePattern.Expand();
                else
                    expandCollapsePattern.Collapse();
            }
        }

        private void Elipori_OnKeyDown(object sender, KeyEventArgs e)
        {
            Key pressedKey = e.Key;

            if (pressedKey == Key.Return)
                Hide();
            else
            {
                if (pressedShortcut.Length == 0)
                    pressedShortcut = pressedShortcut + pressedKey;
                else if (pressedShortcut.Length == 1)
                {
                    pressedShortcut = pressedShortcut + pressedKey;
                    ActivatePattern(pressedShortcut);
                }
            }
        }
    }
}