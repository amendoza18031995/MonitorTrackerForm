using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerMonitor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var process = Process.GetCurrentProcess();
            string fullPath = process.MainModule.FileName.Replace("Worker\\WorkerMonitor.exe", "");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Process[] pname = Process.GetProcessesByName("MonitorTrackerForm");
                    writelog("Validando si el proceso esta activo o no para correrlo: " + pname.Length.ToString(), string.Empty);
                    if (pname.Length <= 0)
                    {
                        writelog("Inactivo, Corriendo monitor", string.Empty);
                        //Process.Start(fullPath + "MonitorTrackerForm.exe", "MonitorTrackerForm.exe");
                        ProcessShell("powershell -noprofile -executionpolicy bypass -file D:\\Proyectos\\AutoMonitor.ps1 -WindowStyle Hidden -noninteractive");
                    }
                }
                catch (Exception ex)
                {
                    writelog("Error: " + ex.Message, string.Empty);
                }
                await Task.Delay(180000, stoppingToken);
            }
        }

        public void ProcessShell(string script)
        {
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipe = runspace.CreatePipeline();
            pipe.Commands.AddScript(script);
            pipe.Commands.Add("Out-String");

            Collection<PSObject> results = pipe.Invoke();
            runspace.Close();
        }
        public void writelog(string msg, string module)
        {
            string realpath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            
            var process = Process.GetCurrentProcess();
            string fullPath = process.MainModule.FileName.Replace("\\Worker", "");

            string directory = fullPath;
            string path = fullPath + "\\log.txt";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("Iniciando");
                sw.WriteLine(realpath);

                sw.WriteLine(msg);
                sw.Close();
            }
            else
            {
                List<string> lines = new List<string>();
                lines.Add(msg + " --/ " + module);
                File.AppendAllLines(path, lines);
            }
        }
    }
}
