using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace tamagochi
{
    internal class Toy
    {
        bool isAlife;
        ToyArgs prevToyArgs;
        Timer timer;
        SortedDictionary <string, MessageBoxIcon> wishList = new SortedDictionary<string, MessageBoxIcon>()
        {
            {"Хочу есть",MessageBoxIcon.Warning },{"хочу спать",MessageBoxIcon.Stop},{"Я заболел",MessageBoxIcon.Warning },
            {"Поиграешь со мной?",MessageBoxIcon.Question },{"Погуляешь со мной?",MessageBoxIcon.Question }
        };


        private void initTimer()
        {
            timer = new Timer(500);
            timer.Elapsed += OnToyEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void OnToyEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss}",
            //                  e.SignalTime);

            timer.Enabled = false;

            ToyArgs toyArgs = getWishList();

            DialogResult result = MessageBox.Show(toyArgs.text, toyArgs.title, MessageBoxButtons.OKCancel, toyArgs.icon);
            if (result == DialogResult.OK) timer.Enabled = true;
            else
            {
                timer.Stop();
                timer.Dispose();
                isAlife = false;
            }
        }
            public Toy()
            {
                isAlife = true;
                prevToyArgs = null;
                initTimer();
                life();
                getWishList();
            }
        private void life()
        {
            while (isAlife)
            {

            };
        }

        private ToyArgs getWishList()
        {
            ToyArgs toyArgs = null;
            Random random = new Random();
            int temp = random.Next(0, 4);
            SortedDictionary<string, MessageBoxIcon>.KeyCollection keys = wishList.Keys;
            List<string> keyList = keys.ToList();
            //keyList.ForEach(k => Console.WriteLine(k));

            MessageBoxIcon icon;
            wishList.TryGetValue(keyList[temp],out icon);
            toyArgs = new ToyArgs("Тамагочи", keyList[temp], icon);
            return toyArgs;

        }

        private class ToyArgs
        {
            public string title, text;
            public MessageBoxIcon icon;
            public ToyArgs(string title, string text, MessageBoxIcon icon)
            {
                this.title = title;
                this.text = text;
                this.icon = icon;
            }
        }
    }
}
