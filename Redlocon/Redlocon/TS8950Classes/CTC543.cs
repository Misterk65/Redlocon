using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC543
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "MEAS\nSTEP", "Level\n(dBm)", "LIMIT HI\n(dBm)", "INTERIM\nRESULT"

        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
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

                    if (line.Contains("Test Step Parameters") && line.Contains("Not applicable")==false)
                    {
                        stepStrings = line.Split(':');
                        step = stepStrings[1].Trim();
                        stepStrings = step.Split(' ');
                        step = stepStrings[1];
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

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC543(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}
