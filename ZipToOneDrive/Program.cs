using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ionic.Zip;

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

                    zip.AddDirectory(args.ElementAt(i));
                    zip.Save();

                    Console.ForegroundColor = ConsoleColor.Green;
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
