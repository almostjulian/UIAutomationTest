using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace UIAutomationTest
{
    class Program
    {

        public static HandwritingCanvas canvas;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            canvas = new HandwritingCanvas();
            canvas.StartPosition = FormStartPosition.Manual;
            canvas.TopMost = true;
            AutomationElement browserElement = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ClassNameProperty, "Internet Explorer_Server"));
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChanged);

            while (true)
            { }

           


        }

       

        private static void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            if (canvas.Visible)
                canvas.Hide();

            var element = sender as AutomationElement;
            ControlType controlTypeId = element.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) as ControlType;
            Console.WriteLine(controlTypeId.LocalizedControlType);
            if (controlTypeId.LocalizedControlType == "combo box" || controlTypeId.LocalizedControlType == "edit")
            {
                System.Windows.Rect editBoxRect = (System.Windows.Rect) element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                canvas.Width = (int)editBoxRect.Width;
                canvas.Height = (int)editBoxRect.Height;
                canvas.Location = new System.Drawing.Point(Convert.ToInt32(editBoxRect.X), Convert.ToInt32(editBoxRect.Y));
                canvas.Show();

                object patternObj;
                if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
                {                
                    var valuePattern = patternObj as ValuePattern;
                    Console.WriteLine(valuePattern.Current.Value);


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
