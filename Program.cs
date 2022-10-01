using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome;
using System.Net;

namespace MusicUploader
{
    class Program
    {
        public const string SAVE_FILE = @"\infos.sav";
        public const string DOWNLOAD_FOLDER_NAME = "tempDownloads";
        public const string CHANGE_URL_MESSAGE = "Do you want to change the playlist url? (y/n)";
        public const string URL_PROMPT = "Enter the adress of the playlist you want to upload.";
        public const string IP_PROMPT = "Enter the ip adress where songs will be uploaded.";
        public const string FIRST_VID_TO_UPLOAD_PROMPT = "Which video is the first one you want to upload?";
        public static readonly string FFMPEG_PATH = Environment.CurrentDirectory + @"\ffmpeg.exe"; // to determine.
        public static readonly string DOWNLOAD_FOLDER_PATH = Environment.CurrentDirectory + @"\" + DOWNLOAD_FOLDER_NAME + @"\";
        public static readonly string VALIDATOR_URL = "https://www.youtube.com/oembed?format=json&url=";
        public static readonly Regex IP_REGEX = new Regex(@"^([0-9]{1,3}\.){3}[0-9]{1,3}$");
        public static ManualResetEvent resetEventStartUpload = new ManualResetEvent(false);
        public static ManualResetEvent resetEventUpload = new ManualResetEvent(false);
        public static ManualResetEvent resetEventDownload = new ManualResetEvent(false);

        /****************************************************************** TODO *******************************************************************/
        /* 
        * - A LOT and I mean a whole fucking lot of validation and exceptions.
        * 
        * - Save infos in json file.
        * - Change validation of youtube urls to whats in the tests project (use validation website instead of regex).
        * - Make it possible to upload video instead of whole playlist.
        * - Make it possible to download song without uploading it (and vice versa maybe?).
        * - Try to use ChromeDriver as little as possible. Try to make everything using the internet be made with no concrete browser or input sim.
        * - Maybe find a way to compile ffmpeg.exe as a smaller file with only the needed components.
        * - Document code cause its kind of a mess at the moment. Especially everything related to ManualResetEvents.
        * - Reorganize classes. Too many functions in Program.cs.
        * - Eventually make a GUI.
        */


        public static void Main(string[] args) 
        {
            Console.Clear();
                        
            string url = GetPlaylistUrl();
            int index = GetIndex();
            string ipAddress = GetUploadIp();
            Console.Clear();

            CancellationTokenSource cancelUpload = new CancellationTokenSource();

            Task download = Task.Run(() => StartDownloadProcess(url, index, DOWNLOAD_FOLDER_PATH));
            Task upload =  Task.Run(() => StartUploadProcess(ipAddress, cancelUpload.Token));


            download.Wait();
            cancelUpload.Cancel();
            upload.Wait();

            Environment.Exit(0);
        }
        
        public static void StartDownloadProcess(string listUrl, int startIndex, string downloadPath)
        {
            DownloadUtil.DownloadSongs(listUrl, startIndex, downloadPath);
        }

        public static void StartUploadProcess(string ipAdress, CancellationToken cancelToken)
        {
            resetEventStartUpload.WaitOne();
            ChromeDriver driver = new ChromeDriver();
            driver.GoToUrl("http://" + ipAdress);
            bool cancelled = false;
            string[] songs = new string[0];
            do
            {
                if (cancelToken.IsCancellationRequested)
                {
                    cancelled = true;
                }
                Program.resetEventUpload.WaitOne();
                Program.resetEventDownload.Reset();

                while (!UploadUtil.AreUploadsFinished(driver))
                {
                    Thread.Sleep(1000);
                }

                if (songs.Length > 0)
                {
                    FileUtil.DeleteFiles(songs);
                }
                songs = UploadUtil.UploadSongs(driver);
                Program.resetEventDownload.Set();
            } while (!cancelled);
            driver.Quit();
        }

        //Validation done for now.
        public static string GetPlaylistUrl()
        {
            if (File.Exists(Environment.CurrentDirectory + SAVE_FILE))
            {
                string url = File.ReadAllText(Environment.CurrentDirectory + SAVE_FILE);
                try
                {
                    Task.WaitAll(ValidateUrlAsync(url));
                } catch (Exception e)
                {
                    return changeUrl();
                }


                while (true)
                {
                    try
                    {
                        Console.WriteLine(CHANGE_URL_MESSAGE);
                        ConsoleKeyInfo ans = Console.ReadKey();
                        Console.WriteLine();
                        if (ans.Key == ConsoleKey.Y)
                            return changeUrl();
                        else if (ans.Key == ConsoleKey.N)
                            return url;
                        else
                            throw new InvalidInputException(ans.Key.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                return changeUrl();
            }
        }

        public static async Task ValidateUrlAsync(string youtubeUrl)
        {
            string url = VALIDATOR_URL + youtubeUrl;
            Uri uri = new Uri(url, UriKind.Absolute);
            HttpClient client = new HttpClient();
            HttpStatusCode status = (await client.GetAsync(uri)).StatusCode;
            if (status != System.Net.HttpStatusCode.OK)
                throw new InvalidInputException(youtubeUrl);
        }

        //Validation done for now.
        public static string changeUrl()
        {
            while (true)
                {
                    Console.WriteLine(URL_PROMPT);
                    string url = Console.ReadLine();
                    try
                    {
                        Task.WaitAll(ValidateUrlAsync(url));

                        File.WriteAllText(Environment.CurrentDirectory + SAVE_FILE, url);
                        return url;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
        }

        //Validation done for now.
        public static string GetUploadIp()
        {
            while (true)
            {
                try 
                {
                    Console.WriteLine(IP_PROMPT);
                    string ipAdress = Console.ReadLine();
                    Match match = IP_REGEX.Match(ipAdress);
                    if (match.Success)
                    {
                        return ipAdress;
                    }
                    else
                    {
                        throw new InvalidInputException(ipAdress);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //Validation done for now.
        public static int GetIndex()
        {
            while(true)
            {
                try {
                    Console.WriteLine(FIRST_VID_TO_UPLOAD_PROMPT);
                    string input = Console.ReadLine();
                    int i = -1;
                    if (int.TryParse(input, out i))
                        return i - 1;
                    else
                        throw new InvalidInputException(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}