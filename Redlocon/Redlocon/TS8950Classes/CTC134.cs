using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC134
    { //TEST          MEAS          OFFSET FREQ   MEAS LEVEL    LIMIT         EXC LIMIT     INTERIM
      //STEP          NUMBER        in MHz        in dBm        in dBm        in dBm        RESULT
        private static readonly string[] TableHeader1 = new string[]
        {
            "MEAS\nSTEP","TEST\nSTEP", "MEAS\nNUMBER", "OFFSET FREQ\nin MHz","MEAS LEVEL\nin dBm","LIMIT\nin dBm",
            "EXC LIMIT\nin dBm","INTERIM\nRESULT"
        };

        private static readonly string[] TableHeader2 = new string[]
        {
            "MEAS\nSTEP","TEST\nSTEP", "MEAS\nNUMBER", "OFFSET FREQ\nin MHz","MEAS LEVEL\nin dBm","MEAS LEVEL\nin dBc","LIMIT\nin dBc",
            "EXC LIMIT\nin dBc","INTERIM\nRESULT"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            var testCaseProf = "";
            var i = 0;
            string measValues = "";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string[] stepStrings;
                string step = "";

                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (line.Contains("Measurement Step Parameters") && line.Contains("SKIPPED BECAUSE TEST STEP IS " +
                                                                                      "DISABLED IN PARAMETER FILE") == false)
                    {
                        stepStrings = line.Split(':');
                        step = stepStrings[1].Trim();
                        stepStrings = step.Split(' ');
                        step = stepStrings[1];
                    }

                    if (line.Contains("Test Case              : TC13.4 MODULATION"))
                    {
                        testCaseProf = "MODULATION";
                    }

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        line = Regex.Replace(line, "\\s+", ";");

                        measValues = step + ";" + line;
                        measValues = measValues.Substring(0, measValues.Length - 1);
                        BodyList.Add(measValues);
                    }
                }
            }

            if (testCaseProf == "MODULATION")
            {
                Cproperties.TableHeader = TableHeader2;
            }
            else
            {
                Cproperties.TableHeader = TableHeader1;
            }
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC134(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}
