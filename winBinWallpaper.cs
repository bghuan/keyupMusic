using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace keyupMusic
{
    public class winBinWallpaper
    {
        static string picUrl = "https://bghuan.cn/api/bing.php";
        static void Main1111(string[] args)
        {
            changeImg();
        }

        public static void changeImg()
        {
            string savePath = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".jpg";
            if (File.Exists(savePath)) return;

            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            string value = String.Empty;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                WebClient mywebclient = new WebClient();
                mywebclient.DownloadFile(picUrl, savePath);
                value = savePath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }


            int nResult;
            if (File.Exists(value))
            {

                nResult = WinAPI.SystemParametersInfo(20, 1, value, 0x1 | 0x2); //更换壁纸
                if (nResult == 0)
                {
                    Console.WriteLine("err");
                }
                else
                {
                    RegistryKey hk = Registry.CurrentUser;
                    RegistryKey run = hk.CreateSubKey(@"Control Panel\Desktop\");
                    run.SetValue("Wallpaper", value);  //将新图片路径写入注册表
                    Console.WriteLine("success");
                }
            }
            else
            {
                Console.WriteLine("not exist");
            }
        }

        public class WinAPI
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
            public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }
    }
}