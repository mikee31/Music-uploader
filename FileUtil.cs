using WindowsInput;
using WindowsInput.Native;
using OpenQA.Selenium.Chrome;

namespace MusicUploader
{
    public static class FileUtil
    {
        
        public static string[] GetAllFiles()
        {
            return Directory.GetFiles(Program.DOWNLOAD_FOLDER_PATH).ToArray<string>();
            
        }

        public static void HandleFileExplorer(string[] songsToUpload, ChromeDriver driver)
        {
            driver.MakeWindowBeInFocus();

            InputSimulator sim = new InputSimulator();
            sim.Keyboard.TextEntry(Program.DOWNLOAD_FOLDER_PATH);
            Thread.Sleep(200);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(200);
            sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
            sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
            sim.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
            Thread.Sleep(200);
            sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            sim.Keyboard.KeyPress(VirtualKeyCode.VK_A);
            sim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            Thread.Sleep(200);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(200);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(500);
        }

        public static void DeleteFiles(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                File.Delete(paths[i]);
            }
        }
    }    
}
