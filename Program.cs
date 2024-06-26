﻿using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;

namespace WinFileReadEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            if (TraceEventSession.IsElevated() != true)
            {
                Console.WriteLine("[ERROR] To turn on ETW events you need to be Administrator, please run from an Admin process.");
                return;
            }

            string? filePath = (args.Length < 1) ? null : args[0];

            if (filePath == "testRun") {
                Console.WriteLine("[BREAK] testRun, exiting");
                return;
            }

            Console.CancelKeyPress += delegate {
                Console.WriteLine("[BREAK] Exiting");
            };

            using var session = new TraceEventSession("FileRead");

            Console.CancelKeyPress += (sender, e) => session.Stop();

            session.EnableKernelProvider(
                KernelTraceEventParser.Keywords.DiskFileIO |
                KernelTraceEventParser.Keywords.FileIOInit);

            session.Source.Kernel.FileIORead += data =>
            {
                if (filePath == null || string.Compare(data.FileName, filePath, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    string line = "[EVENT] ";
                    line += data.Offset + "|";
                    line += data.IoSize + "|";
                    line += data.FileName;
                    Console.WriteLine(line);
                }
            };

            Console.WriteLine("[READY] Starting process");
            session.Source.Process();
        }
    }
}