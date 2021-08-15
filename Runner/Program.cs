using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] pname = Process.GetProcessesByName("MonitorTrackerForm");
            if (pname.Length >= 1)
            {
                Environment.Exit(1023534368);
            }
            else
            {
                TaskService ts = new TaskService();
                Task t = ts.GetTask("MonitorTraker");
                if (t != null)
                    t.Run();
            }

            //try
            //{
            //    Console.WriteLine("*****************");
            //    Process[] pname = Process.GetProcessesByName("MonitorTrackerForm");
            //    var process = Process.GetCurrentProcess();
            //    string fullPath = process.MainModule.FileName.Replace("Runner.exe", "");
            //    Console.WriteLine(fullPath);
            //    Console.ReadLine();
            //    writelog("", "");
            //    string scritp = "powershell - noprofile - executionpolicy bypass - file" + fullPath + "\\AutoMonitor.ps1 - WindowStyle Hidden - noninteractive";

            //    if (pname.Length > 1)
            //    {
            //        Environment.Exit(1023534368);
            //    }
            //    else
            //    {
            //        Runspace runspace = RunspaceFactory.CreateRunspace();
            //        runspace.Open();

            //        Pipeline pipe = runspace.CreatePipeline();
            //        pipe.Commands.AddScript(scritp);
            //        pipe.Commands.Add("Out-String");
            //        pipe.Invoke();
            //        runspace.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    Console.ReadLine();
            //    writelog("", "");
            //}
        }
    }
}
