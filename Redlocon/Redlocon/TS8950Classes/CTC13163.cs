using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC13163
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "MEAS\nNUMBER", "OFFSET FREQ\nin MHz","MEAS LEVEL\nin dBm", "LIMIT\nin dBm",
            "EXC LIMIT\nin dBm","INTERIM\nRESULT"
        };

        private static readonly string[] TableHeader2 = new string[]
        {
            "TEST\nSTEP","MEAS\nNUMBER", "OFFSET FREQ\nin MHz", "MEAS LEVEL\nin dBm","MEAS LEVEL\nin dBc",
            "LIMIT\nin dBc", "EXC LIMIT\nin dBc","INTERIM\nRESULT"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            var i = 0;
            string measValues = "";
            int headerSelector = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        if (Regex.IsMatch(line, @"^\d+"))
                        {
                            line = Regex.Replace(line, "\\s+", ";");
                            
                            measValues = line;
                            measValues = measValues.Substring(0, measValues.Length - 1);

                            string[] helpArr = measValues.Split(';');
                            headerSelector = helpArr.Length;

                            BodyList.Add(measValues);
                        }
                    }
                }
            }

            if (headerSelector == 8)
            {
                Cproperties.TableHeader = TableHeader2;
            }
            else
            {
                Cproperties.TableHeader = TableHeader1;
            }
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC13163(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            if (File.Exists(Path.Combine(FrmMain.ReportOutputPath, Cproperties.DocxReportName + ".docx")))
            {
                Cproperties.TC65Counter++;
                Cproperties.DocxReportName = Cproperties.DocxReportName + "_UID_" + Cproperties.TC65Counter;
                CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);

            }
            else
            {
                CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
            }
        }
    }

    
}
