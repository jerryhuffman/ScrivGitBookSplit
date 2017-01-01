using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrivGitBookSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var strArg1 = args[0];

                //string strOutPath = "C:\\tmp";
                string strInFileName = Path.GetFullPath(strArg1);
                string strOutPath = Path.GetDirectoryName(strArg1);

                Split(strInFileName, strOutPath);


            }
            catch (Exception Ex)
            {
                string strMessage = Ex.Message;
                Console.Write("Error: " + strMessage + "\r\n");
            }
        }

        public static void Split(string strInFile, string strOutputFilePath)
        {
            //System.IO.StreamWriter swOutFile0 = null;
            //string strOutFileName = String.Empty;

            //string strFileName0 = Path.GetFileName(strInFile);

            //string strOutputFilePath0 = strOutputFilePath + "\\Manuscript";

            //string strOutFileName0 = strOutputFilePath0 + "\\" + strFileName0;

            ////string strOutFileName1 = strOutputFilePath1 + "\\" + strFileName;

            //FileStream fs0 = new FileStream(strOutFileName0, FileMode.Create, FileAccess.ReadWrite);

            //System.IO.StreamWriter swOutFile0 = new StreamWriter(fs0, Encoding.Default);

            string strFileName1 = Path.GetFileNameWithoutExtension(strInFile);

            //var parentDir = file.Directory == null ? null : file.Directory.Parent; // null if root
            //if (parentDir != null)
            //{
            //}
            string strNewOutputFilePath = Path.GetFullPath(Path.Combine(strOutputFilePath, @"..\"));

            string strOutputFilePath1 = strNewOutputFilePath + "\\Manuscript" + "\\" + strFileName1;

            bool boolExists = Directory.Exists(strOutputFilePath1);

            if (boolExists != true)
            {
                Directory.CreateDirectory(strOutputFilePath1);
            }

            string strLine = null;
            Dictionary<string, string> dicChapterFileName = new Dictionary<string, string>();

            dicChapterFileName.Add("Title Page", "README.md");

            string strOutFileName1 = strOutputFilePath1 + "\\README.md";

            FileStream fs1 = new FileStream(strOutFileName1, FileMode.Create, FileAccess.ReadWrite);

            System.IO.StreamWriter swOutFile1 = new StreamWriter(fs1, Encoding.Default);

            try
            {
                using (var srInFile = new StreamReader(strInFile, Encoding.Default))
                {
                    int intState = 0;

                    while (srInFile.EndOfStream != true)
                    {
                        strLine = srInFile.ReadLine();

                        if (strLine.StartsWith("$$") == true)
                        {
                            int intIndex1 = strLine.IndexOf("$$");

                            if (intIndex1 == 0)
                            {
                                intState = 1;
                            }
                        }

                        if (strLine.StartsWith("#") == true)
                        {
                            int intIndex1 = strLine.IndexOf("#");
                            int intIndex2 = strLine.IndexOf("#", intIndex1 + 2);

                            int intLen = intIndex2 - intIndex1 - 2;

                            string strChapterName = null;

                            if (intLen > 0 && intState == 1)
                            {
                                strChapterName = strLine.Substring(intIndex1 + 1, intIndex2 - intIndex1 - 1).Trim();

                                strFileName1 = strChapterName.Replace(" ", "-");

                                strOutFileName1 = strOutputFilePath1 + "\\" + strFileName1 + ".md";

                                dicChapterFileName.Add(strChapterName, strChapterName + ".md");

                                intState = 2;
                            }
                        }

                        if (intState == 2)
                        {
                            //fs1.Flush(true);
                            //fs1.Close();
                            //fs1.Dispose();

                            swOutFile1.Flush();
                            swOutFile1.Close();
                            swOutFile1.Dispose();

                            fs1 = new FileStream(strOutFileName1, FileMode.Create, FileAccess.ReadWrite);
                            swOutFile1 = new StreamWriter(fs1, Encoding.Default);

                            intState = 0;
                        }

                        //swOutFile0.WriteLine(strLine);
                        swOutFile1.WriteLine(strLine);

                        //intState = 0;
                    }

                    if (dicChapterFileName.Count() > 0)
                    {
                        swOutFile1.Flush();
                        swOutFile1.Close();
                        swOutFile1.Dispose();

                        strOutFileName1 = strOutputFilePath1 + "\\SUMMARY.md";

                        fs1 = new FileStream(strOutFileName1, FileMode.Create, FileAccess.ReadWrite);
                        swOutFile1 = new StreamWriter(fs1, Encoding.Default);

                        swOutFile1.WriteLine("# Summary\r\n");

                        foreach (KeyValuePair<string, string> kvpChapterFileName in dicChapterFileName)
                        {
                            swOutFile1.WriteLine("* [" + kvpChapterFileName.Key + "] (" + kvpChapterFileName.Value + ")");
                        }
                    }
                }
            }
            //finally
            //{
            ////    if (swOutFile0 != null)
            ////    {
            ////        swOutFile0.Dispose();
            ////    }

            //    if (swOutFile1 != null)
            //    {
            //        swOutFile1.Dispose();
            //    }
            //}
            catch (Exception Ex)
            {
                string strMessage = Ex.Message;

                ////if (swOutFile0 != null)
                ////{
                ////    swOutFile0.Dispose();
                ////}

                //if (swOutFile1 != null)
                //{
                //    swOutFile1.Dispose();
                //}
            }

            //swOutFile0.Flush();
            //swOutFile0.Close();
            //swOutFile0.Dispose();

            swOutFile1.Flush();
            swOutFile1.Close();
            swOutFile1.Dispose();
        }
    }
}
