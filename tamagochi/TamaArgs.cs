using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace tamagochi
{
    internal class TamaArgs    //возврат параметров message box
    {
        private string title, text;
        private int icon;
        private static TamaArgs prevArgs = null;
        private List<string>args = new List<string>()
        {
            "Хочу есть","хочу спать","Я заболел",
            "Поиграй со мной","Погуляй со мной"
        };

        TamaArgs(string title, string text,int icon)
        {
            this.title = title;
            this.text = text;
            this.icon = icon;

        }
        public static TamaArgs getMessadgeArgs()
        {
            TamaArgs tamaArgs = null;
            Random random = new Random();
            int i = random.Next(0,4);
            switch (i)
            {
                case 0: tamaArgs = new TamaArgs(
                default:
                    break;
            }
        }
        
    }
}
