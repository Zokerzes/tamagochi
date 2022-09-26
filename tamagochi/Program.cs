using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
namespace tamagochi
{
    
    internal class Program
    {
        private static Timer aTimer;

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }
        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        static void Main(string[] args)
        {
            DialogResult r8 = MessageBox.Show("запустить таймер",
                                   "Help Caption", MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Question,
                                   MessageBoxDefaultButton.Button2, 0,
                                   //"mspaint.chm",
                                   "mspaint.chm::/paint_brush.htm"
                                   );
            if(r8 == DialogResult.OK)
            {
                SetTimer();
                Console.ReadLine();
                aTimer.Stop();
                aTimer.Dispose();
                

            }
            else Console.WriteLine("Cancel");
        }
    }
}
