using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using WindowsInput;
using WindowsInput.Native;

namespace music
{
    public class ServerUtil
    {
        private const string MP3_NOW_URL = "https://mp3-now.com";
        private const string SAVE_MP3_URL = "https://savemp3.net";

        private const string MP3_URL = MP3_NOW_URL; // It's possible to use MP3_NOW_URL or SAVE_MP3_URL. 
        //                                             Mp3-now has issues every once in a while though.
        //                                             SAVE_MP3_URL not implemented.
        private ChromeDriver driver;
        public ServerUtil()
        {
            this.driver = new ChromeDriver();
            this.turnOnImplicitWait();

        }

        private void turnOnImplicitWait()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);
        }

        private void turnOffImplicitWait()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }

        public void GoToUrl(string url)
        {
            this.driver.Navigate().GoToUrl(url);
        }

        public List<string> GetVideoUrls(string playlistUrl, int startIndex)
        {
            this.GoToUrl(playlistUrl);
            InputSimulator sim = new InputSimulator();
            
            int nbOfVideos;
            int newNbbOfVideos;

            do 
            {
                nbOfVideos = this.GetElementsbyCssSelector("#video-title").Count();
                this.MakeWindowBeInFocus();
                for (int i = 0; i <= 240; i++)
                {
                    sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                }
                Thread.Sleep(2000);
                newNbbOfVideos = this.GetElementsbyCssSelector("#video-title").Count();
            } while (nbOfVideos != newNbbOfVideos);
          
            ReadOnlyCollection<IWebElement> videos = this.GetElementsbyCssSelector("#video-title");
            List<string> urls = new List<string>();
            
            for (int i = startIndex; i < videos.Count(); i++)
            {
                string videoUrl = this.GetUrlOfVideo(videos.ElementAt(i));
                urls.Add(videoUrl);
            }

            return urls;
        }

        public void MakeWindowBeInFocus()
        {
            driver.Manage().Window.Maximize();
            Thread.Sleep(500);
            InputSimulator sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(2000, 2000);
            sim.Mouse.LeftButtonClick();
        }

        public void DownloadSongs(List<string> urls)
        {
            this.GoToUrl(MP3_URL);
            if (MP3_URL == MP3_NOW_URL)
                DownloadSongsUsingMp3Now(urls);
            else
                DownloadSongsUsingSaveMp3(urls);
        }

        private void DownloadSongsUsingMp3Now(List<string> urls)
        {
            string handle = this.driver.CurrentWindowHandle;
            for (int i = 0; i < urls.Count(); i++)
            {
                Thread.Sleep(500);
                this.driver.FindElement(By.Id("k_query")).SendKeys(urls.ElementAt(i) + Keys.Enter);
                this.CloseUselessStuff(handle);
                this.driver.FindElement(By.Id("btn-start-convert")).Click();
                this.CloseUselessStuff(handle);
                this.driver.FindElement(By.CssSelector(".btn-success.btn-orange")).Click();
                this.CloseUselessStuff(handle);
                this.driver.FindElement(By.CssSelector(".btn-success.btn-blue")).Click();
                this.CloseUselessStuff(handle);
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

        private void DownloadSongsUsingSaveMp3(List<string> urls)
        {
            //TODO
        }

        public bool AreUploadsFinished()
        {
            int nbOfUploads = int.Parse(this.driver.FindElement(By.Id("uploads")).GetDomProperty("childElementCount"));
            if (nbOfUploads > 0)
                return false;
            else
                return true;
        }
        

        public string[] UploadSongs()
        {
            FileUtil.FixFileNames();
            string[] allFiles = FileUtil.GetAllFiles();
            if (allFiles.Length > 0)
            {
                this.driver.FindElement(By.Id("upload-file")).Click();
                FileUtil.HandleFileExplorer(allFiles, this);
            }
            return allFiles;
        }

        public void QuitDriver()
        {
            this.driver.Quit();
        }

        private void CloseUselessStuff(string correctHandle)
        {
            System.Threading.Thread.Sleep(1500);
            if (this.driver.WindowHandles.Count() > 1)
            {
                for (int i = 0; i < this.driver.WindowHandles.Count(); i++)
                {
                    if(correctHandle != this.driver.WindowHandles.ElementAt(i))
                    {
                        this.driver.SwitchTo().Window(this.driver.WindowHandles.ElementAt(i));
                        this.driver.Close();
                        i = 0;
                    }
                }
                this.driver.SwitchTo().Window(correctHandle);
            }
            this.RemoveAds();
        }

        private void RemoveAds()
        {
            this.turnOffImplicitWait();
            ReadOnlyCollection<IWebElement> ads = this.driver.FindElements(By.CssSelector("iframe"));

            for (int i = ads.Count() - 1; i >= 0; i--)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)this.driver;
                js.ExecuteScript($"return document.querySelectorAll('iframe')[{i}].remove();");
            }
            this.turnOnImplicitWait();
        }

        private string GetUrlOfVideo(IWebElement elem)
        {

            return elem.GetAttribute("href");
        }

        private ReadOnlyCollection<IWebElement> GetElementsbyCssSelector(string cssSelector)
        {
            return this.driver.FindElements(By.CssSelector(cssSelector));
            
        }
    }

}