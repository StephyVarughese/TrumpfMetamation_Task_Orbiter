using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TrumpfMetamation_Task_Orbiter.Pages.DownloadPage;

namespace TrumpfMetamation_Task_Orbiter.UI_Test
{
    public class APITest
    {
        [Test]
        [Description("Downloading the File using API")]
        [Author("Stephy Anna Varughese")]
        [Category("API")]
        public async Task API_OrbiterDownloadFileTest()
        {
            string[] urls = {
                "https://orbiter-for-testing.azurewebsites.net/products/testApp?isInternal=false",
                "https://orbiter-for-testing.azurewebsites.net/products/testApp?isInternal=true"
            };
            //where i download the files
            string savePath = @"C:\Users\svarugh\Downloads\";
            using (var client = new HttpClient())
            {
                foreach (var url in urls)
                {
                    try
                    {
                        FileDownload fileDowload = new FileDownload();
                        var response = await client.GetStringAsync(url);

                        var regex = new Regex(@"href=""(https?://[^\s""]+)""");
                        var matches = regex.Matches(response);

                        List<string> fileLinks = new List<string>();
                        foreach (Match match in matches)
                        {
                            var fileLink = match.Groups[1].Value;
                            List<string> formats = new List<string> { ".zip", ".pdf", ".exe", ".jpg", ".png" };
                            bool ValidFileFormat = formats.Contains(Path.GetExtension(url)?.ToLower());

                            if (ValidFileFormat)
                            {
                                fileLinks.Add(fileLink);
                            }
                        }

                        foreach (var fileLink in fileLinks)
                        {
                            if (fileLink.StartsWith("http"))
                            {
                                Console.WriteLine($"Found file: {fileLink}");
                                bool downloadSuccess = FileDownload.DownloadFiles(new List<string> { fileLink }, savePath);
                                if (downloadSuccess)
                                {
                                    Console.WriteLine($"File downloaded successfully: {fileLink}");
                                }
                                else
                                {
                                    Console.WriteLine($"Failed to download file: {fileLink}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error downloading files from {url}: {ex.Message}");
                    }
                }
            }
        }
    }
}
