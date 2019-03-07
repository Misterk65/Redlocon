using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Redlocon
{
    public partial class FrmMain : Form
    {
        public static string ReportOutputPath { get; set; }
        public static string ResultRootPath { get; set; }
        public static string TestcaseSourcePath { get; set; }
        
        public static string errString = "";
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {   
            //Props Form
            Text = "Redlocon";
            toolStripStatusLabelVer.Text = "Version: " + GetApplicationVersion();
            chkBoxDelZip.Text = "Delete Zip-Files";
            btnExtractResults.Text = "Extract Data";
            
            ReadSettingsXML();

            lblResRoot.Text = "Result Root Path: " + ResultRootPath;
            lblRptOut.Text = "Report Output Root Path: " + ReportOutputPath;
            Cproperties.TC65Counter = 0;

            //Properties in CSearchDirsRecursively

            CSearchDirsRecursively.DeleteZip = false;

        }

        private string GetApplicationVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return version;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about =new AboutBox1();
            about.ShowDialog();
        }

        private void chkBoxDelZip_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxDelZip.Checked)
            {
                CSearchDirsRecursively.DeleteZip = true;
            }
            else
            {
                CSearchDirsRecursively.DeleteZip = false;
            }
        }

        private void btnExtractResults_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = false; //New Folder can't be created here
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.Description = "Please select the root folder of the of the results to be imported.";
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblResRoot.Text = "Result Root Path: " + ResultRootPath;
                
                //
                //Select the destination folder
                //
                SelectDestinationFolder();
                //
                // The user selected a folder and pressed the OK button.
                // We print the number of files found.
                //
                ResultRootPath = folderBrowserDialog.SelectedPath;
                CSearchDirsRecursively searchDirsRecursively = new CSearchDirsRecursively();
               
                searchDirsRecursively.CheckAndUnpackZips(
                    searchDirsRecursively.FileList(ResultRootPath));
                
                foreach (var itemFileInfo in searchDirsRecursively.FileList(ResultRootPath))
                { 

                    if (!itemFileInfo.Name.StartsWith("."))
                    {
                        if (itemFileInfo.Name.EndsWith(".txt") || itemFileInfo.Name.EndsWith(".xml") ||
                            itemFileInfo.Name.EndsWith(".png") || itemFileInfo.Name.EndsWith(".zip") ||
                            itemFileInfo.Name.EndsWith(".csv"))
                        {
                            if (itemFileInfo.FullName.Contains("SummaryReport.xml"))
                            {
                                TestcaseSourcePath = itemFileInfo.DirectoryName;

                                FileStream fs = new FileStream(itemFileInfo.FullName, FileMode.Open, FileAccess.Read);
                                XmlDocument xmldoc = new XmlDocument();

                                xmldoc.Load(fs);

                                var node = xmldoc.SelectSingleNode("testcasereport/header/testcasenumber");
                                var attr = node.InnerText;
                                attr = Regex.Replace(attr, @"[a-zA-Z]+", "").Trim();

                                Cselection.SelectTestCase(attr,itemFileInfo.FullName);
                            }
                            else if (itemFileInfo.FullName.Contains("ContestDbDataSet.xml"))
                            {
                                FileStream fs = new FileStream(itemFileInfo.FullName, FileMode.Open, FileAccess.Read);
                                XmlDocument xmldoc = new XmlDocument();

                                xmldoc.Load(fs);

                                var node = xmldoc.SelectSingleNode("NewDataSet/testcase/TestcaseNumber");
                                var attr = node.InnerText;
                                attr = Regex.Replace(attr, @"[a-zA-Z]+", "").Trim();

                                Cselection.SelectTestCase(attr, itemFileInfo.FullName);
                            }
                            else if(itemFileInfo.FullName.EndsWith(".txt"))
                            {
                                using (StreamReader reader = new StreamReader(itemFileInfo.FullName))
                                {
                                    while (true)
                                    {
                                        string line = reader.ReadLine();

                                        if (line == null)
                                        {
                                            break;
                                        }

                                        if (line.StartsWith("Test Case"))
                                        {
                                            if (line.Contains("-"))
                                            {
                                                line = line.Replace("-", " ");
                                            }

                                            var helpArr = line.Split(':');
                                            helpArr = helpArr[1].TrimStart().Split(' ');
                                            if (helpArr[0].StartsWith("TC5."))
                                            {
                                                helpArr = helpArr[0].Split('.');
                                                helpArr[0] = helpArr[0] + "." +
                                                             helpArr[1] + "." +
                                                             helpArr[2];
                                            }
                                            reader.Close();

                                            CWordDocumentOutput.errPathOut = itemFileInfo.FullName;
                                            Cselection.SelectTestCase(helpArr[0], itemFileInfo.FullName);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            File.Delete(itemFileInfo.FullName);
                        }

                    }
                }

                if (errString !="")
                {
                    File.WriteAllText(Path.Combine(Application.StartupPath,"Err.txt"),errString);
                }
                MessageBox.Show("Finished", "Importing Finshed",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void SelectDestinationFolder()
        {
            folderBrowserDialogDest.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialogDest.Description = "Please select the folder where the results shall be stored.";
            DialogResult result = folderBrowserDialogDest.ShowDialog();
            if (result == DialogResult.OK)
            {
                ReportOutputPath = folderBrowserDialogDest.SelectedPath;
                lblRptOut.Text = "Report Output Root Path: " + ReportOutputPath;
            }
            else
            {
                return;
            }
        }

        private void closeApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo Save State
            Close();
            Dispose();
        }

        private void supportedTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmSupportForm = new SupportedTest();
            frmSupportForm.ShowDialog();
        }

        private void WriteSettingsXML()
        {
            if (File.Exists(Path.Combine(Application.StartupPath, "Settings.xml")))
            {
                //File.Delete(Path.Combine(Application.StartupPath, "Settings.xml"));

                using (XmlWriter writer = XmlWriter.Create(Path.Combine(Application.StartupPath, "Settings.xml")))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ProgramSettings");

                    if (ResultRootPath != null)
                    {
                        writer.WriteElementString("SourcePath", ResultRootPath);
                    }

                    if (ReportOutputPath != null)
                    {
                        writer.WriteElementString("DestinationPath", ReportOutputPath);
                    }


                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                } 
            }
        }

        private void ReadSettingsXML() //todo
        {
            if (File.Exists(Path.Combine(Application.StartupPath, "Settings.xml")))
            {
                try
                {
                    // Create an XML reader for this file.
                    using (XmlReader reader = XmlReader.Create(Path.Combine(Application.StartupPath, "Settings.xml")))
                    {
                        while (reader.Read())
                        {
                            // Only detect start elements.
                            if (reader.IsStartElement())
                            {
                                // Get element name and switch on it.
                                switch (reader.Name)
                                {
                                    case "SourcePath":
                                        ResultRootPath = reader.Value;
                                        lblResRoot.Text = "Result Root Path: " + ResultRootPath;
                                        break;
                                    case "DestinationPath":
                                        ReportOutputPath = reader.Value;
                                        lblRptOut.Text = "Report Output Root Path: " + ReportOutputPath;
                                        break;
                                }
                            }
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteSettingsXML();
        }

        public static void ResetVariables()
        {
            TestcaseSourcePath = "";
            Cproperties.ArrayOfGraphics = "";
            Cproperties.TestCaseName = "";
        }

    }
}
