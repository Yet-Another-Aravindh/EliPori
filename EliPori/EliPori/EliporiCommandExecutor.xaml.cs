using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Elipori
{
    /// <summary>
    /// Interaction logic for EliporiCommandExecutor.xaml
    /// </summary>
    public partial class EliporiCommandExecutor : Window
    {
        public EliporiCommandExecutor()
        {
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                ExecuteCommand();
        }

        private void ExecuteCommand()
        {

            var process = new Process();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = @"-executionpolicy unrestricted -NoExit -Command "+CommandBox.Text;
            process.Start();
            this.Close();
//            var chromeProcess = new Process();
//            chromeProcess.StartInfo.FileName = "chrome.exe";
//            chromeProcess.StartInfo.Arguments ="http://google.com/search?q="+ CommandBox.Text;
//            chromeProcess.Start();


        }
    }
}
