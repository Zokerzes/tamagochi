using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;
using Timer = System.Timers.Timer;
using static System.Console;

namespace tamagochi
{
    internal class Toy
    {
        bool isAlife=true;          //статус жив или нет
        bool isSick=false;          //статус болеет или нет
        int refusal =0;             //счётчик отказов
        ToyArgs prevToyArgs;        //предыдущее сообщение чтобы не повторялось  
        Timer timer;                //таймер хотелок
        Timer globalTimer;          //таймер жизни
        int timeOfLife = 30000;     //мксекунды
        int wishlistPeriod = 1000;   //мксекунды
        PrintToy printToy = new PrintToy(); 

        //создаём коллекцию хотелок
        SortedDictionary<string, MessageBoxIcon> wishList = new SortedDictionary<string, MessageBoxIcon>()
        {
            {"Хочу есть",MessageBoxIcon.Warning },
            {"хочу спать",MessageBoxIcon.Stop},
            {"Я заболел",MessageBoxIcon.Warning },
            {"Поиграешь со мной?",MessageBoxIcon.Question },
            {"Погуляешь со мной?",MessageBoxIcon.Question }
        };

        

        private void initTimer() //опредиляем таймер хотелок
        {

            timer = new Timer(wishlistPeriod);
            timer.Elapsed += OnToyEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
       
        private void initGlobalTimer() //опредиляем таймер жизни
        {

            globalTimer = new Timer(timeOfLife);
            globalTimer.Elapsed += OnSenility;
            globalTimer.AutoReset = false;
            globalTimer.Enabled = true;
        }

        //событие: старость
        public void OnSenility(Object source, ElapsedEventArgs e)
        {
            globalTimer.Stop();
            globalTimer.Dispose();
            timer.Stop();
            timer.Dispose();
            isAlife = false;
        }

        //событие: хотелка
        public void OnToyEvent(Object source, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            ToyArgs toyArgs = getWishList();
            while (toyArgs.Equals(prevToyArgs)) toyArgs = getWishList();
            prevToyArgs = toyArgs;
            printToy.itsOk();
            Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
            DialogResult result = MessageBox.Show(toyArgs.text, toyArgs.title, MessageBoxButtons.OKCancel, toyArgs.icon);
            if (result == DialogResult.OK) { 
                timer.Enabled = true;
                printToy.itsOk();
                Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
            }
            else
            {
                printToy.itsNotOk();
                Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
                timer.Enabled = true;
                refusal++;
                if (refusal >= 3) isSick = true;


                if (isSick)
                {
                    timer.Enabled = false;
                    printToy.itsSick();
                    Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
                    DialogResult result1 = MessageBox.Show("Вылечи меня", "Тамогочи заболел", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (result1 == DialogResult.OK)
                    {
                        refusal = 0;
                        isSick = false;
                        timer.Enabled = true;
                    }
                    else
                    {
                        printToy.itsRIP();
                        Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
                        MessageBox.Show("RIP: из-за болезни.", "Тамогочи фсёёёоооо", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        timer.Stop();
                        timer.Dispose();
                        isAlife = false;

                    }
                }
            }
        }
        private void life()
        {
            while (isAlife)
            {
                
                
            };
            if (!isSick && !isAlife)
            {
                printToy.itsRIP();
                Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
                MessageBox.Show("RIP: от старости.", "Тамогочи фсёёёоооо", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public Toy()
        {
            
            isAlife = true;
            prevToyArgs = null;
            initGlobalTimer();
            initTimer();
            life();
            getWishList();
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
            wishList.TryGetValue(keyList[temp], out icon);
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
            public override bool Equals(object obj)
            {
                if (obj is ToyArgs)
                {
                    return this.text.Equals((obj as ToyArgs).text);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        public class PrintToy
        {
            public void itsOk()
            {
                Console.Clear();
                WriteLine(@"  _____  ");
                WriteLine(@" /     \ ");
                WriteLine(@" | O O | ");
                WriteLine(@" |  #  | ");
                WriteLine(@" | \_/ | ");
                WriteLine(@" \_____/ ");
            }

            public void itsNotOk()
            {
                Console.Clear();
                WriteLine(@"  _____  ");
                WriteLine(@" /     \ ");
                WriteLine(@" |  o O| ");
                WriteLine(@" | #   | ");
                WriteLine(@" |   _ | ");
                WriteLine(@" \_____/ ");
            }
            public void itsSick()
            {
                Console.Clear();
                WriteLine(@"  _____  ");
                WriteLine(@" /     \ ");
                WriteLine(@" | - - | ");
                WriteLine(@" |  #  | ");
                WriteLine(@" |  _  | ");
                WriteLine(@" \ / \ / ");
                WriteLine(@"  \___/ ");
            }
            public void itsRIP()
            {
                Console.Clear();
                WriteLine(@"  _____  ");
                WriteLine(@" /     \ ");
                WriteLine(@" | X X | ");
                WriteLine(@" |  #  | ");
                WriteLine(@" |  _  | ");
                WriteLine(@" \_____/ ");
            }
        }
    }
}
