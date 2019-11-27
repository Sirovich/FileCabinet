using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CommandLine;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.Handlers;
using FileCabinetApp.Loggers;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Main program class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Ivan Sarokvashin";

        private static IRecordValidator recordValidator;
        private static IFileCabinetService fileCabinetService;
        private static bool isRunning = true;

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine(Source.Resource.GetString("developerNameMessage", CultureInfo.InvariantCulture), Program.DeveloperName);
            GetCommandLineArguments(args);
            Console.WriteLine(Source.Resource.GetString("hintMessage", CultureInfo.InvariantCulture));
            Console.WriteLine();

            var commands = CreateCommandHandler();

            while (isRunning)
            {
                Console.Write(Source.Resource.GetString("pointer", CultureInfo.InvariantCulture));
                var inputs = Console.ReadLine()?.Split(' ', 2);
                const int commandIndex = 0;
                const int argumentIndex = 1;
                var command = inputs[commandIndex];

                if (command is null || command.Trim().Length == 0)
                {
                    Console.WriteLine(Source.Resource.GetString("hintMessage", CultureInfo.InvariantCulture));
                    continue;
                }

                var parameters = inputs.Length > 1 ? inputs[argumentIndex] : string.Empty;
                commands.Handle(new AppCommandRequest(command, parameters));
            }
        }

        private static void GetCommandLineArguments(string[] args)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\validation-rules.json"))
            {
                Console.WriteLine(Source.Resource.GetString("missingJsonFile", CultureInfo.InvariantCulture));
                Environment.Exit(1488);
            }

            try
            {
                var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("validation-rules.json")
                   .Build();
            }
            catch (FormatException)
            {
                Console.WriteLine(Source.Resource.GetString("invalidJsonData", CultureInfo.InvariantCulture));
                Environment.Exit(1478);
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("validation-rules.json")
                .Build();

            if (args is null)
            {
                recordValidator = new ValidatorBuilder().CreateValidator(configuration.GetSection("default"));
                return;
            }

            var opts = new Options();
            var result = Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => opts = parsed);

            SetUpValidators(opts, configuration);
            SetUpStorage(opts);
            SetUpMeter(opts);
            SetUpLogger(opts);
        }

        private static void SetUpValidators(Options opts, IConfiguration configuration)
        {
            if (opts.Rule.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
            {
                recordValidator = new ValidatorBuilder().CreateValidator(configuration.GetSection("default"));
                Console.WriteLine(Source.Resource.GetString("defaultRule", CultureInfo.InvariantCulture));
            }
            else if (opts.Rule.Equals("Custom", StringComparison.InvariantCultureIgnoreCase))
            {
                recordValidator = new ValidatorBuilder().CreateValidator(configuration.GetSection("default"));
                Console.WriteLine(Source.Resource.GetString("customRule", CultureInfo.InvariantCulture));
            }
            else
            {
                throw new ArgumentException(Source.Resource.GetString("invalidRule", CultureInfo.InvariantCulture));
            }
        }

        private static void SetUpStorage(Options opts)
        {
            if (opts.Storage.Equals("memory", StringComparison.InvariantCultureIgnoreCase))
            {
                fileCabinetService = new FileCabinetMemoryService(recordValidator);
                Console.WriteLine(Source.Resource.GetString("memoryStorage", CultureInfo.InvariantCulture));
            }
            else if (opts.Storage.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                FileStream fileStream;
                fileStream = new FileStream(@"cabinet-records.db", FileMode.Create, FileAccess.ReadWrite);
                var service = fileCabinetService = new FileCabinetFilesystemService(fileStream, recordValidator);
                Console.WriteLine(Source.Resource.GetString("fileStorage", CultureInfo.InvariantCulture));
            }
            else
            {
                throw new ArgumentException(Source.Resource.GetString("invalidStorage", CultureInfo.InvariantCulture));
            }
        }

        private static void SetUpMeter(Options opts)
        {
            if (opts.Stopwatch)
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }
        }

        private static void SetUpLogger(Options opts)
        {
            if (opts.Log)
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
            }
        }

        private static void IsRunning(bool state)
        {
            isRunning = state;
        }

        private static ICommandHandler CreateCommandHandler()
        {
            var helpHandler = new HelpCommandHandler();
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(IsRunning);
            var insertHandler = new InsertCommandHandler(recordValidator, fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(recordValidator, fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService);
            var selectCommand = new SelectCommandHandler(fileCabinetService);

            helpHandler.SetNext(importHandler).SetNext(exportHandler)
                .SetNext(purgeHandler).SetNext(statHandler).SetNext(exitHandler)
                .SetNext(insertHandler).SetNext(deleteHandler).SetNext(updateHandler)
                .SetNext(selectCommand);

            return helpHandler;
        }
    }
}