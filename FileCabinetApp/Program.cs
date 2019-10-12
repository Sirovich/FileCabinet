using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Ivan Sarokvashin";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly FileCabinetService FileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'help' command creates new record." },
            new string[] { "edit", "updates record", "The 'help' command updates record." },
            new string[] { "list", "prints all records", "The 'help' command prints all records." },
            new string[] { "stat", "prints count of records", "The 'stat' command prints count of records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void List(string parameters)
        {
            var list = FileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in list)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Sex}, {record.Weight}, {record.Height}, {record.DateOfBirth.ToString("yyyy-MMM-dd", new CultureInfo("us-US"))}");
            }
        }

        private static void Create(string parameters)
        {
            while (true)
            {
                try
                {
                    Console.Write("First name: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Last name: ");
                    string lastName = Console.ReadLine();
                    Console.Write("Sex: ");
                    char sex = Convert.ToChar(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.Write("Weight: ");
                    decimal weight = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.Write("Height: ");
                    short height = Convert.ToInt16(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.Write("Date of birth: ");
                    DateTime dateOfBirth = DateTime.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    int record = FileCabinetService.CreateRecord(height, weight, sex, firstName, lastName, dateOfBirth);
                    Console.WriteLine("Record #{0} is created.", record);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}.");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void Edit(string parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            int id = Convert.ToInt32(parameters, CultureInfo.InvariantCulture);
            var records = FileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in records)
            {
                if (record.Id == id)
                {
                    try
                    {
                        Console.Write("First name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Last name: ");
                        string lastName = Console.ReadLine();
                        Console.Write("Sex: ");
                        char sex = Convert.ToChar(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.Write("Weight: ");
                        decimal weight = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.Write("Height: ");
                        short height = Convert.ToInt16(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.Write("Date of birth: ");
                        DateTime dateOfBirth = DateTime.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        FileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, sex, height, weight);
                        Console.WriteLine($"Record #{id} is updated.");
                        return;
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (OverflowException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            Console.WriteLine($"#{id} record is not found.");
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}