using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace reviews_service.test.acceptance
{
    public class ExternalProgram: IDisposable
    {
        private Process _process;

        public ExternalProgram(string exePath, params string[] args)
        {
            var arguments = string.Join(" ", args.Select(a => $"\"{a.Replace("\"", "\\\"")}\""));

            _process = new Process
            {
                StartInfo =
                {
                    FileName = exePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };

            _process.OutputDataReceived += OnConsoleMessageReceived;
            _process.ErrorDataReceived += OnConsoleMessageReceived;
        }

        private void OnConsoleMessageReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            Console.WriteLine(e.Data);
            LastConsoleMessageReceived = e.Data;
        }

        public string LastConsoleMessageReceived { get; private set; }

        public int ExitCode => _process.ExitCode;

        public void Start()
        {
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        public void Dispose()
        {
            if (_process == null) return;

            _process.Dispose();
            _process = null;
        }

        public void WaitForExit(TimeSpan timeout)
        {
            _process.WaitForExit((int)timeout.TotalMilliseconds);
        }

        public void Kill()
        {
            if (_process.HasExited)
            {
                var processName = Path.GetFileName(_process.StartInfo.FileName);
                Console.WriteLine($"*** This test failed because the process [{processName}] exited unexpectedly early ***");
            }
            else
            {
                _process.Kill();
            }
        }
    }
}