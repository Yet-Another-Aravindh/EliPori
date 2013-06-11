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
        private static string pressedShortcut;
        private readonly UIAService UIAService;
        private readonly List<AutomationElement> curentAutomationElementCollection = new List<AutomationElement>();

        private readonly KeyPressWatcher keyPressWatcher;
        private readonly Timer timer = new Timer();

        public EliporiWindow()
        {
            InitializeComponent();

            keyPressWatcher = new KeyPressWatcher(this);
            UIAService = new UIAService();

            RegisterShowHideKeys();
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
                    EliPoriCanvas.Children.Add(clickableButton);
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
                    EliPoriCanvas.Children.Add(clickableButton);
                }
                    //TODO: Replace this exception catching mechanism with something more sensible
                catch (ArgumentException)
                {
                }
            }
        }


        private void Elipori_OnKeyDown(object sender, KeyEventArgs e)
        {
            Key pressedKey = e.Key;

            if (pressedKey == Key.Return)
                canContinue = false;
            else
            {
                pressedShortcut = pressedShortcut + pressedKey;
            }
        }
    }
}