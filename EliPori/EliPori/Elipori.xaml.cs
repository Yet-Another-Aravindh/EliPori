﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Input;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace Elipori
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EliporiWindow : Window
    {
        [DllImport("user32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        private static string pressedShortcut="";
        private static bool canContinue = true;
        private static readonly Dictionary<string, KeyValuePair<AutomationElement, BasePattern>> keyPatternMap = new Dictionary<string, KeyValuePair<AutomationElement, BasePattern>>();
        private readonly UIAService UIAService;
        private readonly List<AutomationElement> curentAutomationElementCollection = new List<AutomationElement>();

        private readonly KeyPressWatcher keyPressWatcher;
        private readonly Timer timer = new Timer();

        public EliporiWindow()
        {
            InitializeComponent();

            keyPressWatcher = new KeyPressWatcher(this);
            UIAService = new UIAService();
            MinimizeToTray();
            StartEliPori();             
        }

        private void MinimizeToTray()
        {
            RegisterShowHideKeys();
            this.Hide();
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
            keyPressWatcher.RegisterAppShowKeyCombination(Keys.Escape, Keys.S);
            keyPressWatcher.RegisterAppHideKeyCombination(Keys.Escape, Keys.H);
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
                keyPatternMap.Add(clickableButton.Content.ToString(),new KeyValuePair<AutomationElement, BasePattern>(element, (InvokePattern) pattern));
                EliPoriCanvas.Children.Add(clickableButton);
            }
            else
            {
                element.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern);
                if (pattern != null)
                {
                    keyPatternMap.Add(clickableButton.Content.ToString(), new KeyValuePair<AutomationElement, BasePattern>(element, (ExpandCollapsePattern)pattern));
                    EliPoriCanvas.Children.Add(clickableButton);
                }
            }
        }

        private void ActivatePattern(string key)
        {
            var keyValuePair = keyPatternMap[key];
            BasePattern basePattern = keyValuePair.Value;
            var invokePattern = basePattern as InvokePattern;
            if (invokePattern != null)
            {
                invokePattern.Invoke();
                ShowWindow((IntPtr)keyValuePair.Key.Current.NativeWindowHandle,3);
            }
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
                    pressedShortcut = "";
                    StartEliPori();
                }
            }
        }
    }
}