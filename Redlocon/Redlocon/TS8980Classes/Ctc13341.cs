using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Redlocon.TS8980Classes
{
    class Ctc13341
    {

        /*
         * Specific Procedure for TC 13.3.4.1 on TS8980
         * 
        */

        private static readonly string[] TableHeader1 = new string[]
        {
            "Test Step", "Measurement Timeslot", "Average Power Measured",
            "Lower Limit", "Upper Limit","Power Separation","From Test Step",
            "Lower Limit","Upper Limit","Timing Error","Absolute Limit",
            "Power Ramp Result", "Interim Result"
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
                        helpVarTableBody = helpVarTableBody + resRow.ItemArray[19] + ";" + resRow.ItemArray[5] + ";" +
                                           resRow.ItemArray[11] + ";" + resRow.ItemArray[8] + ";" + resRow.ItemArray[18] + ";";
                    }
                }
                helpVarTableBody = helpVarTableBody.Substring(0, helpVarTableBody.Length - 1);

                /*
                 * [0][2][3][4][5][8][7][9][10][13][15][21][1]
                 * Value arrangement
                 */

                string[] helpArr = helpVarTableBody.Split(';');
                if (helpArr.Length < 13) continue;
                helpVarTableBody = helpArr[0] + ";" + helpArr[2].Substring(helpArr[2].Length - 2) + ";" + helpArr[3] +
                                   ";" + helpArr[4] + ";" + helpArr[5] + ";" + helpArr[8] + ";" +
                                   helpArr[7].Substring(helpArr[7].Length - 2) + ";" + helpArr[9] + ";" + helpArr[10] +
                                   ";" + helpArr[13] + ";" + helpArr[15] + ";" + helpArr[21] + ";" + helpArr[1];

                BodyList.Add(helpVarTableBody);
            }

            Cproperties.TableBody = BodyList.ToArray();
            Cproperties.TableHeader = TableHeader1;
        }

        public static void CreateReportTc13341(string filePath)
        {
            Cts8980Common.GetFinalResults(filePath);
            //Cts8980Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }
}
