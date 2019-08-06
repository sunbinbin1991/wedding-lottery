using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Configuration;

///抽奖小程序,这次做一个滚动图片版本的，这个更简单
///魏韶颖
///2016年1月4日
///由于每个人一个图片文件并以名字命名，操作系统不允许重名，所以这里去掉了重名检查
///且只有一个图片没有上下滚动，去掉了反向滚动功能
///
namespace Lottery
{
    public partial class FormPrize : Form
    {
        //默认显示第一个人的照片
        private int p1 = 0;

        //循环显示每个图片
        private int cp = 0;

        private bool ispress = false;

        List<PictureBox> pics = new List<PictureBox>();

        private List<Image> pictureList = new List<Image>();

        private List<Image> graypictureList = new List<Image>();

        private List<string> nameList = new List<string>();

        private List<string> graynameList = new List<string>();

        string _ImagePath = Path.Combine(Application.StartupPath, "Images\\gift\\");

        string _GrayImagePath = Path.Combine(Application.StartupPath, "Images\\puke\\gray\\gray");

        private string titleSoftName = "抽奖程序";
        private string titleWait = "正在抽奖,请点击停止";

        private static string _TipHelp = "请将员工的照片放到Images文件夹中，大小为128x128像素\n格式为jpg、png或bmp，并以员工姓名命名，如：张三.jpg";
        private static string _TipError = "初始化奖池失败，" + _TipHelp;
        private string _TipOnlyOne = "只有一张有效照片，抽奖没有意义";

        private bool bError;
        private Bitmap backbit;

       
        //method 1: given prize define pre
        Dictionary<int, Image> first_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> second_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> third_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> forth_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> fifth_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> sixth_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> seventh_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> eighth_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> ninth_prize = new Dictionary<int, Image>();
        Dictionary<int, Image> tenth_prize = new Dictionary<int, Image>();

        // setting of num of prize num;            
        private int RmNum = 36; //max num is 36
        private int first_num = 1;
        private int second_num = 1;
        private int third_num = 2;
        private int forth_num = 2;
        private int fifth_num = 2;
        private int sixth_num = 2;
        private int seventh_num = 4;
        private int eightth_num = 4;
        private int ninth_num = 5;
        private int tenth_num = 8;

        //get bitmap list
        public Bitmap one_bit = null;
        public Bitmap two_bit = null;
        public Bitmap three_bit = null;
        public Bitmap four_bit = null;
        public Bitmap five_bit = null;
        public Bitmap six_bit = null;
        public Bitmap seven_bit = null;
        public Bitmap eight_bit = null;
        public Bitmap nine_bit = null;
        public Bitmap ten_bit = null;

        Dictionary<int, int> choosed_prize = new Dictionary<int, int>();

        public FormPrize()
        {
            InitializeComponent();
        }

        private void FormPrize_Load(object sender, EventArgs e)
        {
            this.lblEmpName.Text = null;
            this.timer1.Interval = 90;//中间值
                                      //this.cmbPrize.SelectedIndex = 1;
            getGiftBitmap();
            InitData();
            InitPicBox();
            showpuke();                   
        }
        public void getGiftBitmap()
        {

            Bitmap bit = new Bitmap(Image.FromFile(_ImagePath+ "神奇面包处理器.png"));
            one_bit = reResizeImage(bit, 180, 180);
            Bitmap bit1 = new Bitmap(Image.FromFile(_ImagePath + "高科技生米加工仪.png"));
            two_bit = reResizeImage(bit1, 180, 180);
            Bitmap bit2 = new Bitmap(Image.FromFile(_ImagePath + "人类启蒙之火器.png"));
            three_bit = reResizeImage(bit2, 180, 180);
            Bitmap bit3 = new Bitmap(Image.FromFile(_ImagePath + "大分子去除仪.png"));
            four_bit = reResizeImage(bit3, 180, 180);
            Bitmap bit4 = new Bitmap(Image.FromFile(_ImagePath + "锅碗瓢盆没锅瓢盆一套.png"));
            five_bit = reResizeImage(bit4, 180, 180);
            Bitmap bit5 = new Bitmap(Image.FromFile(_ImagePath + "H20热进化器.png"));
            six_bit = reResizeImage(bit5, 180, 180);
            Bitmap bit6 = new Bitmap(Image.FromFile(_ImagePath + "凉凉器.png"));
            seven_bit = reResizeImage(bit6, 180, 180);
            Bitmap bit7 = new Bitmap(Image.FromFile(_ImagePath + "热控温保持器.png"));
            eight_bit = reResizeImage(bit7, 180, 180);
            Bitmap bit8 = new Bitmap(Image.FromFile(_ImagePath + "一带一路畅销瓷器套间.png"));
            nine_bit = reResizeImage(bit8, 180, 180);
            Bitmap bit9 = new Bitmap(Image.FromFile(_ImagePath + "社会人容纳器.png"));
            ten_bit = reResizeImage(bit9, 180, 180);
 
        }


