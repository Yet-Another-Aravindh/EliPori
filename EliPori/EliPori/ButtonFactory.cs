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
            var blinkingButton = ConstructBlinkingButton(currentElement);

            var eventTrigger = new EventTrigger(FrameworkElement.LoadedEvent);
            var beginStoryboard = new BeginStoryboard { Storyboard = Application.Current.Resources["StoryBoard"] as Storyboard };
            eventTrigger.Actions.Add(beginStoryboard);
            blinkingButton.Triggers.Add(eventTrigger);

            return blinkingButton;
        }

        private static Button ConstructBlinkingButton(AutomationElement currentElement)
        {
            var blinkingButton = new Button();
            blinkingButton.Content = ContentFactory.GetRandomButtonContent();
            Rect boundingRectangle = currentElement.Current.BoundingRectangle;
            Point buttonLocation = boundingRectangle.Location;
            blinkingButton.SetValue(Canvas.TopProperty, buttonLocation.Y + 5);
            blinkingButton.SetValue(Canvas.LeftProperty, buttonLocation.X + 8);
            blinkingButton.Style = Application.Current.Resources["ButtonStyle"] as Style;
            double height = boundingRectangle.Height;
            if (height < 20)
                height = height + 10;
            blinkingButton.Height = height;
            double width = boundingRectangle.Width;
            if (width < 20)
                width = width + 10;
            blinkingButton.Width = width;

            return blinkingButton;
        }

    }
}
