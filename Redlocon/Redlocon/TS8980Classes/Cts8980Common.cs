using System.IO;
using System.Xml;

namespace Redlocon.TS8980Classes
{
    public class Cts8980Common
    {
        public static void GetFinalResults(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load(fs);

            var node = xmldoc.SelectSingleNode("testcasereport/header/finalverdict");
            var attr = node.InnerText;

            Cproperties.FinalResult = attr;
        }

        public static void GetTestPlanName(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load(fs);

            var nodeTestCaseNumber = xmldoc.SelectSingleNode("NewDataSet/testcase/TestcaseNumber");
            Cproperties.TestCaseName = nodeTestCaseNumber.InnerText;

            var nodeTestCaseName = xmldoc.SelectSingleNode("NewDataSet/testcase/TestcaseName");
            Cproperties.TestCaseName = Cproperties.TestCaseName + " " + nodeTestCaseName.InnerText;

            var nodeDocxReportName = xmldoc.SelectSingleNode("NewDataSet/testcaserun/ParameterFile");
            Cproperties.DocxReportName = nodeDocxReportName.InnerText;

            //extract test
            string[] helpArr = Cproperties.DocxReportName.Split(' ');
            helpArr = helpArr[0].Split('.');
            Cproperties.DocxReportName = helpArr[helpArr.Length - 1];

            var nodeDocxReportNameExt = xmldoc.SelectSingleNode("NewDataSet/testcaserun/StartTime");

            var helpString = nodeDocxReportNameExt.InnerText.Substring(0, nodeDocxReportNameExt.InnerText.Length-12);
            helpString = helpString.Replace("T", "");
            helpString = helpString.Replace(":", "");
            helpString = helpString.Replace("-", "");

            Cproperties.DocxReportName = Cproperties.DocxReportName + "-" + helpString;

        }

        /*public static void GetGraphicResults(string filePath)
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
        }*/
    }
}
