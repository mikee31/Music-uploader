using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WindowsInput;
using WindowsInput.Native;
using System.Collections.ObjectModel;

namespace MusicUploader
{
   public static class DownloadUtil
    {
        private const string MP3_NOW_URL = "https://mp3-now.com";
        private const string SAVE_MP3_URL = "https://savemp3.net";

        private const string MP3_URL = MP3_NOW_URL; // It's possible to use MP3_NOW_URL or SAVE_MP3_URL. 
        //                                             Mp3-now has issues every once in a while though.
        //                                             SAVE_MP3_URL not implemented.

        public static List<string> GetVideoUrls(string playlistUrl, int startIndex)
        {
            ChromeDriver driver = new ChromeDriver();
            driver.GoToUrl(playlistUrl);
            InputSimulator sim = new InputSimulator();
            
            int nbOfVideos;
            int newNbbOfVideos;

            do 
            {
                nbOfVideos = driver.GetElementsbyCssSelector("#video-title").Count();
                driver.MakeWindowBeInFocus();
                for (int i = 0; i <= 240; i++)
                {
                    sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                }
                Thread.Sleep(2000);
                newNbbOfVideos = driver.GetElementsbyCssSelector("#video-title").Count();
            } while (nbOfVideos != newNbbOfVideos);
          
            ReadOnlyCollection<IWebElement> videos = driver.GetElementsbyCssSelector("#video-title");
            List<string> urls = new List<string>();
            
            for (int i = startIndex; i < videos.Count(); i++)
            {
                string videoUrl = GetUrlOfVideo(videos.ElementAt(i));
                urls.Add(videoUrl);
            }
            
            driver.Quit();
            return urls;
        }

        public static void DownloadSongs(string listUrl, int startIndex)
        {
            ChromeDriver driver = new ChromeDriver();
            List<string> urls = GetVideoUrls(listUrl, startIndex);
            driver.GoToUrl(MP3_URL);
            DownloadSongsUsingMp3Now(urls, driver);
            driver.Quit();
        }

         private static void DownloadSongsUsingMp3Now(List<string> urls, ChromeDriver driver)
        {
            string handle = driver.CurrentWindowHandle;
            for (int i = 0; i < urls.Count(); i++)
            {
                Thread.Sleep(500);
                driver.FindElement(By.Id("k_query")).SendKeys(urls.ElementAt(i) + Keys.Enter);
                driver.CloseUselessStuff(handle);
                driver.FindElement(By.Id("btn-start-convert")).Click();
                driver.CloseUselessStuff(handle);
                driver.FindElement(By.CssSelector(".btn-success.btn-orange")).Click();
                driver.CloseUselessStuff(handle);
                driver.FindElement(By.CssSelector(".btn-success.btn-blue")).Click();
                driver.CloseUselessStuff(handle);
                if (i != 0 && i % 10 == 0)
                {
                    Program.resetEventUpload.Set();
                    Program.resetEventUpload.Reset();
                    Thread.Sleep(500);
                    Program.resetEventDownload.WaitOne();
                }
            }
            Thread.Sleep(3000);
            Program.resetEventUpload.Set();
        }

        private static void DownloadSongsUsingSaveMp3(List<string> urls)
        {
            //TODO
        }

        private static string GetUrlOfVideo(IWebElement elem)
        {
            return elem.GetAttribute("href");
        }
    }
}