using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    class CTC65
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            //TEST            BL_FREQ    SAMPLES       ERRORS         BER %        BER %       EXC.       INTERIM
            //STEP            BL_FREQ       MEASURED     MEASURED         MEAS         LIMIT     COUNTER      RESULT
            "TEST\nSTEP","BL_FREQ\nBL_FREQ", "SAMPLES\nMEASURED", "ERRORS\nMEASURED", "BER %\nMEAS","BER %\nLIMIT",
            "EXC.\nCOUNTER","INTERIM\nRESULT"
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
                        measValues = Regex.Replace(line, "\\s+", ";");
                        measValues =  measValues.Substring(0, measValues.Length - 1);
                        BodyList.Add(measValues);
                        Application.DoEvents();
                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }
        public static void CreateReportTC65(string filePath)
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
