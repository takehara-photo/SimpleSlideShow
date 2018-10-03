using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;

namespace AutoSlideShow
{
    

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public int ImageSum = 0;
       
        public MainWindow()
        {
            
            InitializeComponent();
            Init();
            //ReadSettings
            ReadSettings();
        }

       public void ReadSettings()
        {
            RWSettingFile pri = new RWSettingFile();
            string[] Setting = pri.ReadSetting();

            if (Setting[1] == "true")
            {
                CheckBoxLoop.IsChecked = true;
            }
            if (Setting[3] == "true")
            {
                CheckBoxShowClock.IsChecked = true;
            }
            if (Setting[0] == "true")
            {
                RadioButtonSortRandom.IsChecked = true;
                CheckBoxLoop.IsChecked = true;
                CheckBoxLoop.IsEnabled = false;
            }
            else
            {
                RadioButtonSort.IsChecked = true;
            }
            if (Setting[4] == "true")
            {
                CheckBoxDisplayFolder.IsChecked = true;
            }
            if (Setting[5] == "true")
            {
                CheckBoxDisplayFileName.IsChecked = true;
            }
            TextBoxImageChangeTime.Text = Setting[2].ToString();


            //test
            int x = 0;
            foreach (string value in Setting)
            {
               
                //    MessageBox.Show(x + "\n" + value);
                x++;
            }

        }
        public void WriteSettings()
        {

            string[] Setting = new string[6];

            if (RadioButtonSortRandom.IsChecked == true)
            {
                Setting[0] = "true";

            }
            else
            {
                Setting[0] = "false";
            }

            if (CheckBoxLoop.IsChecked == true)
            {
                Setting[1] = "true";
            }
            else
            {
                Setting[1] = "false";
            }
            if (CheckBoxShowClock.IsChecked == true)
            {
                Setting[3] = "true";
            }
            else
            {
                Setting[3] = "false";
            }
            if (CheckBoxDisplayFolder.IsChecked == true)
            {
                Setting[4] = "true";
            }
              else
            {
                Setting[4] = "false";
            }


            if (CheckBoxDisplayFileName.IsChecked == true)
            {
                Setting[5] = "true";
               }
             else
            {
                Setting[5] = "false";
            }

            Setting[2] = TextBoxImageChangeTime.Text.ToString();
            RWSettingFile pri = new RWSettingFile();
            pri.WriteSetting(Setting);
        }
        

        public void Init()
        {
            bool check = false;
            //フォルダ確認
            if (System.IO.Directory.Exists(@"ImageFiles\"))
            {
                check = true;
                LabelStatus.Content = "";
                
            }
            else
            {
                MessageBox.Show("ImageFilesフォルダを作成しました。\nフォルダに画像を入れた後にソフトを再度起動して下さい。","Informatioon",MessageBoxButton.OK,MessageBoxImage.Information);
                check = false;
                Directory.CreateDirectory(@"ImageFiles\");
                System.Diagnostics.Process.Start(@"ImageFiles\");
                this.Close();
            }
            CheckImageSum();
            if (ImageSum == 0)
            {
                ButtonStartSlideShow.IsEnabled = false;
                LabelStatus.Content = "エラー: 画像ファイルが見つかりませんでした。";
                
            }
            
            LabelImageSum.Content = ImageSum.ToString();
        }

        public void CheckImageSum()
        {
            string dir = @"ImageFiles\";
            int fileCount = 0;
            fileCount = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories).Length;
            fileCount += Directory.GetFiles(dir, "*.jpeg", SearchOption.AllDirectories).Length;
            fileCount += Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories).Length;
            fileCount += Directory.GetFiles(dir, "*.gif", SearchOption.AllDirectories).Length;
            ImageSum = fileCount;
        }

        private void ButtonStartSlideShow_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
            SlideShow awin2 = new SlideShow();
            awin2.Show();
        }

        private void RadioButtonSortRandom_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxLoop.IsChecked = true;
            CheckBoxLoop.IsEnabled = false;
        }

        private void RadioButtonSort_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxLoop.IsEnabled = true;
        }
    }
  
  
}
