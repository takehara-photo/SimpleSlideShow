using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace AutoSlideShow
{



    /// <summary>
    /// SlideShow.xaml の相互作用ロジック
    /// </summary>
    public partial class SlideShow : Window
    {
        public SlideShow()
        {


            InitializeComponent();
          
            ReadSetting();
            ShowClockInit();
            checkAndReadImageFiles();
        }

        public ArrayList imageFileList = new ArrayList();
        public bool randomSlideShow;
        public bool loopSlideShow;
        public bool ShowClock;
        public bool BoolShowFolderName;
        public bool BoolShowFileName;
        public float ImageSwitchTime;
        public System.Random r = new System.Random();

        public void ReadSetting()
        {
            RWSettingFile pri = new RWSettingFile();
            string[] Setting = pri.ReadSetting();
            if (Setting[0] == "true")
            {
                randomSlideShow = true;
            }
            else
            {
                randomSlideShow = false;
            }
            if (Setting[1] == "true")
            {
                loopSlideShow = true;
            }
            else
            {
                loopSlideShow = false;
            }
            if (Setting[3] == "true")
            {
                ShowClock = true;
            }
            else
            {
                ShowClock = false;
            }
            if (Setting[4] == "true")
            {
               BoolShowFolderName = true;
            }
            else
            {
                BoolShowFolderName = false;
            }
            if (Setting[5] == "true")
            {
                BoolShowFileName = true;
            }
            else
            {
                BoolShowFileName = false;
            }

            try
            {
                ImageSwitchTime = float.Parse(Setting[2].ToString());
            }
            catch
            {
                MessageBox.Show("画像切替時間設定の値が不正です。\n切替時間３秒でスライドショーを開始します。", "ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
                ImageSwitchTime = 3;

            }



        }

        //画像ファイルリスト
        public void checkAndReadImageFiles()
        {

            
            //クリア
            imageFileList.Clear();
            //フォルダチェック
            bool check = false;
            //フォルダ確認
            try
            {
                if (System.IO.Directory.Exists(@"ImageFiles\"))
                {
                    check = true;


                }
                else
                {
                    MessageBox.Show("ERROR!!\nImageFiles Folder Not Found!");
                    check = false;
                    Directory.CreateDirectory(@"ImageFiles\");
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR!!");
                this.Close();
            }

            if (check == true)
            {
                //List Add Files
                string dir = @"ImageFiles\";
                imageFileList.AddRange(Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories));
                imageFileList.AddRange(Directory.GetFiles(dir, "*.jpeg", SearchOption.AllDirectories));
                imageFileList.AddRange(Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories));
                imageFileList.AddRange(Directory.GetFiles(dir, "*.gif", SearchOption.AllDirectories));
            }
            //List Null Error
            if (imageFileList.Count == 0)
            {
                check = false;
                MessageBox.Show("ImageFile Not Found!", "ERROR!!",MessageBoxButton.OK,MessageBoxImage.Error);
                this.Close();
            }


           
            ShowSlideShow();


        }


        

        //Image View

        private async void ShowSlideShow()
        {
          

            bool showNextImage = true;
            for (int a = 0; a < imageFileList.Count;)
            {
                if (showNextImage == true)
                {

                    string filesource = System.Environment.CurrentDirectory + @"\" + imageFileList[a];
                    //random
                    if (randomSlideShow == true)
                    {
                        //System.Random r = new System.Random();
                        filesource = System.Environment.CurrentDirectory + @"\" + imageFileList[r.Next(imageFileList.Count)]; 
                    }

                    
                    showNextImage = false;
                    var source = new BitmapImage();
                    source.BeginInit();
                    source.UriSource = new Uri(filesource);
                    source.EndInit();
                    Image.Source = source;


                    string ShowString = "";
                    string tmp2 = filesource;

                    //フォルダ名表示
                    if (BoolShowFolderName == true)
                      {
                        string ShowingFolderName = filesource;

                        try
                        {
                            ShowingFolderName = filesource.Substring(0, ShowingFolderName.LastIndexOf(@"\"));
                            ShowingFolderName = ShowingFolderName.Substring(ShowingFolderName.LastIndexOf(@"\") + 1);
                            if (ShowingFolderName != "")
                            {
                                ShowingFolderName = "   " + ShowingFolderName + " ";
                            }
                        }
                        catch
                        {
                            ShowingFolderName = "";
                        }
                        ShowString = ShowingFolderName;       
                    }

                    //ファイル名表示
                    if (BoolShowFileName == true)
                    {
                        if (BoolShowFolderName == true)
                        {
                            ShowString += "\n      ";
                        }

                            string tmp;
                        tmp = tmp2.Substring(tmp2.LastIndexOf(@"\") + 1);
                        tmp = tmp.Substring(0,tmp.LastIndexOf(@"."));
                        tmp += "   ";
                        ShowString += tmp;
                    }
                    if (ShowString != "")
                    {
                        this.labelFolderName.Opacity = 0.8;
                        this.labelFolderName1.Opacity = 1.0;
                        this.labelFolderName.Content = ShowString;
                        this.labelFolderName1.Content = ShowString;
                    }
                    else
                    {
                        this.labelFolderName.Opacity = 0.0;
                        this.labelFolderName1.Opacity = 0.0;


                    }



                    //X秒待機
                    await Task.Run(async () => {
                        int tmp = (int)(ImageSwitchTime * 1000);
                        await Task.Delay(tmp);
                        a++;
                        showNextImage = true;
                    });

                  



                }
            }

            //スライドショー繰り返し
            if(loopSlideShow == true)
            {
                System.Random r = new System.Random();
                checkAndReadImageFiles();
            }
            else
            {
                //this.Close();
            }

        }





        //画像クリック時閉じる
        private void ButtonImage_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        //時計表示初期化
        public void ShowClockInit()
        {
            labelClockDate.Content = "";
            labelClockTime.Content = "";

            if (ShowClock == true)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
            }
        }
        //時計表示
        void timer_Tick(object sender, EventArgs e)
        {
            labelClockDate.Content = DateTime.Now.ToString("yyyy年MM月dd日ddd曜日");
            labelClockTime.Content = DateTime.Now.ToString("HH:mm");
        }
    }


    public class ShowingImageFileClass
    {
        public string ShowingImageFile { get; set; }
    }
}
