using System.Net;
using System.Collections.Concurrent;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;

namespace IMRScript
{
    public class Download
    {
        private HttpClient client;
        public  Download()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
        }

        public Boolean CheckPDF(string filepath)
        {
            try
            {
                using(PdfReader pdf = new PdfReader(filepath))
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<(Boolean,string)> DownloadFile(string url,string outputDir, string id){
            if(url.Length == 0){
                return (false,"Empty URL");
            }
            string filepath = outputDir + id + ".pdf";
            if(File.Exists(filepath))
            {
                return (true,"");
            }

            try
            {
                HttpResponseMessage req = await client.GetAsync(url).ConfigureAwait(false);
                if(req.IsSuccessStatusCode)
                {
                    using (var fs = new FileStream(filepath,FileMode.CreateNew))
                    {
                        await req.Content.CopyToAsync(fs);
                    }
                }
            }
            catch (System.NotSupportedException)
            {
                return (false,"SomeError"); 
            }
            catch(System.Net.Http.HttpRequestException)
            {
                return (false,"Host does not exist");
            }
            catch(System.Threading.Tasks.TaskCanceledException)
            {
                return (false,"Timeout error");
            }
            catch(System.InvalidOperationException)
            {
                return (false,"Invalid URL");
            }
            catch(System.UriFormatException)
            {
                return (false,"Invalid URL");
            }
            catch (Exception e)
            {  
                Console.WriteLine(e.ToString());
                return (false,e.ToString());
            }
            if(!CheckPDF(filepath))
            {
                File.Delete(filepath);
                return (false,"Invalid pdf or link");
            }

            return (true,"");
        }
    }
}