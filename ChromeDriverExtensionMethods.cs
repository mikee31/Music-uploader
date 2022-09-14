using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WindowsInput;
using WindowsInput.Native;
using System.Collections.ObjectModel;

namespace MusicUploader
{
    public static class ChromeDriverExtensionMethods
    {
        private static void turnOnImplicitWait(this ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);
        }

        private static void turnOffImplicitWait(this ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }

        public static void GoToUrl(this ChromeDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }

         public static void MakeWindowBeInFocus(this ChromeDriver driver)
        {
            driver.Manage().Window.Maximize();
            Thread.Sleep(500);
            InputSimulator sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(2000, 2000);
            sim.Mouse.LeftButtonClick();
        }
        public static ReadOnlyCollection<IWebElement> GetElementsbyCssSelector(this ChromeDriver driver, string cssSelector)
        {
            return driver.FindElements(By.CssSelector(cssSelector));
            
        }

        private static void RemoveAds(this ChromeDriver driver)
        {
            driver.turnOffImplicitWait();
            ReadOnlyCollection<IWebElement> ads = driver.FindElements(By.CssSelector("iframe"));

            for (int i = ads.Count() - 1; i >= 0; i--)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"return document.querySelectorAll('iframe')[{i}].remove();");
            }
            driver.turnOnImplicitWait();
        }

        public static void CloseUselessStuff(this ChromeDriver driver, string correctHandle)
        {
            Thread.Sleep(1500);
            if (driver.WindowHandles.Count() > 1)
            {
                for (int i = 0; i < driver.WindowHandles.Count(); i++)
                {
                    if(correctHandle != driver.WindowHandles.ElementAt(i))
                    {
                        driver.SwitchTo().Window(driver.WindowHandles.ElementAt(i));
                        driver.Close();
                        i = 0;
                    }
                }
                driver.SwitchTo().Window(correctHandle);
            }
            driver.RemoveAds();
        }   
    }
}