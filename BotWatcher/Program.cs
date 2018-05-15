using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace BotWatcher
{
    class Program
    {
        private const string processesPath = "C:\\Olfi01\\BotWatcher\\processes.txt";
        private static List<string> procnames;
        private static Timer timer;
        static void Main(string[] args)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(processesPath));
            if (!File.Exists(processesPath)) File.Create(processesPath).Close();
            procnames = File.ReadAllLines(processesPath).ToList();
            timer = new Timer(CheckProcesses, null, 1000, Timeout.Infinite);
            string input;
            do input = Console.ReadLine();
            while (input != "exit");
        }

        private static void CheckProcesses(object state)
        {
            foreach (var path in procnames)
            {
                var name = path.Split('\\').Last();
                name = name.Remove(name.LastIndexOf(".exe"));
                var procs = Process.GetProcessesByName(name);
                foreach (var proc in procs)
                {
                    if (!proc.Responding) proc.Kill();
                }
                procs = Process.GetProcessesByName(name);
                if (procs.Length < 1)
                {
                    Process.Start(new ProcessStartInfo(path) { WorkingDirectory = Path.GetDirectoryName(path) });
                }
            }
            timer = new Timer(CheckProcesses, null, 1000, Timeout.Infinite);
        }
    }
}
