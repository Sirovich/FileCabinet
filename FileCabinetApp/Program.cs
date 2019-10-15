using System;
using System.Globalization;
using System.Resources;
using FileCabinetApp.Services;

namespace FileCabinetApp
{
    /// <summary>
    /// Main program class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Ivan Sarokvashin";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly FileCabinetService FileCabinetService = new FileCabinetCustomService();
        private static readonly ResourceManager Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'help' command creates new record." },
            new string[] { "edit", "updates record", "The 'help' command updates record." },
            new string[] { "find", "finds records by parameter", "The 'help' command finds records by parameter." },
            new string[] { "list", "prints all records", "The 'help' command prints all records." },
            new string[] { "stat", "prints count of records", "The 'stat' command prints count of records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine(Resource.GetString("developerNameMessage", CultureInfo.InvariantCulture), Program.DeveloperName);
            Console.WriteLine(Resource.GetString("hintMessage", CultureInfo.InvariantCulture));
            Console.WriteLine();

            do
            {
                Console.Write(Resource.GetString("pointer", CultureInfo.InvariantCulture));
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Resource.GetString("hintMessage", CultureInfo.InvariantCulture));
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
                    Console.Write(Resource.GetString("firstNameInputMessage", CultureInfo.InvariantCulture));
                    string firstName = Console.ReadLine();
                    Console.Write(Resource.GetString("lastNameInputMessage", CultureInfo.InvariantCulture));
                    string lastName = Console.ReadLine();
                    Console.Write(Resource.GetString("sexInputMessage", CultureInfo.InvariantCulture));
                    char sex = Convert.ToChar(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.Write(Resource.GetString("weightInputMessage", CultureInfo.InvariantCulture));
                    decimal weight = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.Write(Resource.GetString("heightInputMessage", CultureInfo.InvariantCulture));
                    short height = Convert.ToInt16(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.Write(Resource.GetString("dateOfBirthInputMessage", CultureInfo.InvariantCulture));
                    DateTime dateOfBirth = DateTime.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    int record = FileCabinetService.CreateRecord(height, weight, sex, firstName, lastName, dateOfBirth);
                    Console.WriteLine(Resource.GetString("recordCreateMessage", CultureInfo.InvariantCulture), record);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(Resource.GetString("invalidInputMessage", CultureInfo.InvariantCulture), ex.Message);
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

            int id = 0;
            try
            {
                int input = Convert.ToInt32(parameters, CultureInfo.InvariantCulture);
                id = input;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var records = FileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in records)
            {
                if (record.Id == id)
                {
                    try
                    {
                        Console.Write(Resource.GetString("firstNameInputMessage", CultureInfo.InvariantCulture));
                        string firstName = Console.ReadLine();
                        Console.Write(Resource.GetString("lastNameInputMessage", CultureInfo.InvariantCulture));
                        string lastName = Console.ReadLine();
                        Console.Write(Resource.GetString("sexInputMessage", CultureInfo.InvariantCulture));
                        char sex = Convert.ToChar(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.Write(Resource.GetString("weightInputMessage", CultureInfo.InvariantCulture));
                        decimal weight = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.Write(Resource.GetString("heightInputMessage", CultureInfo.InvariantCulture));
                        short height = Convert.ToInt16(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.Write(Resource.GetString("dateOfBirthInputMessage", CultureInfo.InvariantCulture));
                        DateTime dateOfBirth = DateTime.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        FileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, sex, height, weight);
                        Console.WriteLine(Resource.GetString("recordUpdateMessage", CultureInfo.InvariantCulture), record.Id);
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

        private static void Find(string parameters)
        {
            Tuple<string, Func<string, FileCabinetRecord[]>>[] methods = new Tuple<string, Func<string, FileCabinetRecord[]>>[]
            {
                 new Tuple<string, Func<string, FileCabinetRecord[]>>("firstname", FileCabinetService.FindByFirstName),
                 new Tuple<string, Func<string, FileCabinetRecord[]>>("lastname", FileCabinetService.FindByLastName),
                 new Tuple<string, Func<string, FileCabinetRecord[]>>("dateofbirth", FileCabinetService.FindByDateOfBirth),
            };

            var arguments = parameters.Split(' ', 2);
            var index = Array.FindIndex(methods, 0, methods.Length, i => i.Item1.Equals(arguments[0], StringComparison.InvariantCultureIgnoreCase));
            const int argumentIndex = 1;
            if (index >= 0)
            {
                var records = methods[index].Item2(arguments[argumentIndex]);
                if (records.Length == 0)
                {
                    Console.WriteLine(Resource.GetString("noRecordsMessage", CultureInfo.InvariantCulture));
                }
                else
                {
                    foreach (FileCabinetRecord record in records)
                    {
                        Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Sex}, {record.Weight}, {record.Height}, {record.DateOfBirth.ToString("yyyy-MMM-dd", new CultureInfo("us-US"))}");
                    }
                }
            }
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
                    Console.WriteLine(Resource.GetString("noExplanationMessage", CultureInfo.InvariantCulture), parameters);
                }
            }
            else
            {
                Console.WriteLine(Resource.GetString("availableMessage", CultureInfo.InvariantCulture));

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine($"\t{helpMessage[Program.CommandHelpIndex]}\t- {helpMessage[Program.DescriptionHelpIndex]}");
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine(Resource.GetString("exitMessage", CultureInfo.InvariantCulture));
            isRunning = false;
        }
    }
}