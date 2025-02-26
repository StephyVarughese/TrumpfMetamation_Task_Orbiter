using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace TrumpfMetamation_Task_Orbiter.Pages.DownloadPage
{
    public class FileDownload
    {
        public void DownloadAndDelete(IWebDriver driver, IReadOnlyList<IWebElement> Items)
        {
            string folder = @"C:\Users\svarugh\Downloads\";

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

        public static bool DownloadFiles(List<string> urls, string savePath)
        {
            var client = new RestClient();

            foreach (var url in urls)
            {
                try
                {
                    var request = new RestRequest(url, Method.Get);
                    var response = client.Execute(request);

                    if (response.IsSuccessful && response.RawBytes != null)
                    {
                        var fileName = Path.Combine(savePath, Path.GetFileName(url));

                        File.WriteAllBytes(fileName, response.RawBytes);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download from {url} with status code: {response.StatusCode}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading file from {url}: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

    }
}
