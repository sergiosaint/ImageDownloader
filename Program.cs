using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ImageDownloader
{
    class Program
    {
        static string[] urls = {
            "https://www.pediportugal.com/laura-e-andre-fotos-em-casa-dos-noivos",
            "https://www.pediportugal.com/laura-e",
            "https://www.pediportugal.com/laura-e-andre-noivos",
            "https://www.pediportugal.com/laura-e-andre-sem-noivos" };

        static void Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                foreach (var url in urls)
                {
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string pageCode = content.ReadAsStringAsync().Result;

                            foreach (Match m in Regex.Matches(pageCode, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                            {
                                string src = m.Groups[1].Value;

                                try
                                {
                                    var dotIndex = src.IndexOf(".jpg");
                                    if (dotIndex == -1) {
                                        Console.WriteLine("No Image: " + src);
                                        continue;
                                    }
                                    var imgUrl = src.Substring(0, dotIndex) + ".jpg";

                                    using (WebClient webClient = new WebClient())
                                    {
                                        var filenName = imgUrl.Substring(35);
                                        webClient.DownloadFile(imgUrl, filenName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
