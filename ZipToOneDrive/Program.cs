using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ionic.Zip;
using System.IO;

namespace ZipToOneDrive
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get params from XML
            GetXMLParams(out string password, out string oneDrivePath);

            //Debug
            //args = new string[] {@"C:\Java"};
            //args = new string[] { @"C:\Test_zaZip" };
            //args = new string[] { @"C:\TestVideo\Test_pic\test_1.jpg"};

            //Loop through list
            for (int i = 0; i < args.Length; i++)
            {
                //ZIPing

                String putanja = args.ElementAt(i);
                String nazivDatoteke = args.ElementAt(i);
                int lastIndex = nazivDatoteke.LastIndexOf(@"\");
                nazivDatoteke = nazivDatoteke.Substring(lastIndex);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Zipping folder: " + nazivDatoteke);
                Console.WriteLine(@"Path: {0}", putanja);

                try
                {
                    ZipFile zip = new ZipFile(oneDrivePath + nazivDatoteke + ".zip");
                    zip.UseZip64WhenSaving = Zip64Option.Always;
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                    zip.Password = password;
                    zip.SaveProgress += Zip_SaveProgress;

                    // get the file attributes for file or directory
                    FileAttributes attr = File.GetAttributes(putanja);

                    if (attr.HasFlag(FileAttributes.Directory))
                        zip.AddDirectory(args.ElementAt(i));
                    else
                        zip.AddFile(args.ElementAt(i));

                    zip.Save();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("Zipping successful !!!");
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: {0}", e.Message);
                    Console.WriteLine();
                    Console.ResetColor();
                }

            }

            Console.ReadKey();
        }

        private static void Zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            long bytesTransferred = e.BytesTransferred;
            long totalBytesToTransfer = e.TotalBytesToTransfer;
            int ukupno = e.EntriesTotal;
            int trenutno = e.EntriesSaved;

            if ((bytesTransferred % 17 == 0 && totalBytesToTransfer > 0) || 
                (bytesTransferred == totalBytesToTransfer && totalBytesToTransfer > 0))
            {
                double postotak = (double)bytesTransferred / (double)totalBytesToTransfer * 100;
                postotak = Math.Round(postotak, 2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\r{0}%   ", postotak);
                Console.ResetColor();
            }
        }

        private static void GetXMLParams(out string password, out string oneDrivePath)
        {
            XDocument xdoc = XDocument.Load("Params.xml");
            string pass = xdoc.Descendants("ZipPassword").First().Value;
            string path = xdoc.Descendants("OneDrivePath").First().Value;

            password = pass;
            oneDrivePath = path;
        }
    }
}
