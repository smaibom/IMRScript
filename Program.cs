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
            ConcurrentBag<(string,string,string,string)> result = new ConcurrentBag<(string,string, string, string)>();
            Download dl = new Download();
            string filepath = @"C:\Users\KOM\Documents\IMRScript\Downloads\";
            string fileurlpath = @"C:\Users\KOM\Documents\IMRScript\testdata.xlsx";
            
            ImportExcel openor = new ImportExcel();
            List<(string,string,string)> urls = openor.OpenExcel(fileurlpath);
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
                    //First download link
                    Task<(Boolean,string)> task = dl.DownloadFile(localValue.Item2,filepath,localValue.Item1);
                    (Boolean,string) res = task.Result;
                    

                    //If we succeed no reason to run the second link, add result and finish
                    if(res.Item1)
                    {
                        result.Add((localValue.Item1,"yes","",""));
                    }
                    else
                    {
                        //Second download link attempt
                        Task<(Boolean,string)> task2 = dl.DownloadFile(localValue.Item3,filepath,localValue.Item1);
                        (Boolean,string) res2 = task.Result;
                        if(res2.Item1)
                        {
                            result.Add((localValue.Item1,"yes",res.Item2,""));
                        }
                        else
                        {
                            result.Add((localValue.Item1,"no",res.Item2,res2.Item2));
                        }
                    }
                    Console.WriteLine(String.Format("Finished {0}",localValue.Item1));
                }
            };
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Parallel.Invoke(action, action, action, action);
            watch.Stop();
            //the path of the file
            string filePath = @"C:\Users\KOM\Documents\IMRScript\Downloads\results.xlsx";

            openor.WriteExcel(filePath,result);
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
      }
    }
}