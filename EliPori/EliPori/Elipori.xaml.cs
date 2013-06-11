using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Input;

namespace InfiniteBoard
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EliporiWindow : Window
    {
        private static string pressedShortcut;
        private static bool canContinue = true;
        private readonly Timer timer = new Timer();
        private List<AutomationElement> curentAutomationElementCollection = new List<AutomationElement>();
        private AutomationElement desktopElement = AutomationElement.RootElement;

        private KeyPressWatcher keyPressWatcher;
        private UIManager uiManager;

        public EliporiWindow()
        {
            InitializeComponent();

            keyPressWatcher = new KeyPressWatcher(this);
            uiManager = new UIManager();

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

        public void StartEliPori()
        {
            while (canContinue)
            {
                EliPoriCanvas.Children.Clear();
                var clickableElements = uiManager.GetClickableAutomaionElements();
                foreach (AutomationElement clickableButton in clickableElements)
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