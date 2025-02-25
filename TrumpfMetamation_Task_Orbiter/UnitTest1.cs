using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TrumpfMetamation_Task_Orbiter.Pages.DownloadPage.cs;

namespace TrumpfMetamation_Task_Orbiter
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OrbiterDownloadFileTest()
        {
            IWebDriver driver = new ChromeDriver();
            //Why I used as Array , to avoid the Duplicate Lines of the Code , workouting the Iteration 
            string[] urls = {
                "https://orbiter-for-testing.azurewebsites.net/products/testApp?isInternal=false",
                "https://orbiter-for-testing.azurewebsites.net/products/testApp?isInternal=true"
            };
            foreach (var url in urls)
            {

                driver.Navigate().GoToUrl(url);
                IWebElement DownloadTestAppLbl = driver.FindElement(By.XPath("//h1[normalize-space(text())='Download TestApp']"));
                Assert.True(DownloadTestAppLbl.Displayed, "Download TestApp is not displayed");

                var listItemsTestApp = driver.FindElements(By.XPath("//ul[@class='list-group list-group-flush']/li"));
                Console.WriteLine($"Files in List: {listItemsTestApp.Count}");

                FileDownload fileDownload = new FileDownload();
                fileDownload.DownloadAndDelete(driver, listItemsTestApp);
                Thread.Sleep(500);
            }

            driver.Quit();
        }
    }
}
