using Cli.NET.Abstractions.Actions;
using Cli.NET.External.Models.Windows;
using Cli.NET.Tools;
using System.Diagnostics;
using System.Text;

namespace Cli.NET.External.PlatformUtils
{
    public class WindowsCommand : ICommand
    {
        private readonly OutputProvider? _outputProvider;
        private readonly string _workDirectory;
        private readonly WindowsCommandHandler _commandHandler;
        public WindowsCommand(
            WindowsCommandHandler handler = WindowsCommandHandler.DirectHandler,
            OutputProvider? outputProvider = null)
        {
            _workDirectory = Environment.GetEnvironmentVariable("windir") ?? Environment.CurrentDirectory;
            _commandHandler = handler;
        }

        public void Execute(string[] arguments)
        {
            switch(_commandHandler)
            {
                case WindowsCommandHandler.CommandPrompt:
                    var output = CallCommandPrompt(arguments);

                    break;
            }
        }

        private string CallCommandPrompt(string[] arguments)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", string.Join("", arguments))
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = _workDirectory
            };

            StringBuilder sb = new();
            Process? p = Process.Start(processInfo);
            p.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
            p?.BeginOutputReadLine();
            p?.WaitForExit();

            return sb.ToString();
        }
    }
}