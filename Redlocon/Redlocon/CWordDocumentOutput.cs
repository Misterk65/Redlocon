using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Xceed.Words.NET;
using Font = Xceed.Words.NET.Font;
using Orientation = Xceed.Words.NET.Orientation;

namespace Redlocon
{
    //Information
    //https://www.c-sharpcorner.com/article/generate-word-document-using-c-sharp/

    public class CWordDocumentOutput
    {
        public static string errPathOut;

        public static void MakeDoc(string filePath)
        {
            string fileName = Path.Combine(filePath, Cproperties.DocxReportName + ".docx") ;


            var doc = DocX.Create(fileName);
            doc.PageLayout.Orientation = Orientation.Landscape;

            //Create Table
            var getColumns = Cproperties.TableHeader.Length;
            var getRows = Cproperties.TableBody.Length + 1;

            var counter = 0;
            

            Table t = doc.AddTable(getRows, getColumns);
            t.Alignment = Alignment.center;
            t.Design = TableDesign.TableGrid;
            
            //Fill cells by adding text.
            //Header
            foreach (var item in Cproperties.TableHeader)
            {
                t.Rows[0].Cells[counter].Paragraphs.First().Append(item).Font("Verdana").FontSize(9D).Bold();
                counter++;
            }

            try
            {
                //Content
                counter = 1;
                var zindex = 0;
                foreach (var item in Cproperties.TableBody)
                {
                    if (item != null)
                    {
                        string[] helpArr = item.Split(';');

                        foreach (var set in helpArr)
                        {
                            t.Rows[counter].Cells[zindex].Paragraphs.First().Append(set);
                            zindex++;
                        }

                        zindex = 0;
                        counter++;

                    }
                }
            }
            catch (System.Exception)
            {

                FrmMain.errString = FrmMain.errString + errPathOut + "@" + Cproperties.DocxReportName + Environment.NewLine;
            }

            Paragraph TabPar1 = doc.InsertParagraph();
            TabPar1.Append("Test Case: " + Cproperties.TestCaseName).Font((Font) new Font("Verdana"))
                .FontSize(12D)
                .Bold()
                .SpacingAfter(20);

            Paragraph TabPar = doc.InsertParagraph();
            TabPar.InsertTableBeforeSelf(t);
            TabPar.SpacingAfter(20);
            TabPar.KeepLinesTogether();

            if (Cproperties.FinalResult =="Passed")
            {
                TabPar.Append("\nFinal Result: " + Cproperties.FinalResult)
                        .Font(new Font("Verdana"))
                        .FontSize(10D)
                        .Bold()
                        .Color(Color.Green); 
            }
            else if(Cproperties.FinalResult == "Failed")
            {
                TabPar.Append("\nFinal Result: " + Cproperties.FinalResult)
                    .Font(new Font("Verdana"))
                    .FontSize(10D)
                    .Bold()
                    .Color(Color.Red);
            }
            else
            {
                TabPar.Append("\nFinal Result: " + Cproperties.FinalResult)
                    .Font(new Font("Verdana"))
                    .FontSize(10D)
                    .Bold()
                    .Color(Color.Orange);
            }
               
            doc.AddFooters();
            Footer footer = doc.Footers.Odd;
            footer.PageNumbers = true;


            //***Add a picture 
            /*if (Cproperties.ArrayOfGraphics != null)
            {
                //Replace "Slash" with "Backslash"
                Cproperties.ArrayOfGraphics = Cproperties.ArrayOfGraphics.Replace("/", "\\");

                //Create array and check the length
                string[] graphicArrayOutput = Cproperties.ArrayOfGraphics.Split(';');

                int i = 1;

                if (graphicArrayOutput.Length > 1)
                {
                    foreach (var item in graphicArrayOutput)
                    {
                        var pngPath = Path.Combine(FrmMain.TestcaseSourcePath, item);
                        Image img = doc.AddImage(pngPath);
                        Picture p = img.CreatePicture();
                        Paragraph par = doc.InsertParagraph();
                        par.AppendPicture(p).SpacingAfter(10D);
                        par.Append("\nGraphic " + i);
                        par.Bold();
                        i++;
                    }
                }
                else
                {
                    string[] arr = graphicArrayOutput[0].Split('@');
                    var pngPath = Path.Combine(FrmMain.TestcaseSourcePath, graphicArrayOutput[0]);
                    Image img = doc.AddImage(pngPath);
                    Picture p = img.CreatePicture();
                    Paragraph par = doc.InsertParagraph();
                    par.AppendPicture(p).SpacingAfter(10D);
                    par.Append("\nGraphic " + i);
                    par.Bold();
                } 
            }*/

            doc.Save();
            FrmMain.ResetVariables();
        }
    }
}
