using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public class FileUtil
    {

        private const string DOWNLOAD_FOLDER_NAME = "downloads";
        private readonly string DOWNLOAD_FOLDER_PATH = Environment.CurrentDirectory + @"\" + DOWNLOAD_FOLDER_NAME + @"\";

        public const string SAVE_FILE = @"\infos.sav";
        private readonly string savedInfoPath = Directory.GetCurrentDirectory() + SAVE_FILE;

        public FileUtil()
        {

        }

        public string GetTextFromSaveFile()
        {
            return File.ReadAllText(savedInfoPath);
        }

        public string GetDownloadsFolderPath()
        {
            return DOWNLOAD_FOLDER_PATH;
        }

        public List<string> GetAllMp3s()
        {
            List<string> files = Directory.EnumerateFiles(DOWNLOAD_FOLDER_PATH).ToList<string>();
            return files;
        }

        public void CreateDownloadFolder()
        {
            Directory.CreateDirectory(DOWNLOAD_FOLDER_PATH);
        }


        // Will need to get rid of that.
        public string GetSavedUrl()
        {
            return File.ReadAllText(savedInfoPath);
        }

        // wrong
        public void SaveUrl(string url)
        {
            File.WriteAllText(savedInfoPath, url);
        }

        // wrong
        public void SaveIpAddress(string ip)
        {
            File.WriteAllText(savedInfoPath, ip);
        }

        /// <summary>
        ///     Deletes a file once a given task is completed.
        /// </summary>
        /// <param name="path">The path to the file to delete.</param>
        /// <param name="task">The task to await before deleting the file.</param>
        /// <returns></returns>
        public async Task DeleteFileAfterTaskCompletionAsync(string path, Task task)
        {
            await task;
            File.Delete(path);
        }
    }
}