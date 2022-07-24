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
            Dictionary<ArgumentNames, object> pArgs;
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
        static bool VerifyArguments(string[] args, out Dictionary<ArgumentNames, object> pArgs)
        {
            pArgs = null;
            if (args == null || args.Length == 0)
            {
                return true;
            }

            //Setup default values
            pArgs = new Dictionary<ArgumentNames, object>();
            pArgs.Add(ArgumentNames.Count, 100);
            pArgs.Add(ArgumentNames.Baudrate, 115200);
            pArgs.Add(ArgumentNames.PacketSizeStart, 1);
            pArgs.Add(ArgumentNames.PacketSizeEnd, 100);
            pArgs.Add(ArgumentNames.LoopbackType, Loopback.LoopbackType.Dynamic);

            int tmp = 0;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-comport":
                        pArgs.Add(ArgumentNames.ComPort, args[i + 1]);
                        break;
                    case "-packetsizestart":
                        try
                        {
                            tmp = Convert.ToInt32(args[i + 1]);
                            Console.WriteLine("Error: Argument {0} is not a number.", args[i]);
                        }
                        catch
                        {
                            return false;
                        }
                        pArgs[ArgumentNames.PacketSizeStart] = tmp;
                        break;
                    case "-packetsizeend":
                        try
                        {
                            tmp = Convert.ToInt32(args[i + 1]);
                            Console.WriteLine("Error: Argument {0} is not a number.", args[i]);
                        }
                        catch
                        {
                            return false;
                        }
                        pArgs[ArgumentNames.PacketSizeEnd] = tmp;
                        break;
                    case "-count":
                        try
                        {
                            tmp = Convert.ToInt32(args[i + 1]);
                            Console.WriteLine("Error: Argument {0} is not a number.", args[i]);
                        }
                        catch
                        {
                            return false;
                        }
                        pArgs[ArgumentNames.Count] = tmp;
                        break;
                    default:
                        break;
                }
            }

            return true;
        }
    }
}
