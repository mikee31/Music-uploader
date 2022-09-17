using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WindowsInput;

namespace MusicUploader
{
    
    public static class ChromedriverExtensionMethods
    {
        //TODO : handle connection timeout exception.
        public static void GoToUrl(this ChromeDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
            turnOnImplicitWait(driver);
        }

        // Not reliable. Doesn't always work.
         public static void MakeWindowBeInFocus(this ChromeDriver driver)
        {
            driver.Manage().Window.Maximize();
            Thread.Sleep(500);
            InputSimulator sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(2000, 2000);
            sim.Mouse.LeftButtonClick();
        }

        private static void turnOnImplicitWait(this ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);
        }

        private static void turnOffImplicitWait(this ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }

        public static ReadOnlyCollection<IWebElement> GetElementsbyCssSelector(this ChromeDriver driver, string cssSelector)
        {
            return driver.FindElements(By.CssSelector(cssSelector));
        }
        
    }
}