using Cli.NET.Abstractions.Actions;
using Cli.NET.External.Models.Windows;

namespace Cli.NET.External.PlatformUtils
{
    public class WindowsCommand : ICommand
    {
        private readonly string _workDirectory;
        private readonly WindowsCommandHandler _commandHandler;
        public WindowsCommand(WindowsCommandHandler handler = WindowsCommandHandler.DirectHandler)
        {
            _workDirectory = Environment.GetEnvironmentVariable("windir") ?? Environment.CurrentDirectory;
            _commandHandler = handler;
        }

        public void Execute(string[] arguments)
        {
            switch(_commandHandler)
            {
                case WindowsCommandHandler.CommandPrompt:
                    break;
            }
        }

        private void CallCommandPrompt(string[] arguments)
        {

        }
    }
}