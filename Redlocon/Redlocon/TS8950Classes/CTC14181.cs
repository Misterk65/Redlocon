using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    class CTC14181
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            //TEST              ERROR             SAMPLES           ERROR             ERROR             INTERIM
            //STEP              TYPE              MEASURED          RATE              LIMIT             RESULT   
            "TEST\nSTEP","ERROR\nTYPE", "SAMPLES\nMEASURED", "ERROR_A\nRATE", "ERROR_A\nLimit","INTERIM\nRESULT"
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

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        measValues = Regex.Replace(line, @"\s+", ";");

                        string[] helpArr = measValues.Split(';');
                        string replace = "";
                        string newVal = "";
                        string final = "";

                        foreach (var item in helpArr)
                        {
                            if (Regex.IsMatch(item, @"[A-Za-z]+"))
                            {
                                if (item.ToLower()!="inside")
                                {
                                    replace = replace + item + ";";
                                    newVal = newVal + item + " ";
                                }
                            }

                        }

                        measValues = measValues.Replace(replace, newVal + ";");
                        measValues = measValues.Replace("BCS;ACK/NACK", "BCS ACK/NACK");


                        measValues =  measValues.Substring(0, measValues.Length - 1);
                        BodyList.Add(measValues);
                        Application.DoEvents();
                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }
        public static void CreateReportTC14181(string filePath)
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
