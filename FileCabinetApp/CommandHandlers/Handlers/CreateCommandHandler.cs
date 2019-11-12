using System;
using System.Globalization;
using FileCabinetApp.Converters;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for create command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="recordValidator">Source record validator.</param>
        /// <param name="fileCabinetService">Source service.</param>
        public CreateCommandHandler(IRecordValidator recordValidator, IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
            this.recordValidator = recordValidator;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command.Equals("create", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Create();
            }
            else
            {
                base.Handle(commandRequest);
            }
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

        private void Create()
        {
            Console.Write(Source.Resource.GetString("firstNameInputMessage", CultureInfo.InvariantCulture));
            var firstName = ReadInput(Converter.StringConverter, this.recordValidator.ValidateFirstName);
            Console.Write(Source.Resource.GetString("lastNameInputMessage", CultureInfo.InvariantCulture));
            var lastName = ReadInput(Converter.StringConverter, this.recordValidator.ValidateLastName);
            Console.Write(Source.Resource.GetString("sexInputMessage", CultureInfo.InvariantCulture));
            var sex = ReadInput(Converter.SexConverter, this.recordValidator.ValidateSex);
            Console.Write(Source.Resource.GetString("weightInputMessage", CultureInfo.InvariantCulture));
            var weight = ReadInput(Converter.WeightConverter, this.recordValidator.ValidateWeight);
            Console.Write(Source.Resource.GetString("heightInputMessage", CultureInfo.InvariantCulture));
            var height = ReadInput(Converter.HeightConverter, this.recordValidator.ValidateHeight);
            Console.Write(Source.Resource.GetString("dateOfBirthInputMessage", CultureInfo.InvariantCulture));
            DateTime dateOfBirth = ReadInput(Converter.DateOfBirthConverter, this.recordValidator.ValidateDateOfBirth);
            int record = this.Service.CreateRecord(height, weight, sex, firstName, lastName, dateOfBirth);
            Console.WriteLine(Source.Resource.GetString("recordCreateMessage", CultureInfo.InvariantCulture), record);
        }
    }
}
