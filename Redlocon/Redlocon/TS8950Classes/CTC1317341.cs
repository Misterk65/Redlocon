using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC1317341
    {
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "MEAS\nStep", "Time\nSlot", "NUM\nBURST", "AVG POWER\nin dBm","HI LIM\nin dBm",
            "LO LIM\nin dBm","PWR SEP\nin dB", "FROM\nSTEP","LO LIM\nin dB","HI LIM\nin dB","AVG POWER\nRESULT",
            "PWR SEP\nRESULT","RAMP\nRESULT"
        };

        public static void CreateTableContent(string filePath)
        {
            List<string> BodyList = new List<string>();
            var i = 0;
            string measValues = "";
            var measStep = "";
            var timeSlot = "";

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (line.Contains("Measurement Step Parameters : STEP"))
                    {
                       string[] helpArr = line.Split(':');
                       helpArr = helpArr[1].Trim().Split(' ');
                       measStep = helpArr[1].Trim();
                    }

                    if (line.Contains("Measurement Timeslot :"))
                    {
                        string[] helpArr = line.Split(':');
                        timeSlot = helpArr[1].Trim();

                    }

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        if (Regex.IsMatch(line, @"^\d+"))
                        {
                            line = Regex.Replace(line, "\\s+", ";");
                            
                            measValues = line.Replace("Not;Meas", "Not Meas");
                            measValues = measValues.Substring(0, measValues.Length - 1);
                            
                        }

                        string[] helpArrIn = measValues.Split(';');
                        measValues = helpArrIn[0] + ";" + measStep + ";" +
                                     timeSlot + ";" + helpArrIn[1] + ";" + 
                                     helpArrIn[2] + ";" +
                                     helpArrIn[3] + ";" +
                                     helpArrIn[4] + ";" +
                                     helpArrIn[5] + ";" +
                                     helpArrIn[6] + ";" +
                                     helpArrIn[7] + ";" +
                                     helpArrIn[8] + ";" +
                                     helpArrIn[9] + ";" +
                                     helpArrIn[10] + ";" +
                                     helpArrIn[11];
                        BodyList.Add(measValues);
                    }
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC1317341(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}