        private void InitData()
        {
            LoadFromFile();
            if (bError)
            {
                //this.btnStart.Enabled = false;
                this.ka.Enabled = false;
                return;
            }
        }

        //定义规则：把员工128*128大小的照片，并以名字命名放到Images下
        private void LoadFromFile()
        {
            //简单点，没有做递归查找图片
            pictureList.Clear();
            nameList.Clear();
            try
            {
                DirectoryInfo folder = new DirectoryInfo(_ImagePath);

                FileInfo[] fiArr1 = folder.GetFiles("*.jpg");
                InitArrs(fiArr1);

                FileInfo[] fiArr2 = folder.GetFiles("*.png");
                InitArrs(fiArr2);

                FileInfo[] fiArr3 = folder.GetFiles("*.bmp");
                InitArrs(fiArr3);

                DirectoryInfo grayfolder = new DirectoryInfo(_GrayImagePath);

                FileInfo[] fiArrgray = grayfolder.GetFiles("*.jpg");

                InitArrs(fiArrgray,1);

                if (null == nameList || nameList.Count < 1)
                {
                    bError = true;
                    MessageBox.Show(_TipError);
                    return;
                }
                if (nameList.Count == 1)
                {
                    bError = true;
                    MessageBox.Show(_TipOnlyOne);
                    return;
                }

                //初始化成功后默认显示第一个
                this.picEmp.Image = this.pictureList[p1];
                this.lblEmpName.Text = this.nameList[p1];
            }
            catch (Exception)
            {
                bError = true;
                MessageBox.Show(_TipError);
                return;
            }
        }

