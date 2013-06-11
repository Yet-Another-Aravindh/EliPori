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
        private Key[] showKeys = new Key[2];
        private Key[] hideKeys = new Key[2];
        private bool watchSecondKey;
        public KeyPressWatcher(Window currentWindow)
        {
            this.currentWindow = currentWindow;
            keyboardHookListener.KeyDown += Windows_keyPress;
        }

        private void Windows_keyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (key.Equals(showKeys[0])||key.Equals(hideKeys[0]))
            {
                watchSecondKey = true;
            }

            if (watchSecondKey)
            {
                if(key.Equals(showKeys[1]))
                    currentWindow.Show();
                else if(key.Equals(hideKeys[1]))
                    currentWindow.Hide();
                watchSecondKey = false;
            }

        }

        public void RegisterAppShowKeyCombination(params Key[] showKeys)
        {
            this.showKeys = showKeys;
        }

        public void RegisterAppHideKeyCombination(params Key[] hideKeys)
        {
            this.hideKeys = hideKeys;
        }
    }
}