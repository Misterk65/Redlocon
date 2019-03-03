using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC544
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "TEST TIME\nSec", "UL POWER\ndBm", "LIMIT_A\ndBm", "INTERIM\nRESULT_A"

        };

        private static readonly string[] TableHeader2 = new string[]
        {
            "TEST\nSTEP", "TEST TIME\nSec", "UL POWER\ndBm", "LIMIT A\ndBm", "INTERIM\nRESULT_A", "LIMIT B\ndBm", "INTERIM\nRESULT_B"

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

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        line = Regex.Replace(line, "\\s+", ";");

                        //Adjust to long header
                        string[] helpArr = line.Split(';');

                        if (helpArr.Length == 6)
                        {
                            line = line.Replace("Inside","Inside;-;-");
                        }

                        measValues = line;
                        measValues = measValues.Substring(0, measValues.Length - 1);
                        BodyList.Add(measValues);
                    }
                }
            }

            Cproperties.TableHeader = TableHeader2;
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC544(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}
