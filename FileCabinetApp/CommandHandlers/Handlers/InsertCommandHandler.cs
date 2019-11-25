using System;
using System.Globalization;
using System.Linq;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for insert.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="recordValidator">Source record validator.</param>
        /// <param name="fileCabinetService">Source service.</param>
        public InsertCommandHandler(IRecordValidator recordValidator, IFileCabinetService fileCabinetService)
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

            if (commandRequest.Command.Equals("insert", StringComparison.InvariantCultureIgnoreCase))
            {
                this.ParseArguments(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Insert(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, decimal weight, short height)
        {
            var temp = new FileCabinetRecord
            {
                Id = id,
                Sex = sex,
                Weight = weight,
                Height = height,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            if (!this.recordValidator.ValidateParameters(temp).Item1)
            {
                Console.WriteLine(this.recordValidator.ValidateParameters(temp).Item2);
                return;
            }

            this.Service.Insert(temp);
        }

        private void ParseArguments(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var arguments = parameters.Split("values");

            if (arguments.Length < 2)
            {
                Console.WriteLine(Source.Resource.GetString("invalidInsertInput", CultureInfo.InvariantCulture));
                Console.WriteLine(Source.Resource.GetString("insertFormat", CultureInfo.InvariantCulture));
                return;
            }
            else if (arguments.Length == 2)
            {
                var fields = arguments[0].Split(',', ')', '(').ToList();
                var values = arguments[1].Split(',', ')', '(').ToList();

                fields.RemoveAll(x => x.Trim(')', '(', ' ').Length == 0);
                values.RemoveAll(x => x.Trim().Length == 0);

                for (int i = 0; i < fields.Count; i++)
                {
                    fields[i] = fields[i].Trim();
                    if (!fields[i].Equals("id", StringComparison.InvariantCultureIgnoreCase)
                        && !fields[i].Equals("firstname", StringComparison.InvariantCultureIgnoreCase)
                        && !fields[i].Equals("lastname", StringComparison.InvariantCultureIgnoreCase)
                        && !fields[i].Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase)
                        && !fields[i].Equals("sex", StringComparison.InvariantCultureIgnoreCase)
                        && !fields[i].Equals("weight", StringComparison.InvariantCultureIgnoreCase)
                        && !fields[i].Equals("height", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine(Source.Resource.GetString("unknownArgument", CultureInfo.InvariantCulture), fields[i]);
                        return;
                    }
                }

                for (int i = 0; i < values.Count; i++)
                {
                    values[i] = values[i].Trim('\'', ' ');
                }

                if (values.Count == Source.FieldsCount && fields.Count == Source.FieldsCount)
                {
                    int id = 0;
                    string firstName = null;
                    string lastName = null;
                    DateTime dateOfBirth;
                    char sex;
                    decimal weight = 0;
                    short height = 0;

                    if (!int.TryParse(values[fields.FindIndex(x => x.Equals("id", StringComparison.InvariantCultureIgnoreCase))], out id))
                    {
                        Console.WriteLine(Source.Resource.GetString("idException", CultureInfo.InvariantCulture));
                        return;
                    }

                    if (fields.FindIndex(x => x.Equals("firstname", StringComparison.InvariantCultureIgnoreCase)) != -1)
                    {
                        firstName = values[fields.FindIndex(x => x.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))];
                    }
                    else
                    {
                        Console.WriteLine(Source.Resource.GetString("firstNameException", CultureInfo.InvariantCulture));
                        return;
                    }

                    if (fields.FindIndex(x => x.Equals("lastname", StringComparison.InvariantCultureIgnoreCase)) != -1)
                    {
                        lastName = values[fields.FindIndex(x => x.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))];
                    }
                    else
                    {
                        Console.WriteLine(Source.Resource.GetString("lastNameException", CultureInfo.InvariantCulture));
                        return;
                    }

                    if (!DateTime.TryParse(values[fields.FindIndex(x => x.Equals("dateOfBirth", StringComparison.InvariantCultureIgnoreCase))], out dateOfBirth))
                    {
                        Console.WriteLine(Source.Resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
                        return;
                    }

                    if (!char.TryParse(values[fields.FindIndex(x => x.Equals("sex", StringComparison.InvariantCultureIgnoreCase))], out sex))
                    {
                        Console.WriteLine(Source.Resource.GetString("sexException", CultureInfo.InvariantCulture));
                        return;
                    }

                    if (!decimal.TryParse(values[fields.FindIndex(x => x.Equals("weight", StringComparison.InvariantCultureIgnoreCase))], out weight))
                    {
                        Console.WriteLine(Source.Resource.GetString("weightException", CultureInfo.InvariantCulture));
                        return;
                    }

                    if (!short.TryParse(values[fields.FindIndex(x => x.Equals("height", StringComparison.InvariantCultureIgnoreCase))], out height))
                    {
                        Console.WriteLine(Source.Resource.GetString("heightException", CultureInfo.InvariantCulture));
                        return;
                    }

                    this.Insert(id, firstName, lastName, dateOfBirth, sex, weight, height);
                }
            }
        }
    }
}