        private void InitArrs(FileInfo[] fiArr1,int mode =0)
        {
            //normal rgb
            if (mode == 0) {
                if (null != fiArr1 && fiArr1.Length > 0)
                {
                    foreach (FileInfo file in fiArr1)
                    {
                        nameList.Add(file.Name.Substring(0, file.Name.IndexOf('.')));

                        Bitmap bit = new Bitmap(Image.FromFile(file.FullName));
                        Bitmap bit2;
                        bit2 = reResizeImage(bit, 180, 180);                        
                        pictureList.Add(bit2);
                        //bit.Dispose();
                        //bit2.Dispose();
                    }
                }
            }

            //normal gray
            if (mode == 1) {
                if (null != fiArr1 && fiArr1.Length > 0)
                {
                    foreach (FileInfo file in fiArr1)
                    {
                        graynameList.Add(file.Name.Substring(0, file.Name.IndexOf('.')));
                        graypictureList.Add(Image.FromFile(file.FullName));
                    }
                }
            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            p1++;
            if (p1 >= pictureList.Count)
            {
                p1 = 0;
            }
         
            
            Image light = null;
            Image dark = null;
            light = Image.FromFile("Images\\puke\\gray\\puke\\3.png");
            dark = Image.FromFile("Images\\puke\\gray\\gray\\0.jpg");
            Bitmap lightbit = new Bitmap(light);
            Bitmap lightbit2 = reResizeImage(lightbit, 100, 120);

            Bitmap darkbit = new Bitmap(dark);
            Bitmap darkbit2 = reResizeImage(darkbit, 100, 120);
            int tempcp = cp;
            cp++;           
            while (choosed_prize.ContainsKey(cp))
            {
                pics[cp].Image = darkbit2;
                cp++;
            }
          
            if (cp >= pics.Count)
            {
                cp = 0;
            }
            if (cp == 0)
            {
                if (!choosed_prize.ContainsKey(pics.Count - 1))
                {
                    pics[pics.Count - 1].Image = darkbit2;
                }
            }
            else { 
                 if (!choosed_prize.ContainsKey(tempcp))
                {
                    pics[tempcp].Image = darkbit2;
                }               
            }
             pics[cp].Image = lightbit2;
            this.picEmp.Image = pictureList[p1];
            this.lblEmpName.Text = nameList[p1];

        }

        //private void btnStart_Click(object sender, EventArgs e)
        //{
        //    if (bError)
        //    {
        //        return;
        //    }
        //    this.timer1.Start();
        //    //this.btnStart.Enabled = false;
        //    //this.btnStop.Enabled = true;           
        //}

        private void showPrize(int cp) {
            string result = null;            
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (first_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t神奇面包处理器\n\n\t烤头花落未成阴\t\n\t面缺崩城山寂寂\t\n\t包含四象立乾坤\t\n\t机鸣舂响日暾暾\t\n";
                first_num = int.Parse(ConfigurationManager.AppSettings["one"]);
                config.AppSettings.Settings["one"].Value = (first_num - 1).ToString();
                this.picEmp.Image = first_prize[cp];
                this.lblEmpName.Text = "神奇面包处理器";
                MessageBox.Show(result,
                        "封奖榜",
                        MessageBoxButtons.OK
                );

            }
            else if (second_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t高科技生米加工仪\n\n\t热处先争炙手去\t\n\t电奉茅山访道朝\t\n\t饭颗山头逢杜甫\t\n\t煲晴绿鸭鸣咬咬\t\n";
                this.picEmp.Image = second_prize[cp];
                this.lblEmpName.Text = "高科技生米加工仪";
                MessageBox.Show(result,
                         "封奖榜",
                         MessageBoxButtons.OK
                 );
            }
            else if (third_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t人类启蒙之火器\n\n\t热烟疏竹古原西\t\n\t电影还连后夜雷\t\n\t烤开金殿看星河\t\n\t箱有花开久客中\t\n";
                this.picEmp.Image = third_prize[cp];
                this.lblEmpName.Text = "人类启蒙之火器";
                MessageBox.Show(result,
                      "封奖榜",
                      MessageBoxButtons.OK
              );
            }
            else if (forth_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t大分子去除仪\n\n\t滤他织女嫁牵牛\t\n\t水槛风凉不待秋\t\n\t壶觞须就陶彭泽\t\n";
                this.picEmp.Image = forth_prize[cp];
                this.lblEmpName.Text = "大分子去除仪";
                MessageBox.Show(result,
                      "封奖榜",
                      MessageBoxButtons.OK
              );
            }
            else if (fifth_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t锅碗瓢盆没锅瓢盆一套\n\n\t碗凤斜飞入五弦\t\n\t碟闻私地学求仙\t\n\t套水登山四体轻\t\n\t装湖楼下水如天\t\n";
                this.picEmp.Image = fifth_prize[cp];
                this.lblEmpName.Text = "锅碗瓢盆没锅瓢盆一套";
                MessageBox.Show(result,
                         "封奖榜",
                         MessageBoxButtons.OK
                 );
            }
            else if (sixth_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\tH20热进化器\n\n\t电光时掣紫金蛇\t\n\t热颜不及寒鸦色\t\n\t水南地空多明月\t\n\t壶觞既卜仙人夜\t\n";
                this.picEmp.Image = sixth_prize[cp];
                this.lblEmpName.Text = "H20热进化器";
                MessageBox.Show(result,
                          "封奖榜",
                          MessageBoxButtons.OK
                  );
            }
            else if (seventh_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t凉凉器\n\n\t凉州声韵喜参差\t\n\t水天一色无津涯\t\n\t杯前笑歌徒勉强\t\n\t奶把玉鞭骑御马\t\n\t锅年为客遍天涯\t\n";
                this.picEmp.Image = seventh_prize[cp];
                this.lblEmpName.Text = "凉凉器";
                MessageBox.Show(result,
                         "封奖榜",
                         MessageBoxButtons.OK
                 );
            }
            else if (eighth_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t热控温保持器\n\n\t热恼渐知随念尽\t\n\t保金逐手快意尽\t\n\t温馨熟美鲜香起\t\n\t杯马朝衣野客心\t\n";
                this.picEmp.Image = eighth_prize[cp];
                this.lblEmpName.Text = "热控温保持器";
                MessageBox.Show(result,
                         "封奖榜",
                         MessageBoxButtons.OK
                 );
            }
            else if (ninth_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t一带一路畅销瓷器套间\n\n\t甜迷蜂醉飞无声\t\n\t蜜房羽客类芳心\t\n\t对君新赠远诗章\t\n\t杯酒无辞到醉乡\t\n";
                this.picEmp.Image = ninth_prize[cp];
                this.lblEmpName.Text = "一带一路畅销瓷器套间";
                MessageBox.Show(result,
                        "封奖榜",
                        MessageBoxButtons.OK
                );
            }
            else if (tenth_prize.ContainsKey(cp))
            {
                result = "恭喜您,喜提:\n\t社会人容纳器\n\n\t佩缤纷以缭转兮\t\n\t奇君对酒遥相思\t\n\t布谷鸟啼桃李院\t\n\t袋叶朱唇似花发\t\n";
                this.picEmp.Image = tenth_prize[cp];
                this.lblEmpName.Text = "社会人容纳器";
                MessageBox.Show(result,
                        "封奖榜",
                        MessageBoxButtons.OK
                );
            }
            else
            {
                MessageBox.Show("什么情况 ，后台工程师麻烦站出来一下，你需要给我一个解释，奖品是不是被你黑了？");
            }
            pics[cp].Hide();
            if (!choosed_prize.ContainsKey(cp))
            {
                choosed_prize.Add(cp, 1);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings"); // 重新加载新的配置文件 

        }

        //private void btnStop_Click(object sender, EventArgs e)
        //{
        //    this.timer1.Stop();
        //    //this.btnStart.Enabled = true;
        //    //this.btnStop.Enabled = false;
        //    showPrize(cp);
        //}

        private void read_config() {      
            first_num = int.Parse(ConfigurationManager.AppSettings["one"]);
            second_num = int.Parse(ConfigurationManager.AppSettings["two"]);
            third_num = int.Parse(ConfigurationManager.AppSettings["three"]);
            forth_num = int.Parse(ConfigurationManager.AppSettings["four"]);
            fifth_num = int.Parse(ConfigurationManager.AppSettings["five"]);
            sixth_num = int.Parse(ConfigurationManager.AppSettings["six"]);
            seventh_num = int.Parse(ConfigurationManager.AppSettings["seven"]);
            eightth_num = int.Parse(ConfigurationManager.AppSettings["eight"]);
            ninth_num = int.Parse(ConfigurationManager.AppSettings["nine"]);
            tenth_num = int.Parse(ConfigurationManager.AppSettings["ten"]);
            Console.WriteLine("first ====" + first_num);
            Console.WriteLine("two ====" + second_num);
            Console.WriteLine("third_num ====" + third_num);
            Console.WriteLine("forth_num ====" + forth_num);
            Console.WriteLine("fifth_num ====" + fifth_num);
            Console.WriteLine("sixth_num ====" + sixth_num);
            Console.WriteLine("seventh_num ====" + seventh_num);
            Console.WriteLine("eightth_num ====" + eightth_num);
            Console.WriteLine("ninth_num ====" + ninth_num);
            Console.WriteLine("tenth_num ====" + tenth_num);

        }

        //private void btnInitPool_Click(object sender, EventArgs e)
        //{
        //    read_config();
        //    LoadFromFile();
        //    prizePoolInit();
        //    string result = string.Format("初始化奖池完成，当前奖池一等奖个数为{0}，二等奖个数为{1}，三等奖个数为{2}",
        //        first_prize.Count,second_prize.Count,third_prize.Count); 
        //    MessageBox.Show(result);
        //}

        private void prizePoolInit() {            
            first_prize.Clear();
            second_prize.Clear();
            third_prize.Clear();
            forth_prize.Clear();
            fifth_prize.Clear();
            sixth_prize.Clear(); ;
            seventh_prize.Clear();
            eighth_prize.Clear();
            ninth_prize.Clear();
            tenth_prize.Clear();
            Hashtable hashtable = new Hashtable();
            Random rm = new Random();

            int[] array = new int[RmNum];
            for (int i = 0; i < RmNum; i++) {
                array[i] = i;
            }
            int[] newarrry = utils.RandomSort(array);
            for (int i = 0; i < RmNum; i++)
            {
                if (first_prize.Count < first_num)
                {
                  
                    first_prize.Add(newarrry[i], one_bit);
                }
                else if (second_prize.Count < second_num)
                {                 
                    second_prize.Add(newarrry[i], two_bit);
                }
                else if (third_prize.Count < third_num)
                {                 
                    third_prize.Add(newarrry[i], three_bit);
                }
                else if (forth_prize.Count < forth_num)
                {
                    forth_prize.Add(newarrry[i], four_bit);
                }
                else if (fifth_prize.Count < fifth_num)
                {
                    fifth_prize.Add(newarrry[i], five_bit);
                }
                else if (sixth_prize.Count < sixth_num)
                {
                    sixth_prize.Add(newarrry[i], six_bit);
                }
                else if (seventh_prize.Count < seventh_num)
                {
                    seventh_prize.Add(newarrry[i], seven_bit);
                }
                else if (eighth_prize.Count < eightth_num)
                {
                    eighth_prize.Add(newarrry[i], eight_bit);
                }
                else if (ninth_prize.Count < ninth_num)
                {
                    ninth_prize.Add(newarrry[i], nine_bit);
                }
                else {
                    tenth_prize.Add(newarrry[i], ten_bit);
                }

                Console.WriteLine(newarrry[i].ToString());
                
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //频率：10ms - 210ms
            //this.timer1.Interval = 180 - this.trackBar1.Value * 20;
        }

        private void toolStripMenuItemAuthor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("微信号：sunbinbin\n编写日期：2019年07月23日");
        }

        private void toolStripMenuItemDoc_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_TipHelp);
        }

        private void toolStripMenuItemSoftUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你想升级么,你需要最新的版本么，告诉你，我没有！");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int i = 10;
            Image temp = Image.FromFile("Images\\puke\\gray\\puke\\3.png");

            while (i-->0){
                Bitmap bit = new Bitmap(temp);
                int thiswidth = this.pictureBox2.Width;
                int thisHeight = this.pictureBox2.Height;
                Bitmap bit2 = reResizeImage(bit, thiswidth, thisHeight);
                this.pictureBox1.Image = bit2;
                this.pictureBox1.Hide();

                foreach (Control control in this.Controls)
                {
                    if (control is PictureBox) //查找某Name的控件
                    {
                        //MessageBox.Show("control"+ control.Name);
                        Console.WriteLine(control.Name + "  picbox " );
                    }
                }

                //bit2.RotateFlip(RotateFlipType.Rotate90FlipY);
                //this.pictureBox1.Image = bit2;
                //this.pictureBox1.Show();
                //bit2.RotateFlip(RotateFlipType.Rotate180FlipY);
                //this.pictureBox1.Image = bit2;
                //this.pictureBox1.Show();
                //bit2.RotateFlip(RotateFlipType.Rotate270FlipY);
                //this.pictureBox1.Image = bit2;
                //this.pictureBox1.Show();
                //temp = Image.FromFile("D:\\data\\picture\\puke\\-180.png");
                //bit = new Bitmap(temp);
                //bit = reResizeImage(bit, thiswidth, thisHeight);
                //this.pictureBox1.Image = bit;
            }

        }
        
