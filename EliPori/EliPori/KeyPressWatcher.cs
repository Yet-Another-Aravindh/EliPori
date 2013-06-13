using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace Elipori
{
    public class KeyPressWatcher
    {
        private readonly Window currentWindow;
        private KeyboardHookListener keyboardHookListener = new KeyboardHookListener(new GlobalHooker());
        private Keys[] showKeys = new Keys[2];
        private Keys[] hideKeys = new Keys[2];
        private bool watchSecondKey;
        public KeyPressWatcher(Window currentWindow)
        {
            this.currentWindow = currentWindow;
            keyboardHookListener.Enabled = true;
            keyboardHookListener.KeyDown += Windows_keyPress;
        }

        private void Windows_keyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!currentWindow.IsActive)
            {
                var key = e.KeyCode;
                if (!watchSecondKey && (key.Equals(showKeys[0]) || key.Equals(hideKeys[0])))
                {
                    watchSecondKey = true;
                }

                else if (watchSecondKey)
                {
                    if (key.Equals(showKeys[1]))
                        currentWindow.Show();
                    else if (key.Equals(hideKeys[1]))
                        currentWindow.Hide();
                    watchSecondKey = false;
                }
            }

        }

        public void RegisterAppShowKeyCombination(params Keys[] showKeys)
        {
            this.showKeys = showKeys;
        }

        public void RegisterAppHideKeyCombination(params Keys[] hideKeys)
        {
            this.hideKeys = hideKeys;
        }
    }
}