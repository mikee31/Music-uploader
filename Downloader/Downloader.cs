using FFmpeg.NET;
using System.Collections;
using VideoLibrary;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MusicUploaderGUI
{
    public class Downloader : IDownloader
    {
        private const string FFMPEG_PATH = @"D:\Documents\C#\Source\MusicUploader3\MusicUploaderGUI\ffmpeg.exe";
        public const string DOWNLOAD_FOLDER_NAME = "downloads";
        public static readonly string DOWNLOAD_FOLDER_PATH = Environment.CurrentDirectory + @"\" + DOWNLOAD_FOLDER_NAME + @"\";

        private FileUtil fileUtil;
        private IYoutubeAPI ytApi;

        public Downloader()
        {
            this.fileUtil = new FileUtil();
            this.ytApi = new YoutubeAPI();
        }

        public async void Download(string url, int index)
        {https://www.youtube.com/watch?v=LwnlLxMtoWw&list=PLEFrgFvmdtvtH_pCEKPX1B6wIpcDZv1z6
            this.fileUtil.CreateDownloadFolder();
            Engine ffmpeg = new Engine(FFMPEG_PATH);

            YoutubeClient ytClient = new YoutubeClient();


            // TODO : might be a good idea to make a class for this eventually.
            // Key : name of video, Value : URL of video
            List<KeyValuePair<string, string>> videosInfo = ytApi.GetVideoNamesAndUrls(url);

            for (int i = index; i < videosInfo.Count; i++)
            {
                StreamManifest streamManifest = await ytClient.Videos.Streams.GetManifestAsync(videosInfo[i].Value);
                var stream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                
                // TODO : get video names
                await ytClient.Videos.Streams.DownloadAsync(stream, DOWNLOAD_FOLDER_PATH + videosInfo[i].Key + ".mp3");
            }
            Console.WriteLine("Done");
        }
    }
}