        public static Bitmap reResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Image temp = null;
            if (ispress)
            {
                temp = Image.FromFile("Images\\puke\\gray\\puke\\3.png");
                ispress = false;
            }
            else {
                temp = Image.FromFile("Images\\puke\\gray\\gray\\0.jpg");
                ispress = true;
            }           
            Bitmap bit = new Bitmap(temp);

            Bitmap bit2;                    
      
            bit2 = reResizeImage(bit, 100, 120);

            for (int i = 0; i < pics.Count; i++) {
                pics[i].Image = bit2;
                pics[i].Show();
            }
            bit.Dispose();
            bit2.Dispose();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbPrize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblWait_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            cp = 3;
            showPrize(cp-1);
            this.pictureBox3.Hide();
        }

        private void lblEmpName_Click(object sender, EventArgs e)
        {

        }

        private void showpuke()
        {
            Image temp = Image.FromFile("Images\\puke\\gray\\gray\\0.jpg");
            Bitmap bit = new Bitmap(temp);
            Bitmap bit2;
            int thiswidth = this.pictureBox1.Width;
            int thisHeight = this.pictureBox1.Height;
            bit2 = reResizeImage(bit, thiswidth, thisHeight);

            for (int i = 0; i < pics.Count; i++)
            {
                pics[i].Image = bit2;
                pics[i].Show();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            cp = 4;
            showPrize(cp-1);
            this.pictureBox4.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            cp = 12;
            showPrize(cp-1);
            this.pictureBox12.Hide();
        }

        private void picEmp_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            cp = 2;
            showPrize(cp-1);
            this.pictureBox2.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            cp = 5;
            showPrize(cp-1);
            this.pictureBox5.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            cp = 6;
            showPrize(cp-1);
            this.pictureBox6.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            cp = 7;
            showPrize(cp-1);
            this.pictureBox7.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            cp = 8;
            showPrize(cp-1);
            this.pictureBox8.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            cp = 9;
            showPrize(cp-1);
            this.pictureBox9.Hide();
        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {
            cp = 33;
            showPrize(cp-1);
            this.pictureBox33.Hide();
        }

        private void InitPicBox() {
            pics.Add(pictureBox1);
            pics.Add(pictureBox2);
            pics.Add(pictureBox3);
            pics.Add(pictureBox4);
            pics.Add(pictureBox5);
            pics.Add(pictureBox6);
            pics.Add(pictureBox7);
            pics.Add(pictureBox8);
            pics.Add(pictureBox9);
            pics.Add(pictureBox10);
            pics.Add(pictureBox11);
            pics.Add(pictureBox12);
            pics.Add(pictureBox13);
            pics.Add(pictureBox14);
            pics.Add(pictureBox15);
            pics.Add(pictureBox16);
            pics.Add(pictureBox17);
            pics.Add(pictureBox18);
            pics.Add(pictureBox19);
            pics.Add(pictureBox20);
            pics.Add(pictureBox21);
            pics.Add(pictureBox22);
            pics.Add(pictureBox23);
            pics.Add(pictureBox24);
            pics.Add(pictureBox25);
            pics.Add(pictureBox26);
            pics.Add(pictureBox27);
            pics.Add(pictureBox28);
            pics.Add(pictureBox29);
            pics.Add(pictureBox30);
            pics.Add(pictureBox31);
            pics.Add(pictureBox32);
            pics.Add(pictureBox33);
            pics.Add(pictureBox34);
            pics.Add(pictureBox35);
            pics.Add(pictureBox36);
        }

       
        private void pictureBox18_Click(object sender, EventArgs e)
        {
            cp = 18;
            showPrize(cp-1);
            this.pictureBox18.Hide();
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            cp = 17;
            showPrize(cp-1);
            this.pictureBox17.Hide();

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            cp = 16;
            showPrize(cp-1);
            this.pictureBox16.Hide();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            cp = 15;
            showPrize(cp-1);
            this.pictureBox15.Hide();

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            cp = 14;
            showPrize(cp-1);
            this.pictureBox14.Hide();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            cp = 13;
            showPrize(cp-1);
            this.pictureBox13.Hide();
        }

        private void pictureBox19_Click_1(object sender, EventArgs e)
        {
            cp = 19;
            showPrize(cp-1);
            this.pictureBox19.Hide();
        }

        private void pictureBox24_Click_1(object sender, EventArgs e)
        {
            cp = 24;
            showPrize(cp-1);
            this.pictureBox24.Hide();
        }

        private void pictureBox28_Click_1(object sender, EventArgs e)
        {
            cp = 28;
            showPrize(cp-1);
            this.pictureBox28.Hide();
        }

        private void pictureBox29_Click_1(object sender, EventArgs e)
        {
            cp = 29;
            showPrize(cp-1);
            this.pictureBox29.Hide();
        }

        private void pictureBox30_Click_1(object sender, EventArgs e)
        {
            cp = 30;
            showPrize(cp-1);
            this.pictureBox30.Hide();
        }

        private void pictureBox31_Click_1(object sender, EventArgs e)
        {
            cp = 31;
            showPrize(cp-1);
            this.pictureBox31.Hide();
        }

        private void pictureBox32_Click_1(object sender, EventArgs e)
        {
            cp = 32;
            showPrize(cp-1);
            this.pictureBox32.Hide();
        }

        private void pictureBox33_Click_1(object sender, EventArgs e)
        {
            cp = 33;
            showPrize(cp-1);
            this.pictureBox33.Hide();
        }

        private void pictureBox34_Click_1(object sender, EventArgs e)
        {
            cp = 34;
            showPrize(cp-1);
            this.pictureBox34.Hide();
        }

        private void pictureBox35_Click_1(object sender, EventArgs e)
        {
            cp = 35;
            showPrize(cp-1);
            this.pictureBox35.Hide();
        }

        private void pictureBox36_Click_1(object sender, EventArgs e)
        {
            cp = 36;
            showPrize(cp-1);
            this.pictureBox36.Hide();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            cp = 12;
            showPrize(cp-1);
            this.pictureBox12.Hide();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            cp = 11;
            showPrize(cp-1);
            this.pictureBox11.Hide();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            cp = 10;
            showPrize(cp-1);
            this.pictureBox10.Hide();
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            cp = 22;
            showPrize(cp-1);
            this.pictureBox22.Hide();
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            cp = 23;
            showPrize(cp-1);
            this.pictureBox23.Hide();

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            cp = 21;
            showPrize(cp-1);
            this.pictureBox21.Hide();

        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            cp = 20;
            showPrize(cp-1);
            this.pictureBox20.Hide();
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            cp = 25;
            showPrize(cp-1);
            this.pictureBox25.Hide();
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            cp = 26;
            showPrize(cp-1);
            this.pictureBox26.Hide();
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            cp = 27;
            showPrize(cp-1);
            this.pictureBox27.Hide();
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {//start
            if (bError)
            {
                return;
            }
            this.timer1.Start();
            this.ka.Enabled = false;
            this.roundButton2.Enabled = true;
        }

        private void roundButton1_Click_1(object sender, EventArgs e)
        {//init
            read_config();
            LoadFromFile();
            prizePoolInit();
            string result = string.Format("初始化奖池完成，当前奖池一等奖个数为{0}，二等奖个数为{1}，三等奖个数为{2}",
                first_prize.Count, second_prize.Count, third_prize.Count);
            MessageBox.Show(result);
        }

        private void roundButton2_Click(object sender, EventArgs e)
        {//stop
            this.timer1.Stop();
            this.ka.Enabled = true;
            this.roundButton2.Enabled = false;
            showPrize(cp);
        }

        //private void pictureBox20_Click(object sender, EventArgs e)
        //{

        //}

        //private void pictureBox20_Click(object sender, EventArgs e)
        //{

        //}
    }
}
