
using System;
using System.Threading.Tasks;

namespace GSdkNet.Carrier.Example {
    abstract class BasicExample {
        private string Name => GetType().Name;

        public void PrintInfo(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Name + ": " + message);
            Console.ResetColor();
        }

        public void PrintWarning(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Name + ": " + message);
            Console.ResetColor();
        }

        public void PrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Name + ": " + message);
            Console.ResetColor();
        }

        public void WaitForInput() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Name + ": " + "Press any key to continue");
            Console.ResetColor();
            Console.ReadLine();
        }

        public abstract Task StartAsync();

        public abstract Task StopAsync();
    }
}
