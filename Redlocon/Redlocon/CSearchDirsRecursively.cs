using System.IO;
using System.IO.Compression;

namespace Redlocon
{
    public class CSearchDirsRecursively
    {
        public FileInfo[] Files { get; private set; }
        public static bool DeleteZip { get; set; }

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
            foreach (var compressedfile in compressedfiles)
            {
                if (compressedfile.Name.EndsWith(".zip"))
                {
                    string zipPath = compressedfile.FullName;
                    string extractPath = compressedfile.FullName.Replace(compressedfile.Name, "");

                    if (File.Exists(extractPath))
                    {
                        ZipFile.ExtractToDirectory(zipPath, extractPath); 
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
