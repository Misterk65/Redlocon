using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Redlocon.TS8980Classes
{
    class Ctc1421
    {

        /*
         * Specific Procedure for TC 13.3.4.1 on TS8980
         * 
        */

        private static readonly string[] TableHeader1 = new string[]
        {
            "Test Step", "Fer Speech\nin %", "Test limit\nin %", "Fer Speech\nMargin",
            "BER Class1b\nin %","Test limit\nin %", "BER Class1b\nMargin",
            "BER Class2\nin %","Test limit\nin %", "BER Class2\nMargin", "Interim\nResult"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(filePath, XmlReadMode.ReadSchema);
            DataTable tblteststep = dataSet.Tables["teststep"];
            DataRow[] resultsTestStep = tblteststep.Select();

            int i = 0;
            string helpVarTableBody = "";

            foreach (var row in resultsTestStep)
            {
                Regex regex = new Regex(@"[A-Za-z\-]+");
                Match match = regex.Match(row.ItemArray[4].ToString());
                
                // [2]; [4]
                if (match.Success)
                {
                    helpVarTableBody = row.ItemArray[2] + ";" +
                                       row.ItemArray[4] + ";"; 
                }
                else
                {
                    helpVarTableBody = row.ItemArray[2] + ";" +
                                       "None" + ";";
                }

                DataTable tblMeasurementStep = dataSet.Tables["measurementstep"];
                DataRow[] resultsMeasurementStep = tblMeasurementStep.Select();

                foreach (var resRow in resultsMeasurementStep)
                {
                    //[5];[8];[11];[17]

                    if (row.ItemArray[0].ToString() == resRow.ItemArray[1].ToString())
                    {
                        helpVarTableBody = helpVarTableBody + resRow.ItemArray[3] + ";" + resRow.ItemArray[6] + ";" +
                                           resRow.ItemArray[15] + ";" + resRow.ItemArray[16] + ";";
                    }
                }
                helpVarTableBody = helpVarTableBody.Substring(0, helpVarTableBody.Length - 1);

                /*
                 * [0][2][3][4][5][8][7][9][10][13][15][21][1]
                 * Value arrangement
                 */

                string[] helpArr = helpVarTableBody.Split(';');
                if (helpArr.Length < 13) continue;
                helpVarTableBody = helpArr[0] + ";" + helpArr[2] + ";" + helpArr[3] +";" + helpArr[4] + ";" 
                                   + helpArr[6] + ";" + helpArr[7] + ";" + helpArr[8] + ";" 
                                   + helpArr[10] + ";" + helpArr[11] + ";" + helpArr[12] + ";" + helpArr[1];

                BodyList.Add(helpVarTableBody);
            }

            Cproperties.TableBody = BodyList.ToArray();
            Cproperties.TableHeader = TableHeader1;
        }

        public static void CreateReportTc1421(string filePath)
        {
            Cts8980Common.GetFinalResults(filePath);
            //Cts8980Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }
}
