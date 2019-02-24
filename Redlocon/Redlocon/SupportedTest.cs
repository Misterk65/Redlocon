using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Redlocon
{
    public partial class SupportedTest : Form
    {
        public SupportedTest()
        {
            InitializeComponent();
        }

        private void SupportedTest_Load(object sender, EventArgs e)
        {
            btnClose.Text = "&Close";
            Text = "Supported Tests";
            int i = 0;

            string docPath = Path.Combine(Application.StartupPath, "Supported.txt");

            List<string[]> rows = File.ReadAllLines(docPath).Select(x => x.Split(';')).ToList();
            DataTable dt = new DataTable();

            dt.Columns.Add("Test Case");
            dt.Columns.Add("TS 8950");
            dt.Columns.Add("TS 8980");
            dt.Columns.Add("T-Spec.");

            rows.ForEach(x => {
                if (i>0)
                {
                    dt.Rows.Add(x);
                }

                i++;

            });
            dataGridViewSupportedTests.DataSource = dt;
            dataGridViewSupportedTests.ScrollBars = ScrollBars.Vertical;
            dataGridViewSupportedTests.Sort(dataGridViewSupportedTests.Columns["T-Spec."], ListSortDirection.Descending);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
