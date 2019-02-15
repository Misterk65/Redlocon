using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC13341
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "BURST\nNUM", "AVG PWR\nin dBm","HI LIM\nin dBm", "LO LIM\nin dBm",
            "PWR SEP\nin dB","FROM\nSTEP","LO LIM\nin dB","HI LIM\nin dB","AV PWR\nRESULT",
            "PWR SEP\nRESULT","RAMP\nRESULT", "TIM ERR\nin µs","ABS LIM\nin µs","TIMING\nRESULT"
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
                        line = Regex.Replace(line, "\\s+", ";");
                        if (i <=1)
                        {
                            if (measValues==String.Empty)
                            {
                                measValues = line;
                                i++;
                            }
                            else
                            {
                                measValues = measValues + line.Substring(2);
                                measValues = measValues.Substring(0, measValues.Length - 1);
                                measValues = measValues.Replace("Not;Meas", "Not Meas");//todo better way to be found
                                measValues = measValues.Replace(";;", ";");//todo better way to be found
                                i = 0;
                                BodyList.Add(measValues);
                                measValues = "";
                            }
                        }

                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC13341(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}
