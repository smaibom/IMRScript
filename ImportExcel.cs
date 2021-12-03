using System.Data;
using OfficeOpenXml;
using System.Collections.Concurrent;

namespace IMRScript
{
    public class ImportExcel
    {
        private DataTable ds;

        public ImportExcel()
        {
            ds = new DataTable();
            
            
            DataColumn col1 = new DataColumn();
            col1.DataType = System.Type.GetType("System.String");
            col1.ColumnName = "id";
            ds.Columns.Add(col1);
        }


        public void getExcelFile(string filepath)
        {
            ;
        }

        public void WriteExcel(string filepath,ConcurrentBag<(string,string,string,string)> data)
        {
            using (ExcelPackage excelFile = new ExcelPackage())
            {
                //create a new Worksheet
                ExcelWorksheet worksheet = excelFile.Workbook.Worksheets.Add("Sheet 1");

                //add data to excel sheet:
                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Result";
                worksheet.Cells["C1"].Value = "error msg 1";
                worksheet.Cells["D1"].Value = "error msg 2";
                int i = 2;
                foreach((string,string,string,string) res in data)
                {
                    worksheet.Cells["A"+i.ToString()].Value = res.Item1;
                    worksheet.Cells["B"+i.ToString()].Value = res.Item2;
                    worksheet.Cells["C"+i.ToString()].Value = res.Item3;
                    worksheet.Cells["D"+i.ToString()].Value = res.Item4;
                    i++;
                }


                //Write the file to the disk
                FileInfo fi = new FileInfo(filepath);
                excelFile.SaveAs(fi);
            }
        }

        public List<(string,string,string)> OpenExcel(string filepath)
        {
            List<(string,string,string)> data = new List<(string, string, string)>();
            try
            {
                using(var excel = new ExcelPackage(filepath))
                {
                    List<string> lst = new List<string>();
                    // Get the sheet from Excel in the file
                    var sheet = excel.Workbook.Worksheets.First();

                    //Get Customer Data
                    List<string> ids = (from cell in sheet.Cells["A2:A21058"] select cell.GetValue<string>()).ToList();
                    //Get Customer Data
                    List<string> url1 = (from cell in sheet.Cells["AL2:AL21058"] select cell.GetValue<string>()).ToList();

                    //Get Customer Data
                    List<string> url2 = (from cell in sheet.Cells["AM2:AM21058"] select cell.GetValue<string>()).ToList();
                    for(int i = 0; i < ids.Count; i++)
                    {
                        data.Add((ids[i],url1[i],url2[i]));
                    }
                }
            }    
            catch (Exception)
            {

            }
            return data;
        }




        public List<(string,string,string)> getLinks(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            List<(string,string,string)> urls = new List<(string,string,string)>();
            try
            {
                foreach(string line in lines)
                {
                    string[] splitLine = line.Split(',');
                    urls.Add((splitLine[0],splitLine[1],splitLine[2]));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed");
                return urls;
            }

            return urls;
        }
    }
}

