using System;
using System.Collections.Concurrent;
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

        private readonly TimeSpan responseTimeout = TimeSpan.FromSeconds(3);
        private readonly int playerId;
        private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
        private Process process = new Process();
        private AutoResetEvent messageHandle = new AutoResetEvent(false);

        public PlayerProcess(string processString, int id)
        {
            playerId = id;

            ProcessType processType = ParseProcessType(processString);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            SetStartArguments(startInfo, processString, processType);

            process.StartInfo = startInfo;
            process.OutputDataReceived += (s, a) =>
            {
                if (a.Data != null)
                {
                    messages.Enqueue(a.Data);
                    messageHandle.Set();
                }
            };

            process.Start();
            process.BeginOutputReadLine();
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

        public void SendMessage(string message)
        {
            Console.WriteLine($"{message} -> {playerId}");
            process.StandardInput.WriteLine(message);
        }

        public string ReceiveResponse()
        {
            // Try to read from the queue if not empty, if it is empty, wait for a message to arrive

            if (!messages.TryDequeue(out string result))
            {
                if (messageHandle.WaitOne(responseTimeout))
                {
                    messages.TryDequeue(out result);
                }
            }

            messageHandle.Reset();

            Console.WriteLine($"{result} <- {playerId}");
            return result;
        }
    }
}
