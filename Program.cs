using System;
using System.Threading;
using System.Collections.Concurrent;


namespace IMRScript
{


    class Program
    {
        static void  Main(string[] args)
        {
            ConcurrentQueue<(string,string,string)> cq = new ConcurrentQueue<(string,string,string)>();
            ConcurrentBag<(string,string,string)> result = new ConcurrentBag<(string, string, string)>();
            Download dl = new Download();
            string filepath = @"C:\Users\KOM\Documents\IMRScript\Downloads\";
            string fileurlpath = @"C:\Users\KOM\Documents\IMRScript\output.csv";
            
            ImportExcel openor = new ImportExcel();
            List<(string,string,string)> urls = openor.getLinks(fileurlpath);
            for(int i = 0; i < urls.Count; i++)
            {
                cq.Enqueue((urls[i].Item1,urls[i].Item2,urls[i].Item3));
            }


            
            Action  action = () =>
            {
                (string,string,string) localValue;
                Download dl = new Download();
                while (cq.TryDequeue(out localValue))
                {
                    dl.RunDownloads(localValue.Item1,localValue.Item2,localValue.Item3,result,filepath).Wait();
                    Console.WriteLine(String.Format("Finished {0}",localValue.Item1));
                }
            };
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Parallel.Invoke(action, action, action, action);
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
      }

       
    }
}