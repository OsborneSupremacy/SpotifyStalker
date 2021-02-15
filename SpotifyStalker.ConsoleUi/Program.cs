using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpotifyStalker.ConsoleUi
{
    class Program
    {
        private readonly static Dictionary<int, string> _operations = 
            new() {
                { 1, "Query Artists" },
                { 0, "Exit" }
            };

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var selectedOperation = SelectOperation();

            while (selectedOperation != 0)
            {
                Console.WriteLine($"Selected operation: {selectedOperation.Value}");
                Console.WriteLine();
                selectedOperation = SelectOperation();
            };

            Environment.Exit(0);
        }

        public static int? SelectOperation()
        {
            StringBuilder s = new();
            s.AppendLine("SELECT AN OPERATION:");

            foreach (var op in _operations) {
                if(op.Key == 0) {
                    s.AppendLine($"Any Other Key: {op.Value}");
                    continue;
                }
                s.AppendLine($"{op.Key}. {op.Value}");
            }

            Console.Write(s.ToString());

            var i = Console.ReadKey();
            Console.WriteLine();

            i.KeyChar.ToString();

            if (!int.TryParse(i.KeyChar.ToString(), out var input)) return 0;
            if (!_operations.TryGetValue(input, out _)) return 0;

            return input;
        }
    }
}
