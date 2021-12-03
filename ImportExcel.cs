using System.Data;

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

