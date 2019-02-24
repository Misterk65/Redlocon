using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Redlocon.TS8980Classes;

namespace Redlocon.TS8950Classes
{
    public class CTC131
    { //TEST         NUM          MAX PK       PK PH ERR    MAX RMS      RMS PH ERR   MAX FR       FR ERR       AV FR        INTERIM
      //STEP         BURSTS       PH ERR deg   LIMIT deg    PH ERR deg   LIMIT deg    ERR ppm      LIMIT ppm    ERR ppm      RESULT
        private static readonly string[] TableHeader1 = new string[]
        {
            "TEST\nSTEP", "NUM\nBURSTS", "MAX PK\nPH ERR deg","PK PH ERR\nLIMIT deg","MAX RMS\nPH ERR deg", "RMS PH ERR\nLIMIT deg",
            "MAX FR\nERR ppm","FR ERR\nLIMIT ppm","AV FR\nERR ppm","INTERIM\nRESULT"
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

                    /*if (line.Contains("Test Step Parameters") && line.Contains("Not applicable") == false)
                    {
                        stepStrings = line.Split(':');
                        step = stepStrings[1].Trim();
                        stepStrings = step.Split(' ');
                        step = stepStrings[1];
                    }*/

                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        line = Regex.Replace(line, "\\s+", ";");

                        measValues = line;
                        measValues = measValues.Substring(0, measValues.Length - 1);
                        BodyList.Add(measValues);
                    }

                    /*if (Regex.IsMatch(line, @"^\d+"))
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
                        }*/
           
                }
            }

            Cproperties.TableHeader = TableHeader1;
            Cproperties.TableBody = BodyList.ToArray();
        }

        public static void CreateReportTC131(string filePath)
        {
            Cts8950Common.GetTestReportParameter(filePath);
            CreateTableContent(filePath);
            //Cts8950Common.GetGraphicResults(filePath);
            CWordDocumentOutput.MakeDoc(FrmMain.ReportOutputPath);
        }
    }

    
}
