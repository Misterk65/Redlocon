using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    class CTC67
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            //Samples          Errors           NER1           EARLYPASS         NER0         EARLYFAIL
            //(#)             (#)            ratio             LIMIT          ratio           LIMIT 
            
            "TEST\nSTEP","Samples\n(#)", "Errors\n(#)", "NER1\nratio", "EARLYPASS\nLIMIT","NER0_A\nratio",
            "EARLYFAIL\nLIMIT","INTERIM\nRESULT"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            var i = 0;
            string measValues = "";


            using (StreamReader reader = new StreamReader(filePath))
            { 
                string step = "";
                string line = "";
                string earlyPass = "";

                while (true)
                {
                    line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (line.Contains("Test Step Parameters") &&
                        line.Contains("Not applicable for UEs Power Class") == false)
                    {
                        //add test step
                        string[] stepStrings = line.Split(':');
                        step = stepStrings[1].Trim();
                        stepStrings = step.Split(' ');
                        step = stepStrings[1];
                    }

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        measValues = Regex.Replace(line, "\\s+", ";");
                        measValues = step + ";" + measValues.Substring(0, measValues.Length - 1);
                    }

                    if (line.Contains("EARLY PASS detected"))
                    {

                        earlyPass = "(EARLY PASS) ";

                    }

                    if (line.Contains("Test Step Result"))
                    {
                        //add interims results
                        string[] resultStrings = line.Split(':');
                        var res = resultStrings[1].Trim();
                        if (earlyPass != String.Empty)
                        {
                            measValues = measValues + ";" + earlyPass + res;
                        }
                        else
                        {
                            measValues = measValues + ";" + res;
                        }
                        BodyList.Add(measValues);
                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }
        public static void CreateReportTC67(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            if (File.Exists(Path.Combine(FrmMain.ReportOutputPath,Cproperties.DocxReportName + ".docx")))
            {
                Cproperties.TC65Counter++;
                Cproperties.DocxReportName = Cproperties.DocxReportName + "_" + Cproperties.TC65Counter;
                CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath); 
                
            }
            else
            {
                CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
            }
        }
    }
}
