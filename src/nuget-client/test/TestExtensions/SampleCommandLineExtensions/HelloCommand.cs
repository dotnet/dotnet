using System.ComponentModel.Composition;
using NuGet;
using NuGet.CommandLine;

namespace SampleCommandLineExtensions
{
    [Export]
    [Command("hello", "Says \"Hello!\"")]
    public class HelloCommand : Command
    {
        public override void ExecuteCommand()
        {
            Console.WriteLine("Hello!");
        }
    }
}
