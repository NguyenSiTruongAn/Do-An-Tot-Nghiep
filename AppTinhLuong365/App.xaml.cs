using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace AppTinhLuong365
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void RunPowerShellByAdmin1()
        {
            try
            {
                // Run PowerShell script as administrator
                /*using (PowerShell ps = PowerShell.Create())
                {
                    string path = "Start-Process powershell.exe -Verb RunAs -ArgumentList '-File " + $"{Environment.GetEnvironmentVariable("APPDATA")}/Chat365/script-certificate.ps1'";

                    string data = "Import-Certificate -FilePath " + Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.cer " + "-CertStoreLocation Cert:\\LocalMachine\\TrustedPublisher";
                    File.WriteAllText($"{Environment.GetEnvironmentVariable("APPDATA")}/Chat365/script-certificate.ps1", data);
                    
                    ps.AddScript(path);
                    ps.Invoke();
                }*/
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c certutil -addstore " + "\"TrustedPublisher" + "\" " + Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.cer";
                startInfo.Verb = "runas";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process = new System.Diagnostics.Process();
                startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c certutil -enterprise -f -v -AddStore " + "\"Root" + "\" " + Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.cer";
                startInfo.Verb = "runas";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                /*var processInfo = new ProcessStartInfo("cmd.exe", "certutil -addstore " + "\"TrustedPublisher" + "\" " + Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.cer")
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    //RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    Verb = "runas",
                    WorkingDirectory = @"C:\Windows\System32\"
                };

                StringBuilder sb = new StringBuilder();
                Process p = Process.Start(processInfo);
                p.OutputDataReceived += (sender, args_) => sb.AppendLine(args_.Data);
                p.BeginOutputReadLine();
                p.WaitForExit();*/
            }
            catch (Exception ex)
            {

            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificate"))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates");
            }
            if (!System.IO.File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.zip"))
            {
                using (WebClient web = new WebClient())
                {
                    try
                    {
                        web.DownloadFile(new Uri("https://app.timviec365.vn/Download/Chat365/Certificate/CONGTYCOPHANTHANHTOANHUNGHA.zip"), Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.zip");
                        ZipFile.ExtractToDirectory(Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\CONGTYCOPHANTHANHTOANHUNGHA.Zip", Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\Certificates\");
                        RunPowerShellByAdmin1();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            //RunPowerShellByAdmin();
            //RunPowerShellByAdmin1();
            //AddCertificate();
        }
    }
}
