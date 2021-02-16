using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStalker.ConsoleUi
{
    public class UserPromptService
    {
        private readonly ArtistQueryService _artistQueryService;

        private readonly static Dictionary<int, string> _operations =
            new()
            {
                { 1, "Query Artists" },
                { 0, "Exit" }
            };

        public UserPromptService(
            ArtistQueryService artistQueryService
        )
        {
            _artistQueryService = artistQueryService ?? throw new ArgumentNullException(nameof(artistQueryService));
        }

        public async Task<bool> PromptUserAsync()
        {
            var selectedOperation = SelectOperation();
            if (selectedOperation == 0) return false;

            Console.WriteLine($"Selected operation: {_operations[selectedOperation.Value]}");
            Console.WriteLine();

            switch(selectedOperation) {
                case 1:
                    await _artistQueryService.ExecuteAsync();
                    break;
            }

            return true; // since we're turning true, the host will run this again
        }

        public static int? SelectOperation()
        {
            StringBuilder s = new();
            s.AppendLine();
            s.AppendLine("SELECT AN OPERATION:");

            foreach (var op in _operations)
            {
                if (op.Key == 0)
                {
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
