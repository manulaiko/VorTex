using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Fiddler;

namespace VorTex
{
    class Program
    {
        public static string version = "2.2";
        public static Dictionary<string, string> resources = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            System.Console.Title = "DarkOrbit VorTex "+ version +" by Manulaiko";
            System.Console.SetWindowSize(95, 30);
            Console.ForegroundColor(ConsoleColor.Green);
            Console.WriteLine("|||          ///   OOOOOOOOOOOO    |||||||||||   ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡  ((((((((((((  \\\\\\       ///");
            Console.WriteLine("|||        ///   OOO        OOO   |||     ||||        |||         (((             \\\\\\    /// ");
            Console.WriteLine("|||      ///   OOO          OOO  |||    ||||         |||         (((((((((         \\\\\\///    ");
            Console.WriteLine("|||    ///    OOO          OOO  |||||||||           |||         ((((((            ///\\\\\\     ");
            Console.WriteLine("|||  ///      OOO        OOO   |||   |||           |||         (((             ///    \\\\\\    ");
            Console.WriteLine("|||///        OOOOOOOOOOOO    |||     |||         |||         ((((((((((((  ///        \\\\\\");
            Console.WriteLine();
            Console.WriteLine("                              DarkOrbit VorTex " + version + " by Manulaiko");
            Console.WriteLine();
            Console.ForegroundColor(ConsoleColor.White);

            Console.WriteLine("Reading resources.xml...");
            readXml("resources.xml");
            Console.WriteLine("resources.xml readed!");

            Console.WriteLine("Clearing cache...");
            Interaction.Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 255", AppWinStyle.MinimizedFocus, true, -1);
            Console.WriteLine("Done!");

            Console.WriteLine("Starting proxy...");
            Start();

            while (true)
            {
                string command = System.Console.ReadLine();
                string[] c = new string[1];
                c[0] = "close";

                if (command == c[0])
                {
                    Console.WriteLine("Closing proxy...");
                    FiddlerApplication.Shutdown();
                    Console.WriteLine("Proxy closed!");
                    Console.WriteLine("See you!!");
                    Environment.Exit(0);
                }
            }
        }

        public static void Start()
        {
            FiddlerApplication.Startup(0, (FiddlerCoreStartupFlags)187);
            Console.WriteLine("Proxy started!");

            Console.WriteLine("Do you want to use the translating function?");
            Console.ForegroundColor(ConsoleColor.Blue);
            string translate = System.Console.ReadLine();
            Console.ForegroundColor(ConsoleColor.White);
            if (translate.ToLower().Substring(0, 1) == "y")
            {
                Console.WriteLine("Translating function is now activated!");
                Console.WriteLine("Write the shortcut of your language (Spanish = es, English(UK) = en, English(US) = us, German = de, French = fr...)");
                Console.ForegroundColor(ConsoleColor.Blue);
                string lang = System.Console.ReadLine();
                Console.ForegroundColor(ConsoleColor.White);

                changeResource("translationSpacemap.php", "translations/" + lang + ".php");
            }

            Console.WriteLine("Waiting for resources...");
            Task.Factory.StartNew(loadResources);
        }

        public static void loadResources()
        {
            foreach (var res in resources)
            {
                changeResource(res.Key, res.Value);
            }
        }

        public static string getPath(string path)
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), path);
            return path;
        }

        public static void changeResource(string resource, string path, string url = "internalMapRevolution")
        {
            FiddlerApplication.BeforeRequest += delegate(Session session)
            {
                if (session.fullUrl.Contains(url))
                {
                }
                if (!session.fullUrl.Contains(resource))
                {
                    return;
                }
                session.bBufferResponse = true;
            };

            FiddlerApplication.BeforeResponse += delegate(Session session)
            {
                if (session.fullUrl.Contains(url))
                {
                }
                if (!session.fullUrl.Contains(resource)) {
                    return;
                }

                if (session.LoadResponseFromFile(getPath(path))) {
                    Console.WriteLine(resource + " changed!");
                } else {
                    Console.WriteError("Couldn't load resource: " + path + " which is used to replace " + resource + "\nCheck resources.xml");
                }
            };
        }

        public static void readXml(string file)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(file);

            XmlNodeList spacemap = xml.GetElementsByTagName("spacemap");

            XmlNodeList resource = ((XmlElement)spacemap[0]).GetElementsByTagName("res");

            foreach (XmlElement nodo in resource)
            {

                string name = nodo.GetAttribute("name");
                string path = nodo.GetAttribute("path");

                resources.Add(name, path);
            }
        }
    }
}