using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OptifineDownloader.Utils
{
    abstract class WebHelper
    {
        static readonly HttpClient client = new HttpClient();
        public static string GetHtml(string url)
        {
            try
            {
                using HttpResponseMessage response = client.GetAsync(url).Result;
                string html = response.Content.ReadAsStringAsync().Result;
                return html;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async static Task DownloadOptifine(string downloadLink, string downloadPath, string name)
        {
            var res = await client.GetAsync(downloadLink);
            using (var file = System.IO.File.Create(downloadPath + @"\" + name + ".jar"))
            {
                var contentStream = await res.Content.ReadAsStreamAsync();
                await contentStream.CopyToAsync(file);
            }
        }
    }
}
