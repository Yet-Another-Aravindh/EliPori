using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Elipori
{
    public class ButtonFactory
    {
        public static Button GetButtonForAutomationElement(AutomationElement currentElement)
        {
            var blinkingButton = new Button();
            var eventTrigger = new EventTrigger(FrameworkElement.LoadedEvent);
            var beginStoryboard = new BeginStoryboard{Storyboard = Application.Current.Resources["StoryBoard"] as Storyboard};
            eventTrigger.Actions.Add(beginStoryboard);
            blinkingButton.Triggers.Add(eventTrigger);
            return null;
        }

    }
}
