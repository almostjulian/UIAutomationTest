using System;
using System.Windows.Automation;


namespace UIAutomationTest
{
    class Program
    {

        //public static HandwritingCanvas canvas;

        [STAThread]
        static void Main(string[] args)
        {
            var app = new System.Windows.Application();
            app.Run(new HandWritingCanvas());

        }

       
     
       
    }
}
