using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Redlocon
{
    public class CSearchDirsRecursively
    {
        public FileInfo[] Files { get; private set; }
        public static bool DeleteZip { get; set; }

        public string[] helpArr { get; set; }

        public FileInfo[] FileList(string path)
        {
            string searchPattern = "*";

            DirectoryInfo di = new DirectoryInfo(path);
            di.GetDirectories(searchPattern, SearchOption.AllDirectories);

            Files = di.GetFiles(searchPattern, SearchOption.AllDirectories);
          
            return Files;
        }

        public void CheckAndUnpackZips(FileInfo[] compressedfiles)
        {
            string extractRootPath = "";
            string compressedfilePath = "";
            string checkFile = "";

            foreach (var compressedfile in compressedfiles)
            {
                helpArr = compressedfile.DirectoryName.Split('\\');
                compressedfilePath = compressedfile.DirectoryName;
                checkFile = compressedfile.Extension.ToLower();

                

               if (compressedfile.Name.EndsWith(".zip"))
                {
                    string zipPath = compressedfile.FullName;
                    string extractPath = extractRootPath; //Path.Combine(extractRootPath, compressedfile.Name);

                    if (helpArr.Length == 2)
                    {
                        extractRootPath = Path.Combine(compressedfilePath, "ZipTemp");
                        FrmMain.ResultRootPath = Path.Combine(compressedfilePath, "ZipTemp");
                    }
                    else
                    {
                        extractRootPath = helpArr[0] +
                                          "\\" + helpArr[1] +
                                          "\\ZipTemp" +
                                          "\\" + helpArr[helpArr.Length - 2] +
                                          "\\" + helpArr[helpArr.Length - 1];

                        FrmMain.ResultRootPath = helpArr[0] +
                                                 "\\" + helpArr[1] +
                                                 "\\ZipTemp";
                    }

                    if (!File.Exists(extractRootPath))
                    {
                        try
                        {
                            ZipFile.ExtractToDirectory(zipPath, extractRootPath);
                        }
                        catch (System.Exception ex)
                        {

                            MessageBox.Show("Unzip Error", "The following Error occured\n\n" + ex.Message);
                        } 
                    }

                    if (checkFile == ".zip")
                    {
                        if (helpArr.Length == 2)
                        {
                            FrmMain.ResultRootPath = Path.Combine(compressedfilePath, "ZipTemp");
                        }
                        else
                        {
                            FrmMain.ResultRootPath = helpArr[0] +
                                                     "\\" + helpArr[1] +
                                                     "\\ZipTemp";
                        }
                    }

                    if (DeleteZip)
                    {
                        File.Delete(zipPath);
                    }
                   
                }
            }
            
        }
    }
}
