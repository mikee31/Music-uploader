using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace MusicUploader
{
    class Program
    {
        public const string SAVE_FILE = @"\infos.sav";
        public const string INVALID_URL_MESSAGE = "This URL is not valid.";
        public const string CHANGE_URL_MESSAGE = "Do you want to change the playlist url? (y/n)";
        public const string WARNING = "*** You need to make sure your default downloads folder is empty. The program won't work otherwise. Press any key to continue. ***";
        public const string URL_PROMPT = "Enter the adress of the playlist that you want to upload.";
        public const string IP_PROMPT = "Enter the ip adress where songs will be uploaded.";
        public const string FIRST_VID_TO_UPLOAD_PROMPT = "Which video is the first one you want to upload?";
        public static readonly Regex IP_REGEX = new Regex(@"^([0-9]{1,3}\.){3}[0-9]{1,3}$");
        public static readonly Regex URL_REGEX = new Regex(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube(-nocookie)?\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$");
        public static ManualResetEvent resetEventStartUpload = new ManualResetEvent(false);
        public static ManualResetEvent resetEventUpload = new ManualResetEvent(false);
        public static ManualResetEvent resetEventDownload = new ManualResetEvent(false);

        // TODO : a LOT and I mean a whole fucking lot of validation and exceptions.
        public static void Main(string[] args) 
        {
            Console.Clear();
                        
            string url = DeterminePlaylistUrl();
            int index = PromptIndex();
            string ipAddress = DetermineUploadIp();
            Console.WriteLine(WARNING);
            Console.ReadKey();
            Console.Clear();

            CancellationTokenSource cancelUpload = new CancellationTokenSource();

            Task download = Task.Run(() => StartDownloadProcess(url, index));
            Task upload =  Task.Run(() => StartUploadProcess(ipAddress, cancelUpload.Token));


            download.Wait();
            cancelUpload.Cancel();
            upload.Wait();

            Environment.Exit(0);
        }

        public static void StartDownloadProcess(string listUrl, int startIndex)
        {
            ServerUtil server = new ServerUtil();
            resetEventStartUpload.Set();
            server.DownloadSongs(listUrl, startIndex);
            server.QuitDriver();
        }

        public static void StartUploadProcess(string ipAdress, CancellationToken cancelToken)
        {
            resetEventStartUpload.WaitOne();
            ServerUtil server = new ServerUtil();
            server.GoToUrl("http://" + ipAdress);
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

                while (!server.AreUploadsFinished())
                {
                    Thread.Sleep(1000);
                }

                if (songs.Length > 0)
                {
                    FileUtil.DeleteFiles(songs);
                }
                songs = server.UploadSongs();
                Program.resetEventDownload.Set();
            } while (!cancelled);
            server.QuitDriver();
        }
        
        //Validation done for now.
        public static string DeterminePlaylistUrl()
        {
            if (File.Exists(Environment.CurrentDirectory + SAVE_FILE))
            {
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
                        return File.ReadAllText(Environment.CurrentDirectory + SAVE_FILE);
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

        //Validation done for now.
        public static string changeUrl()
        {
            while (true)
                {
                    try
                    {
                        Console.WriteLine(URL_PROMPT);
                        string url = Console.ReadLine();
                        Match match = URL_REGEX.Match(url);
                        if (match.Success)
                        {
                            File.WriteAllText(Environment.CurrentDirectory + SAVE_FILE, url);
                            return url;
                        }
                        else
                        {
                            throw new InvalidInputException(url);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
        }

        //Validation done for now.
        public static string DetermineUploadIp()
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
        public static int PromptIndex()
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