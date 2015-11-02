using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorTex
{
    class Console
    {
        public static ConsoleColor defColor = ConsoleColor.White;

        public static void WriteLine(string message = "", bool pad = false)
        {
            write(message);
            if (pad)
                System.Console.WriteLine(" " + message); //padding :P
            else
                System.Console.WriteLine(message);
        }

        public static void Write(string message = "")
        {
            System.Console.WriteLine(message);
            write(message);
        }

        public static void WriteError(string message = "", string error = "", bool pad = false)
        {
            ForegroundColor(ConsoleColor.Red);
            writeToErr(message + "\n" + error);
            if (pad)
            {
                System.Console.WriteLine(" " + message); //padding :P
                System.Console.WriteLine(" " + error);
            }
            else
            {
                System.Console.WriteLine(message);
                System.Console.WriteLine(error);
            }
            ForegroundColor(ConsoleColor.White);
        }

        public static void WriteWarning(string message = "", bool pad = false)
        {
            ForegroundColor(ConsoleColor.Yellow);
            writeToErr("Warning!\n "+ message);
            if (pad)
                System.Console.WriteLine(" " + message); //padding :P
            else
                System.Console.WriteLine(message);
            ForegroundColor(ConsoleColor.White);
        }

        public static void ForegroundColor(ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
        }

        public static void BackgroundColor(ConsoleColor color)
        {
            System.Console.BackgroundColor = color;
        }

        public static void write(string content)
        {
            using (StreamWriter w = File.AppendText("logs/[" + System.DateTime.Now.ToString("dd-MM-yyyy") + "] Logs.log"))
            {
                w.WriteLine("\n[{0}] " + content, DateTime.Now.ToLongTimeString());
                w.Close();
            }
        }

        public static void writeToErr(string content)
        {
            using (StreamWriter w = File.AppendText("logs/[" + System.DateTime.Now.ToString("dd-MM-yyyy") + "] Errors.log"))
            {
                w.WriteLine("\n[{0}] " + content, DateTime.Now.ToLongTimeString());
                w.Close();
            }
        }
    }
}
