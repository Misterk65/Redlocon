using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Redlocon.TS8980Classes
{
    class Ctc4231fdd
    {
        /*
         * Specific Procedure for TC 4.2.3.1 FDD on TS8980
         * 
        */
        
        private static readonly string[] TableHeader1 = new string[]
        {
            "Measured", "Nominal", "Lower Limit ","Upper Limit", "Deviation", "Deviation"
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

                // [2]; [4]
                helpVarTableBody = row.ItemArray[2] + ";" +
                                   row.ItemArray[4] + ";";

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
                 * [3][8][4][5]([3]-[8])[1]
                 * Value arrangement
                 */
                string[] helpArr = helpVarTableBody.Split(';');

                double diff3and8 = Convert.ToDouble(Regex.Replace(helpArr[3].Trim(), @"[a-zA-Z]+","")) 
                                - Convert.ToDouble(Regex.Replace(helpArr[8].Trim(), @"[a-zA-Z]+", ""));

                helpVarTableBody = helpArr[3] + ";" +
                                   helpArr[8] + ";" +
                                   helpArr[4] + ";" +
                                   helpArr[5] + ";" +
                                   diff3and8.ToString("F1") + " dBm;" +
                                   helpArr[1];
                                   
                BodyList.Add(helpVarTableBody);
            }

            Cproperties.TableBody = BodyList.ToArray();
            Cproperties.TableHeader = TableHeader1;
        }

        public static void CreateReportTc4231fdd(string filePath)
        {
            Cts8980Common.GetFinalResults(filePath);
            //Cts8980Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }
}
