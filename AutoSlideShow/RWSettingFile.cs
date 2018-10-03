using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoSlideShow
{
    class RWSettingFile
    {
        public string filepath = "settings.ini";
        public int x, g = 0;
        public string[] ReadSetting()
        {
            //string[] Setting={"Random(bool)","Loop(bool)","SwitchTime(int)","ShowClock(bool)"};
            string[] Setting = { "false", "true", "5", "true","true","false" };
            string[] Setting1 = { "false", "true", "5", "true","true","false" };
            if (System.IO.File.Exists(filepath))
            {
                try
                {
                    // csvファイルを開く
                    using (var sr = new System.IO.StreamReader(filepath, System.Text.Encoding.GetEncoding("utf-8")))
                    {

                        // ストリームの末尾まで繰り返す
                        while (!sr.EndOfStream)
                        {
                            // ファイルから一行読み込む
                            string line = sr.ReadToEnd();
                            // 読み込んだ一行をカンマ毎に分けて配列に格納する
                            Setting1 = line.Split(',');

                        }

                        foreach (string value in Setting1)
                        {
                            if (value != "")
                            {
                                string value1;
                                // MessageBox.Show(value);
                                value1 = value.Replace(Environment.NewLine, "");
                                Setting[x] = value1;
                                //MessageBox.Show("Setting1 " + g + "\n" + "Value=" + value1);
                                x++;
                            }
                            g++;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    // ファイルを開くのに失敗したとき
                    System.Console.WriteLine(e.Message);
                }
            }
            else
            {
                //create file
                WriteSetting(Setting);
            }


            return Setting;
        }

        public void WriteSetting(string[] Setting)
        {
            // string[] Setting = { "Random", "Loop", "SwitchTime", "ShowClock" };
            try
            {
                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = false;
                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(filepath, append, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    for (int i = 0; i < Setting.Length; ++i)
                    {
                        sw.WriteLine("{0},", Setting[i]);
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                // System.Console.WriteLine(e.Message);
            }

        }



    }
}
