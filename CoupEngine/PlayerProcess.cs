using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoupEngine
{
    internal class PlayerProcess
    {
        public PlayerProcess(string processString)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = processString; //"PythonApp.py";
            startInfo.FileName = "C:\\Program Files (x86)\\Microsoft Visual Studio\\Shared\\Python37_64\\python.exe";

            Process p = new Process();
            p.StartInfo = startInfo;
            p.OutputDataReceived += (s, a) =>
            {
                Console.WriteLine("Received: {0}", a.Data);
                p.StandardInput.WriteLine("Greetings");
            };

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
        }
    }
}
