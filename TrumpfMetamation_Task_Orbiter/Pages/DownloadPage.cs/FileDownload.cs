using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Threading;

namespace TrumpfMetamation_Task_Orbiter.Pages.DownloadPage.cs
{
    public class FileDownload
    {
        public void DownloadAndDelete(IWebDriver driver, IReadOnlyList<IWebElement> Items)
        {
            string folder = @"C:\Users\jinog\Downloads\";

            foreach (var Item in Items)
            {
                string downloadLink = Item.FindElement(By.TagName("a")).GetAttribute("href");
                string fileName = Path.GetFileName(downloadLink);
                string filePath = Path.Combine(folder, fileName);
                driver.Navigate().GoToUrl(downloadLink);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(200));
                Thread.Sleep(500);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"File {fileName} downloaded and deleted at: {filePath}");

                }
                else
                {
                     throw new FileNotFoundException($"File {fileName} did not download correctly at {filePath}");
                }
            }

        }
    }
}
