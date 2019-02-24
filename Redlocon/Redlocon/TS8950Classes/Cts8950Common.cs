using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Redlocon.TS8980Classes
{
    public class Cts8950Common
    {
        public static void GetTestReportParameter(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }
                    
                    if (line.StartsWith("TC Parameter File"))
                    {
                        //DocxReportName
                        var helpArr = line.Split('\\');
                        Cproperties.DocxReportName = helpArr[helpArr.Length - 1];
                        Cproperties.DocxReportName = Cproperties.DocxReportName.Substring(0, 
                            Cproperties.DocxReportName.Length - 4);

                    }
                    else if(line.Contains("Test Case"))
                    {
                        //Test Case Name
                        var helpArr = Regex.Replace(line, @"\s+", " ").Split(':');
                        //var helpArr = line.Split(':');
                        if (Cproperties.TestCaseName==null)
                        {
                            Cproperties.TestCaseName = helpArr[1]; 
                        }

                    }
                    else if (line.StartsWith("Final Verdict"))
                    {
                        //GetFinalResult
                        var helpArr = Regex.Replace(line, @"\s+", "").Split(':');
                        Cproperties.FinalResult = helpArr[1];
                    }
                }
            }
        }

        public static void GetGraphicResults(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XmlDocument xmldoc = new XmlDocument();
            
            xmldoc.Load(fs);


            string toArray = "";
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line==null)
                    {
                        break;
                    }

                    if (line.Contains("chart"))
                    {
                        string[] x = line.Split('=');
                        string[] y = x[3].Split('>');
                        
                        var attr = y[0].Replace("%5b", "[");
                        attr = attr.Replace("%5d", "]");
                        attr = attr.Replace("%26", "&");

                        Cproperties.ArrayOfGraphics = Cproperties.ArrayOfGraphics + ";" + attr;
                    }
                }
            
                if (Cproperties.ArrayOfGraphics.StartsWith(";"))
                {
                    Cproperties.ArrayOfGraphics = Cproperties.ArrayOfGraphics.Substring(1);
                }

                if (Cproperties.ArrayOfGraphics.Contains("\""))
                {
                    Cproperties.ArrayOfGraphics = Cproperties.ArrayOfGraphics.Replace("\"","");
                }
                reader.Close();
            }
        }
    }
}
