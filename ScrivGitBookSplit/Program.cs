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
            System.IO.StreamWriter swOutFile = null;
            string strLine = null;
            string strOutFileName = String.Empty;

            string strFileName = Path.GetFileName(strInFile);

            strOutputFilePath = strOutputFilePath + "\\Manuscript";

            bool boolExists = Directory.Exists(strOutputFilePath);

            if (boolExists != true)
            {
                Directory.CreateDirectory(strOutputFilePath);
            }

            strOutFileName = strOutputFilePath + "\\" + strFileName;

            FileStream fs = new FileStream(strOutFileName, FileMode.Create, FileAccess.ReadWrite);

            swOutFile = new StreamWriter(fs, Encoding.Default);

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

                                intState = 2;
                            }
                        }

                        swOutFile.WriteLine(strLine);

                        //intState = 0;
                    }
                }
            }
            finally
            {
                if (swOutFile != null)
                {
                    swOutFile.Dispose();
                }
            }
        }
    }
}
