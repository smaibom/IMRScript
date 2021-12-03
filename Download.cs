using System.Net;
using System.Collections.Concurrent;

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

        public async Task<Boolean> RunDownloads(string id, string url1, string url2, ConcurrentBag<(string,string,string)> result,string filepath)
        {

            (string,string) res = await this.downloadFile(url1,filepath,id);
            if(res.Item1 == "no")
            {
                (string,string) res2 = await this.downloadFile(url2,filepath,id);
                if(res2.Item1 == "no")
                {
                    result.Add((res2.Item1,res.Item1,res.Item2));
                }
                else
                {
                    result.Add((res2.Item1,"",""));
                }
            }
            else
            {
                result.Add((res.Item1,"",""));
            }
            return true;
        }
        public async Task<(string,string)> downloadFile(string url,string outputDir, string id){
            if(url.Length == 0){
                Console.WriteLine("Empty url");
                return ("no","Empty URL");
            }
            string filepath = outputDir + id + ".pdf";
            if(File.Exists(filepath))
            {
                return ("yes","");
            }

            try
            {
                HttpResponseMessage req = await client.GetAsync(url);
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
                return ("no","SomeError"); 
            }
            catch(System.Net.Http.HttpRequestException)
            {
                return ("no","Host does not exist");
            }
            catch(System.Threading.Tasks.TaskCanceledException)
            {
                return ("no","Timeout error");
            }
            catch(System.InvalidOperationException)
            {
                return ("no","Invalid URL");
            }
            catch(System.UriFormatException)
            {
                return ("no","Invalid URL");
            }
            catch (Exception e)
            {  
                Console.WriteLine(e.ToString());
                return ("no",e.ToString());
            }

            return ("yes","");
        }
    }
}