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
            pArgs = new Dictionary<ArgumentNames, object>();
            if (args == null || args.Length == 0)
            {
                return true;
            }

            //Setup default values
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
                        tmp = ConvertNumberWithSuffix(args[i], args[i + 1]);
                        if (tmp > 0)
                        {
                            pArgs[ArgumentNames.PacketSizeStart] = tmp;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "-packetsizeend":
                        tmp = ConvertNumberWithSuffix(args[i], args[i + 1]);
                        if (tmp > 0)
                        {
                            pArgs[ArgumentNames.PacketSizeEnd] = tmp;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "-count":
                        tmp = ConvertNumberWithSuffix(args[i], args[i + 1]);
                        if (tmp > 0)
                        {
                            pArgs[ArgumentNames.Count] = tmp;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private static int ConvertNumberWithSuffix(string argName, string argVal)
        {
            try
            {
                //Check if ends with k/m/g => add proper amount of zeros instead, kilo-mega-giga
                string sVal = argVal.ToLower();
                if (sVal.Contains("k"))
                {
                    sVal = sVal.Replace("k", "000");
                }
                else if (sVal.Contains("m"))
                {
                    sVal = sVal.Replace("m", "000000");
                }
                else if (sVal.Contains("g"))
                {
                    sVal = sVal.Replace("g", "000000000");
                }
                return Convert.ToInt32(sVal);
                
            }
            catch
            {
                Console.WriteLine("Error: Argument {0} is not a number ({1})", argName, argVal);
                return -1;
            }
        }
    }
}
