using SimplexProject.Core;
using SimplexProject.Utils;

namespace SimplexProject
{
    static class Program
    {
        static void Main(string[] args)
        {
            var consoleInput = new ConsoleInput();
            LPTask task = consoleInput.GetInput();
        }
    }
}
