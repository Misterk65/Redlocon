﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC13174
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "MEAS\nNUMBER", "OFFSET FREQ\nin MHz","MEAS LEVEL\nin dBm", "MEAS LEVEL\nin dBc","LIMIT\nin dBc",
            "EXC LIMIT\nindBc","INTERIM\nRESULT"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            var i = 0;
            string measValues = "";

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
                            BodyList.Add(measValues);
                        }
                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC13174(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}