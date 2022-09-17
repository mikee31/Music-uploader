using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WindowsInput;
using WindowsInput.Native;
using System.Collections.ObjectModel;
using FFmpeg.NET;
using VideoLibrary;

namespace MusicUploader
{
    public static class DownloadUtil
    {

        public static void DownloadSongs(string listUrl, int startIndex, string downloadPath)
        {
            List<string> urls = GetVideoUrls(listUrl, startIndex);
            Program.resetEventStartUpload.Set();
            DownloadSongsUsingVideoLibrary(urls, downloadPath);
        }

        private static List<string> GetVideoUrls(string playlistUrl, int startIndex)
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

            driver.Quit(); // Do not move. Quitting driver destroys "videos".
            return urls;
        }

        // not tested properly yet
        private static void DownloadSongsUsingVideoLibrary(List<string> videoUrls, string downloadPath)
        {
            Directory.CreateDirectory(downloadPath);
            Engine ffmpeg = new Engine(Program.FFMPEG_PATH);
            YouTube yt = new YouTube();

            for (int i = 0; i < videoUrls.Count(); i++)
            {
                YouTubeVideo video = yt.GetVideo(videoUrls.ElementAt(i)); // gets a Video object with info about the video
                
                string mp4Path = downloadPath + video.FullName;
                
                // changes file extension from .mp4 to .mp3 by removing the last character of the path (4) and replacing it with 3.
                string mp3Path = mp4Path.Remove(mp4Path.Length - 1) + "3";
                
                byte[] vidData = video.GetBytes();

                //not working for some reason.
                File.WriteAllBytes(mp4Path, vidData);

                MediaFile inputFile = new MediaFile (mp4Path);
                MediaFile outputFile = new MediaFile (mp3Path);
                

                // ConvertAsync() is fast enough that having it async is not worth the hassle.
                Task.Run(() => ffmpeg.ConvertAsync(inputFile, outputFile)).Wait();
                File.Delete(mp4Path);
                
                if (i != 0 && i % 10 == 0)
                {
                    Program.resetEventUpload.Set();
                    Program.resetEventUpload.Reset();
                    Thread.Sleep(500);
                    Program.resetEventDownload.WaitOne();
                }
            }

            Program.resetEventUpload.Set();
        }

        public static string GetUrlOfVideo(IWebElement elem)
        {
            return elem.GetAttribute("href");
        }
    }
}