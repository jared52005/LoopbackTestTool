using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LTT
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        public static int ExitCode { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Dictionary<ArgumentNames, string> pArgs;
            ExitCode = 0;
            if (VerifyArguments(args, out pArgs))
            {
                if (pArgs.Count == 0)
                {
                    FreeConsole();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    CLI cli = new CLI();
                    ExitCode = cli.Execute(pArgs);
                }
                return ExitCode;
            }
            else
            {
                ExitCode = -1;
            }

            return ExitCode;
        }
        static bool VerifyArguments(string[] args, out Dictionary<ArgumentNames, string> pArgs)
        {
            pArgs = null;
            if (args == null || args.Length == 0)
            {
                return true;
            }

            pArgs = new Dictionary<ArgumentNames, string>();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-comport":
                        pArgs.Add(ArgumentNames.ComPort, args[i + 1]);
                        break;
                    default:
                        break;
                }
            }

            return true;
        }
    }
}
