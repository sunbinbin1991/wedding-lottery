using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Lottery
{
    class utils
    {
        public enum Gift
        {
            神奇面包处理器,
            高科技生米加工仪,
            人类启蒙之火器,
            大分子去除仪,
            锅碗瓢盆没锅瓢盆一套,
            H20热进化器,
            凉凉器,
            热控温保持器,
            一带一路畅销瓷器套间,
            社会人容纳器,
        };



        void addall(PictureBox[] pics) {
            //pics.Add(PictureBox1);
        }

        public static T[] RandomSort<T>(T[] array)
        {
            int len = array.Length;
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            T[] ret = new T[len];
            Random rand = new Random();
            int i = 0;
            while (list.Count < len)
            {
                int iter = rand.Next(0, len);
                if (!list.Contains(iter))
                {
                    list.Add(iter);
                    ret[i] = array[iter];
                    i++;
                }

            }
            return ret;
        }
    }
}
