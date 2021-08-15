using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32.TaskScheduler;
namespace MonitorTrackerForm
{
    [RunInstaller(true)]
    public partial class WhisperInstaller : Installer
    {
        public WhisperInstaller()
        {
            InitializeComponent();
        }

        public async override void Install(IDictionary stateSaver)
        {
            try
            {
                base.Install(stateSaver);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Error: " + ex.Message);
            }

        }

        public override void Commit(IDictionary savedState)
        {
            try
            {
                string path = Assembly.GetExecutingAssembly().Location;
                string pathfolder = Assembly.GetExecutingAssembly().Location.Replace("MonitorTrackerForm.exe", "");

                //creando el ps1

                //StreamWriter sw = new StreamWriter(pathfolder + @"Worker\AutoMonitor.ps1");
                //string startprogram = "start " + (char)34 + Assembly.GetExecutingAssembly().Location + (char)34;
                //sw.WriteLine(startprogram);
                //sw.Close();

                ///creando la tarea runner
                //esta valida si el sistema esta corriendo y sino, tira la otra tarea
                //using (TaskService ts = new TaskService())
                //{
                //    // Create a new task definition and assign properties
                //    TaskDefinition td = ts.NewTask();

                //    td.RegistrationInfo.Description = "MonitorRunner";

                //    LogonTrigger lt = new LogonTrigger();
                //    lt.Delay = TimeSpan.FromMinutes(2); // V2 only

                //    BootTrigger bt = new BootTrigger();
                //    bt.Delay = TimeSpan.FromMinutes(2);

                //    RegistrationTrigger rTrigger = new RegistrationTrigger();
                //    rTrigger.Delay = TimeSpan.FromMinutes(5);

                //    new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.SessionUnlock };
                //    new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.SessionLock };

                //    // Create a trigger that will fire the task at this time every other day
                //    td.Triggers.Add(lt);
                //    td.Triggers.Add(bt);
                //    td.Triggers.Add(rTrigger);

                //    TimeSpan span = new TimeSpan(0, 0, 5, 0, 0);
                //    TimeSpan spanduration = new TimeSpan(0, 0, 0, 0, 0);

                //    RepetitionPattern rpp = new RepetitionPattern(span, spanduration, false);
                //    td.Triggers.Add(new DailyTrigger { Repetition = rpp });

                //    string execution = pathfolder + @"Worker\Runner.exe";
                //    td.Actions.Add(new ExecAction(execution, "", null));

                //    // Register the task in the root folder
                //    ts.RootFolder.RegisterTaskDefinition(@"MonitorRunner", td);
                //}


                //creando la tarea que lanza la app
                //using (TaskService ts = new TaskService())
                //{
                //    // Create a new task definition and assign properties
                //    TaskDefinition td = ts.NewTask();

                //    td.RegistrationInfo.Description = "MonitorTraker";

                //    string execution = "-ExecutionPolicy Bypass -File " + (char)34 + pathfolder + @"Worker\" + "AutoMonitor.ps1" + (char)34;
                //    td.Actions.Add(new ExecAction("PowerShell.exe", execution, null));

                //    // Register the task in the root folder
                //    ts.RootFolder.RegisterTaskDefinition(@"MonitorTraker", td);
                //}

                writelog("intentando lanzar la app desde el installer lanzando la app", string.Empty);
                writelog("en la ruta:" + path, string.Empty);
                Process.Start(path, "MonitorTrackerForm.exe");

                ProcessStartInfo startInfo = new ProcessStartInfo(path, path);
                //Process.Start(startInfo);



                //string strCmdText = path.Replace("MonitorTrackerForm.exe", "") + "Worker\\WorkerMonitor.exe";
                //string svcDispName = "WorkerMonitor";
                //string svcName = "WorkerMonitor";

                //SvcInstaller svc = new SvcInstaller();
                //svc.InstallService(strCmdText, svcName, svcDispName);

            }
            catch (Exception ex)
            {
                writelog("lanzando la app error: " + ex.Message, string.Empty);
            }
        }

        public void writelog(string msg, string module)
        {
            string appdirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string directory = "C:\\Logs";
            string path = "C:\\Logs\\log.txt";


            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("Iniciando");
                sw.WriteLine(appdirectory);

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
