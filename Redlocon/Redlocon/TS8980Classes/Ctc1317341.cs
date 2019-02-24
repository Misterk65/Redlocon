using System.Collections.Generic;
using System.Data;

namespace Redlocon.TS8980Classes
{
    class Ctc1317341
    {
        /*
        * Specific Procedure for TC 13.17.3.4.1 on TS8980
        * 
       */

        private static readonly string[] TableHeader1 = new string[]
        {
            "Test Step", "Measurement Timeslot", "Average Power Measured",
            "Lower Limit", "Upper Limit","Power Separation","From Test Step",
            "Lower Limit","Upper Limit","Power Ramp Result", "Interim Result"
        };
        //Test Step

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(filePath, XmlReadMode.ReadSchema);
            DataTable tblteststep = dataSet.Tables["teststep"];
            DataRow[] resultsTestStep = tblteststep.Select();

            int i = 0;
            string[] dataArray = new string[CreateResultDataContent(filePath).Length];
            string helpVarTableBody = "";

            foreach (var row in resultsTestStep)
            {

                // [2]; [4]
                helpVarTableBody = row.ItemArray[0] + ";" +
                                   row.ItemArray[2] + ";" +
                                   row.ItemArray[4] + ";";

                foreach (var resRow in CreateResultDataContent(filePath))
                {
                    string[] arrStrings = resRow.Split(';');

                    if (row.ItemArray[0].ToString()==arrStrings[1])
                    {
                        dataArray[i] = helpVarTableBody + resRow;
                        
                        i++;
                    }
                    
                    
                }
        
                
            }
            foreach (var item in dataArray)
            {
                /*
                         * [0][2][3][4][5][8][7][9][10][13][15][21][1]
                         * Value arrangement
                         */
                string[] helpArr = item.Split(';');//todo
                helpVarTableBody = helpArr[1] + ";" +
                                   helpArr[3] + ";" +
                                   helpArr[5] + ";" +
                                   helpArr[7] + ";" +
                                   helpArr[6] + ";" +
                                   helpArr[9]+ ";" +
                                   helpArr[10] + ";" +
                                   helpArr[11] + ";" +
                                   helpArr[12] + ";" +
                                   helpArr[13] + ";" +
                                   helpArr[2];

                BodyList.Add(helpVarTableBody);
            }

            Cproperties.TableBody = BodyList.ToArray();
            Cproperties.TableHeader = TableHeader1;
        }

        private static string[] CreateResultDataContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(filePath, XmlReadMode.ReadSchema);
            DataTable tblMeasurementStep = dataSet.Tables["measurementstep"];
            DataRow[] resultsMeasurementStep = tblMeasurementStep.Select();

            int rowCount = 0;
            var helpVarTableBody = "";

            foreach (var resRow in resultsMeasurementStep)
            {

                if (rowCount == 0)
                {
                    //{[1]163998;[5]23.12 dBm;[8]25.98 dBm;[11]17.98 dBm;[17]2.86 dB}
                    helpVarTableBody = resRow.ItemArray[19].ToString().Substring(resRow.ItemArray[19].ToString().Length -1) + ";" + 
                                       resRow.ItemArray[1] + ";" + resRow.ItemArray[5] + ";"+ resRow.ItemArray[8] + ";" +
                                       resRow.ItemArray[11] + ";" + resRow.ItemArray[17] + ";";
                    rowCount++;
                }
                else if (rowCount == 1)
                {
                    if (resRow.ItemArray.Length==13)
                    {
                        helpVarTableBody = helpVarTableBody + resRow.ItemArray[5] + ";" + 
                                           resRow.ItemArray[12].ToString().Substring(resRow.ItemArray[12].ToString().Length - 1) + ";" +
                                           resRow.ItemArray[11] + ";" + resRow.ItemArray[8] + ";";
                        rowCount++;
                    }
                    else
                    {
                        helpVarTableBody = helpVarTableBody + resRow.ItemArray[5] + ";" +
                                           resRow.ItemArray[19].ToString().Substring(resRow.ItemArray[19].ToString().Length - 1) + ";" +
                                           resRow.ItemArray[11] + ";" + resRow.ItemArray[8] + ";";
                        rowCount++;
                    }
                    
                }
                else if (rowCount == 2)
                {
                    helpVarTableBody = helpVarTableBody + resRow.ItemArray[resRow.ItemArray.Length-2];
                    BodyList.Add(helpVarTableBody);
                    helpVarTableBody = "";
                    rowCount = 0;
                }

            }


            return(BodyList.ToArray());
        }

        public static void CreateReportTc1317341(string filePath)
        {
            Cts8980Common.GetFinalResults(filePath);
            //Cts8980Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }
}

