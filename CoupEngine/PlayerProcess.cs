using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoupEngine
{
    internal class PlayerProcess
    {
        private enum ProcessType
        {
            Exe,
            Python,
            Javascript
        }

        public PlayerProcess(string processString)
        {
            ProcessType processType = ParseProcessType(processString);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            SetStartArguments(startInfo, processString, processType);

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

        private ProcessType ParseProcessType(string processString)
        {
            var stringPieces = processString.Split('.');
            var fileExtension = stringPieces[stringPieces.Length - 1];

            switch (fileExtension.ToLower())
            {
                case "exe":
                    return ProcessType.Exe;

                case "js":
                    return ProcessType.Javascript;

                case "py":
                    return ProcessType.Python;
            }

            throw new ArgumentException();
        }

        private void SetStartArguments(ProcessStartInfo startInfo, string processString, ProcessType processType)
        {
            switch (processType)
            {
                case ProcessType.Exe:
                    startInfo.FileName = processString;
                    break;

                case ProcessType.Javascript:
                    startInfo.FileName = "node";
                    startInfo.Arguments = processString;
                    break;

                case ProcessType.Python:
                    startInfo.FileName = "python";
                    startInfo.Arguments = processString;
                    break;
            }
        }
    }
}
