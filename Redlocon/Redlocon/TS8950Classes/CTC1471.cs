﻿using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    class CTC1471
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            //TEST              ERROR             SAMPLES           ERROR             ERROR             INTERIM
            //STEP              TYPE              MEASURED          RATE              LIMIT             RESULT   
            "TEST\nSTEP","MEAS\nNUM", "BLOCK\nSIGNAL", "BL FREQ\nin MHz", "BL LEVEL\nin dBm","ERROR\nTYPE",
            "SAMPLES\nMEASURED", "ERROR\nRATE","ERROR\nLIMIT","INTERIM\nRESULT"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            var i = 0;
            string measValues = "";


            using (StreamReader reader = new StreamReader(filePath))
            { 
                string line = "";
                
                while (true)
                {
                    line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        line = Regex.Replace(line, "\\s+", ";");

                        if (line.Contains("Cl;II"))
                        {
                            measValues = line.Replace("Cl;II", "Cl II");//todo better solution needed 
                        }

                        if (measValues.Contains("Grpd;Lmt"))
                        {
                            measValues = measValues.Replace("Grpd;Lmt", "Grpd Lmt");//todo better solution needed 
                        }

                        measValues = measValues.Substring(0, measValues.Length - 1);
                        BodyList.Add(measValues);
                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }
        public static void CreateReportTC1471(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            if (File.Exists(Path.Combine(FrmMain.ReportOutputPath,Cproperties.DocxReportName + ".docx")))
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
