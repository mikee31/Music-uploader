using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MusicUploader
{
    public static class UploadUtil
    {
        public static bool AreUploadsFinished(ChromeDriver driver)
        {
            int nbOfUploads = int.Parse(driver.FindElement(By.Id("uploads")).GetDomProperty("childElementCount"));
            if (nbOfUploads > 0)
                return false;
            else
                return true;
        }
        

        public static string[] UploadSongs(ChromeDriver driver)
        {
            FileUtil.FixFileNames();
            string[] allFiles = FileUtil.GetAllFiles();
            if (allFiles.Length > 0)
            {
                driver.FindElement(By.Id("upload-file")).Click();
                FileUtil.HandleFileExplorer(allFiles, driver);
            }
            return allFiles;
        }
    }
}