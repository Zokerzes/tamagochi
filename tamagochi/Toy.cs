using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using static System.Console;
using Timer = System.Timers.Timer;

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
            timer.Enabled = false;                          //таймер хотелки стоп
            ToyArgs toyArgs = getWishList();                //что тамогочи хочет
            while (toyArgs.Equals(prevToyArgs)) toyArgs = getWishList();    //защита от повтора хотелки
            prevToyArgs = toyArgs;                                          //запоминаем хотелку чтобы не повторяться
            printToy.itsOk();                                               //вывод картинки
            Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");   //вывод статусов

            // отрисовка и обработка окна диалога
            DialogResult result = MessageBox.Show(toyArgs.text, toyArgs.title, MessageBoxButtons.OKCancel, toyArgs.icon);
            if (result == DialogResult.OK) {    //если ок
                timer.Enabled = true;           //таймер  продолжить
                printToy.itsOk();               //вывод картинки
                Console.WriteLine($"isSick = {isSick}, refusal = {refusal}"); //вывод статусов
            }
            else
            {
                printToy.itsNotOk();
                Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
                timer.Enabled = true;
                refusal++;                              //если тамогочи отказали прибавить счетчик отказов 
                if (refusal >= 3) isSick = true;        //при 3х отказов - заболевает


                if (isSick)
                {
                    timer.Enabled = false;
                    printToy.itsSick();
                    Console.WriteLine($"isSick = {isSick}, refusal = {refusal}");
                    DialogResult result1 = MessageBox.Show("Вылечи меня", "Тамогочи заболел", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (result1 == DialogResult.OK)
                    {
                        refusal = 0;            //вылечили - всё снова хорошо
                        isSick = false;
                        timer.Enabled = true;
                    }
                    else
                    {
                        printToy.itsRIP();          //невылечили - тама умер от болезни
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
            if (!isSick && !isAlife) //если тама не болной но просто состарился - умер от старости
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
            initGlobalTimer();  //старт таймера жизни
            initTimer();        //старт хотелок
            life();                 
            
        }
       

        private ToyArgs getWishList() // сборка текущей хотелки
        {
            ToyArgs toyArgs = null;
            Random random = new Random();
            int temp = random.Next(0, 4);
            SortedDictionary<string, MessageBoxIcon>.KeyCollection keys = wishList.Keys; //достаём ключи из словаря
            List<string> keyList = keys.ToList(); //формируем список ключей
            MessageBoxIcon icon;
            wishList.TryGetValue(keyList[temp], out icon);          //по ключу достаем значение (?) иконки
            toyArgs = new ToyArgs("Тамагочи", keyList[temp], icon); //формируем аргументаы хотелки

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
