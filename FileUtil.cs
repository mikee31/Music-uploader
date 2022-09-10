using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace MusicUploader
{
    public class FileUtil
    {
        public const string DOWNLOADS_FOLDER = @"C:\Users\Mike\Downloads\";
        public const string MP3_TAG = "mp3-now.com";
        public const int LENGTH_OF_PATH_TO_REMOVE_MP3_TAG = 38;
        public static void FixFileNames()
        {
            string[] filePaths = GetAllFiles();

            for (int i = 0; i < filePaths.Count(); i++)
            {
                string file;
                if (filePaths[i].Contains(MP3_TAG))
                {
                    file = filePaths[i].Substring(LENGTH_OF_PATH_TO_REMOVE_MP3_TAG);
                    File.Move(filePaths[i], DOWNLOADS_FOLDER + file);
                }
            }

            filePaths = GetAllFiles();
            for (int i = 0; i < filePaths.Count(); i++)
            {
                string file;
                if (filePaths[i].Contains(".webm"))
                {
                    file = filePaths[i].Remove(filePaths[i].Count() - 5);
                    File.Move(filePaths[i], file);
                }
            }
        }
        
        public static string[] GetAllFiles()
        {
            return Directory.GetFiles(DOWNLOADS_FOLDER).Where(
                file => !file.Contains("desktop.ini"))
                .ToArray<string>();
            
        }

        public static void HandleFileExplorer(string[] songsToUpload, ServerUtil server)
        {
            server.MakeWindowBeInFocus();

            InputSimulator sim = new InputSimulator();
            sim.Keyboard.TextEntry(DOWNLOADS_FOLDER);
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
