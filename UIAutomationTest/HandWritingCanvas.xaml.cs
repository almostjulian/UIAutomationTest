using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;


namespace UIAutomationTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HandWritingCanvas : Window
    {
        public HandWritingCanvas()
        {
            InitializeComponent();
            Debug.WriteLine(Process.GetCurrentProcess().Id);
            AutomationElement browserElement = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ClassNameProperty, "Internet Explorer_Server"));
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChanged);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
           

        }

        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement senderElement = sender as AutomationElement;
            Debug.WriteLine(IsVisible);
            Debug.WriteLine(senderElement.Current.AutomationId);
            Debug.WriteLine(senderElement.Current.ClassName);
            Debug.WriteLine(senderElement.Current.ControlType);
            Debug.WriteLine(senderElement.Current.FrameworkId);
            Debug.WriteLine(senderElement.Current.Name);
            Debug.WriteLine(senderElement.Current.ProcessId);
            Debug.WriteLine("\r\n");





            var element = sender as AutomationElement;
            ControlType controlTypeId = element.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) as ControlType;
            //Console.WriteLine(controlTypeId.LocalizedControlType);
            if ((controlTypeId.LocalizedControlType == "combo box" || controlTypeId.LocalizedControlType == "edit") && (senderElement.Current.ProcessId != Process.GetCurrentProcess().Id))
            {
                
                if (this.Visibility == Visibility.Visible && (senderElement.Current.ProcessId != Process.GetCurrentProcess().Id))
                    Dispatcher.Invoke(() =>
                    {
                        Visibility = Visibility.Collapsed;
                    });
                
                Rect editBoxRect = (Rect)element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                this.Dispatcher.Invoke((Action)(() =>
                {
                    Height = editBoxRect.Height;
                    Width = editBoxRect.Width;
                    Left = editBoxRect.Left;
                    Top = editBoxRect.Top;

                    Visibility = Visibility.Visible;
                }));

                /*
                  canvas.Width = (int)editBoxRect.Width;
                  canvas.Height = (int)editBoxRect.Height;
                  canvas.Location = new System.Drawing.Point(Convert.ToInt32(editBoxRect.X), Convert.ToInt32(editBoxRect.Y));
                  canvas.Show();
                  */


                object patternObj;
                if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
                {
                    var valuePattern = patternObj as ValuePattern;
                    //Console.WriteLine(valuePattern.Current.Value);


                    //  valuePattern.SetValue("sucker");

                }
                else if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
                {
                    var textPattern = patternObj as TextPattern;
                    Console.WriteLine(textPattern.DocumentRange.GetText(-1).TrimEnd('\r'));
                }
                else
                {
                    Console.WriteLine(element.Current.Name);
                }
            }
           

        }
    }
}
