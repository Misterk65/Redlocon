using Redlocon.TS8950Classes;
using Redlocon.TS8980Classes;

namespace Redlocon
{
    class Cselection
    {
        public static void SelectTestCase(string tcNumber, string fullPath)
        {
            switch (tcNumber)
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
                case "TC6.05.00"://TS8950
                    CTC65.CreateReportTC65(fullPath);
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
    }
}
