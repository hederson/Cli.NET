using Cli.NET.Abstractions.Actions;
using Cli.NET.External.Models.Windows;
using Cli.NET.Tools;
using Cli.NET.Models;
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
                    try
                    {
                        var output = CallCommandPrompt(arguments);
                        if (_outputProvider != null) _outputProvider.AddLog(output, OutputType.Message);
                    }
                    catch (Exception ex)
                    {
                        if (_outputProvider != null) _outputProvider.AddLog(ex);
                    }
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

            StringBuilder stringBuilder = new();
            Process? process = Process.Start(processInfo);

            if(process != null)
            {
                process.OutputDataReceived += (sender, args) => stringBuilder.AppendLine(args.Data);
                process.BeginOutputReadLine();
                process.WaitForExit();
            }

            return stringBuilder.ToString();
        }
    }
}