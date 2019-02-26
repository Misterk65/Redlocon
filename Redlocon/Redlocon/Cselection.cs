using System.Text.RegularExpressions;
using System.Windows.Forms;
using Redlocon.TS8950Classes;
using Redlocon.TS8980Classes;

namespace Redlocon
{
    class Cselection
    {
        public static void SelectTestCase(string tcNumber, string fullPath)
        {

            switch (CheckForProcedure(tcNumber))
            {
                case "TC5.02.00"://TS8950
                    CTC52.CreateReportTC52(fullPath);
                    break;
                case "TC5.04.03"://TS8950
                    CTC543.CreateReportTC543(fullPath);
                    break;
                case "TC5.09.00"://TS8950
                    CTC59.CreateReportTC59(fullPath);
                    break;
                case "TC5.10.00"://TS8950
                    CTC510.CreateReportTC510(fullPath);
                    break;
                case "TC5.11.00"://TS8950
                    CTC511.CreateReportTC511(fullPath);
                    break;
                case "TC6.02.00"://TS8950
                    CTC62.CreateReportTC62(fullPath);
                    break;
                case "TC6.04.00"://TS8950
                    CTC64.CreateReportTC64(fullPath);
                    break;
                case "TC6.05.00.P1"://TS8950
                    CTC65.CreateReportTC65(fullPath);
                    break;
                case "TC6.07.00"://TS8950
                    CTC67.CreateReportTC67(fullPath);
                    break;
                case "TC6.08.00"://TS8950
                    CTC68.CreateReportTC68(fullPath);
                    break;
                case "13.3.4.1":
                    if (fullPath.Contains("SummaryReport.xml"))
                    {
                        Ctc13341.CreateReportTc13341(fullPath);
                    }
                    if (fullPath.Contains("ContestDbDataSet.xml"))
                    {
                        Ctc13341.CreateTableContent(fullPath);
                        Cts8980Common.GetTestPlanName(fullPath);

                    }
                    break;
                case "TC13.3.4.1"://TS8950
                    CTC13341.CreateReportTC13341(fullPath);
                    break;
                case "TC13.1"://TS8950
                    CTC131.CreateReportTC131(fullPath);
                    break;
                case "TC13.2"://TS8950
                    CTC132.CreateReportTC132(fullPath);
                    break;
                case "TC13.4"://TS8950
                    CTC134.CreateReportTC134(fullPath);
                    break;
                case "TC14.2.1"://TS8950
                    CTC1421.CreateReportTC1421(fullPath);
                    break;
                case "TC14.6.1"://TS8950
                    CTC1461.CreateReportTC1461(fullPath);
                    break;
                case "TC14.8.1"://TS8950
                    CTC1481.CreateReportTC1481(fullPath);
                    break;
                case "TC14.8.3"://TS8950
                    CTC1481.CreateReportTC1481(fullPath);
                    break;
                case "TC14.18.1"://TS8950
                    CTC14181.CreateReportTC14181(fullPath);
                    break;
                case "TC14.18.3"://TS8950
                    CTC14183.CreateReportTC14183(fullPath);
                    break;
                case "TC14.18.4"://TS8950
                    CTC14184.CreateReportTC14184(fullPath);
                    break;
                case "4.2.3.1":
                    if (fullPath.Contains("SummaryReport.xml"))
                    {
                       Ctc4231fdd.CreateReportTc4231fdd(fullPath);
                    }
                    if (fullPath.Contains("ContestDbDataSet.xml"))
                    {
                        Ctc4231fdd.CreateTableContent(fullPath);
                        Cts8980Common.GetTestPlanName(fullPath);

                    }
                    break;
                case "13.17.3.4.1":
                    if (fullPath.Contains("SummaryReport.xml"))
                    {
                        Ctc1317341.CreateReportTc1317341(fullPath);
                    }
                    if (fullPath.Contains("ContestDbDataSet.xml"))
                    {
                        Ctc1317341.CreateTableContent(fullPath);
                        Cts8980Common.GetTestPlanName(fullPath);

                    }
                    break;
                default:
                    break;
            }
        }

        private static string CheckForProcedure(string tc)
        {
            string output = "";
            string[] helpArr = tc.Split('.');

            if (Regex.IsMatch(helpArr[helpArr.Length-1], @"\[A-Z]"))
            {
                output = tc.Replace("." + helpArr[helpArr.Length - 1], "");
            }
            else
            {
                output = tc;
            }

            return output;
        }
    }
}
