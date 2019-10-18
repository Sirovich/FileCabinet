using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using CommandLine;
using FileCabinetApp.Converters;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

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

        private static readonly ResourceManager Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);
        private static IRecordValidator recordValidator;
        private static IFileCabinetService fileCabinetService = new FileCabinetService();
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
            GetCommandLineArguments(args);
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

        private static void GetCommandLineArguments(string[] args)
        {
            if (args is null)
            {
                recordValidator = new DefaultValidator();
                return;
            }

            var opts = new Options();
            var result = Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => opts = parsed);

            if (opts.Rule.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
            {
                recordValidator = new DefaultValidator();
                Console.WriteLine(Resource.GetString("defaultRule", CultureInfo.InvariantCulture));
            }
            else if (opts.Rule.Equals("Custom", StringComparison.InvariantCultureIgnoreCase))
            {
                recordValidator = new CustomValidator();
                Console.WriteLine(Resource.GetString("customRule", CultureInfo.InvariantCulture));
            }
            else
            {
                throw new ArgumentException(Resource.GetString("invalidRule", CultureInfo.InvariantCulture));
            }
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void List(string parameters)
        {
            var list = fileCabinetService.GetRecords();
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
                    var firstName = ReadInput(Converter.StringConverter, recordValidator.ValidateFirstName);
                    Console.Write(Resource.GetString("lastNameInputMessage", CultureInfo.InvariantCulture));
                    var lastName = ReadInput(Converter.StringConverter, recordValidator.ValidateLastName);
                    Console.Write(Resource.GetString("sexInputMessage", CultureInfo.InvariantCulture));
                    var sex = ReadInput(Converter.SexConverter, recordValidator.ValidateSex);
                    Console.Write(Resource.GetString("weightInputMessage", CultureInfo.InvariantCulture));
                    var weight = ReadInput(Converter.WeightConverter, recordValidator.ValidateWeight);
                    Console.Write(Resource.GetString("heightInputMessage", CultureInfo.InvariantCulture));
                    var height = ReadInput(Converter.HeightConverter, recordValidator.ValidateHeight);
                    Console.Write(Resource.GetString("dateOfBirthInputMessage", CultureInfo.InvariantCulture));
                    DateTime dateOfBirth = ReadInput(Converter.DateOfBirthConverter, recordValidator.ValidateDateOfBirth);
                    int record = fileCabinetService.CreateRecord(height, weight, sex, firstName, lastName, dateOfBirth);
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

            var records = fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in records)
            {
                if (record.Id == id)
                {
                    try
                    {
                        Console.Write(Resource.GetString("firstNameInputMessage", CultureInfo.InvariantCulture));
                        var firstName = ReadInput(Converter.StringConverter, recordValidator.ValidateFirstName);
                        Console.Write(Resource.GetString("lastNameInputMessage", CultureInfo.InvariantCulture));
                        var lastName = ReadInput(Converter.StringConverter, recordValidator.ValidateLastName);
                        Console.Write(Resource.GetString("sexInputMessage", CultureInfo.InvariantCulture));
                        var sex = ReadInput(Converter.SexConverter, recordValidator.ValidateSex);
                        Console.Write(Resource.GetString("weightInputMessage", CultureInfo.InvariantCulture));
                        var weight = ReadInput(Converter.WeightConverter, recordValidator.ValidateWeight);
                        Console.Write(Resource.GetString("heightInputMessage", CultureInfo.InvariantCulture));
                        var height = ReadInput(Converter.HeightConverter, recordValidator.ValidateHeight);
                        Console.Write(Resource.GetString("dateOfBirthInputMessage", CultureInfo.InvariantCulture));
                        DateTime dateOfBirth = ReadInput(Converter.DateOfBirthConverter, recordValidator.ValidateDateOfBirth);
                        fileCabinetService.EditRecord(id, firstName as string, lastName as string, dateOfBirth, sex, height, weight);
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
            var methods = new Tuple<string, Func<string, ReadOnlyCollection<FileCabinetRecord>>>[]
            {
                 new Tuple<string, Func<string, ReadOnlyCollection<FileCabinetRecord>>>("firstname", fileCabinetService.FindByFirstName),
                 new Tuple<string, Func<string, ReadOnlyCollection<FileCabinetRecord>>>("lastname", fileCabinetService.FindByLastName),
                 new Tuple<string, Func<string, ReadOnlyCollection<FileCabinetRecord>>>("dateofbirth", fileCabinetService.FindByDateOfBirth),
            };

            var arguments = parameters.Split(' ', 2);
            var index = Array.FindIndex(methods, 0, methods.Length, i => i.Item1.Equals(arguments[0], StringComparison.InvariantCultureIgnoreCase));
            const int argumentIndex = 1;
            if (index >= 0)
            {
                var records = methods[index].Item2(arguments[argumentIndex]);
                if (records.Count == 0)
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
            var recordsCount = Program.fileCabinetService.GetStat();
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

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}