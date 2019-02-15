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
                case "TC13.3.4.1":
                    CTC13341.CreateReportTC13341(fullPath);
                    break;
                default:
                    break;
            }
        }
    }
}
