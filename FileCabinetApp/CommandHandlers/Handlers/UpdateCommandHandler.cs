using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Update command handler.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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

            if (commandRequest.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Update(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private static bool FillFields(ref List<int> recordId, ref List<string> recordFirstName, ref List<string> recordLastName, ref List<DateTime> recordDate, ref List<char> recordSex, ref List<decimal> recordWeight, ref List<short> recordHeight, string key, string value)
        {
            if (key.Equals("id", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordId == null)
                {
                    recordId = new List<int>();
                }

                int temp;
                if (!int.TryParse(value, out temp))
                {
                    Console.WriteLine(Source.Resource.GetString("idException", CultureInfo.InvariantCulture));
                    return false;
                }

                recordId.Add(temp);
                return true;
            }

            if (key.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordFirstName == null)
                {
                    recordFirstName = new List<string>();
                }

                recordFirstName.Add(value);
                return true;
            }

            if (key.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordLastName == null)
                {
                    recordLastName = new List<string>();
                }

                recordLastName.Add(value);
                return true;
            }

            if (key.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordDate == null)
                {
                    recordDate = new List<DateTime>();
                }

                DateTime temp;
                if (!DateTime.TryParse(value, out temp))
                {
                    Console.WriteLine(Source.Resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
                    return false;
                }

                recordDate.Add(temp);
                return true;
            }

            if (key.Equals("sex", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordSex == null)
                {
                    recordSex = new List<char>();
                }

                char temp;
                if (!char.TryParse(value, out temp))
                {
                    Console.WriteLine(Source.Resource.GetString("sexException", CultureInfo.InvariantCulture));
                    return false;
                }

                recordSex.Add(temp);
                return true;
            }

            if (key.Equals("weight", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordWeight == null)
                {
                    recordWeight = new List<decimal>();
                }

                decimal temp;
                if (!decimal.TryParse(value, out temp))
                {
                    Console.WriteLine(Source.Resource.GetString("weightException", CultureInfo.InvariantCulture));
                    return false;
                }

                recordWeight.Add(temp);
                return true;
            }

            if (key.Equals("height", StringComparison.InvariantCultureIgnoreCase))
            {
                if (recordHeight == null)
                {
                    recordHeight = new List<short>();
                }

                short temp;
                if (!short.TryParse(value, out temp))
                {
                    Console.WriteLine(Source.Resource.GetString("heightException", CultureInfo.InvariantCulture));
                    return false;
                }

                recordHeight.Add(temp);
                return true;
            }

            Console.WriteLine(Source.Resource.GetString("unknownArgument", CultureInfo.InvariantCulture), key);
            return false;
        }

        private void Update(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Substring(0, 3).Equals("set", StringComparison.InvariantCulture))
            {
                parameters = parameters.Remove(0, 3);
            }
            else
            {
                Console.WriteLine();
            }

            var arguments = parameters.Split("where ", 2);

            if (arguments.Length < 2)
            {
                Console.WriteLine();
                return;
            }
            else
            {
                var fieldsToReplace = arguments[0].Split(',');
                var fieldsAndValuesToReplace = fieldsToReplace.Select(x => x.Split('=').Select(y => y.Trim('\'', ' ')));

                var valuesAnd = arguments[1].Split("and");
                var valuesOr = arguments[1].Split("or");
                string[] values;
                string type = null;

                if (valuesAnd.Length < valuesOr.Length)
                {
                    values = valuesOr;
                    type = "or";
                }
                else
                {
                    values = valuesAnd;
                    type = "and";
                }

                List<int> recordId = null;
                List<string> recordFirstName = null;
                List<string> recordLastName = null;
                List<DateTime> recordDate = null;
                List<char> recordSex = null;
                List<decimal> recordWeight = null;
                List<short> recordHeight = null;

                var valuesPairs = values.Select(x => x.Split('=').Select(y => y.Trim('\'', ' ')));

                foreach (var pair in valuesPairs)
                {
                    if (!FillFields(ref recordId, ref recordFirstName, ref recordLastName, ref recordDate, ref recordSex, ref recordWeight, ref recordHeight, pair.First(), pair.Last().Trim('\'')))
                    {
                        return;
                    }
                }

                IEnumerable<FileCabinetRecord> mustBeUpdated;
                if (type.Equals("and", StringComparison.InvariantCulture))
                {
                    mustBeUpdated = this.SelectAnd(recordId, recordFirstName, recordLastName, recordDate, recordSex, recordWeight, recordHeight);
                }
                else
                {
                    mustBeUpdated = this.SelectOr(recordId, recordFirstName, recordLastName, recordDate, recordSex, recordWeight, recordHeight);
                }

                if (mustBeUpdated is null)
                {
                    return;
                }
                else
                {
                    this.Service.Update(mustBeUpdated, fieldsAndValuesToReplace);
                }
            }
        }

        private IEnumerable<FileCabinetRecord> SelectAnd(List<int> recordId, List<string> recordFirstName, List<string> recordLastName, List<DateTime> recordDate, List<char> recordSex, List<decimal> recordWeight, List<short> recordHeight)
        {
            var mustBeDeleted = new List<FileCabinetRecord>(this.Service.GetRecords());

            if (recordId != null)
            {
                if (recordId.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => x.Id != recordId[0]);
            }

            if (recordFirstName != null)
            {
                if (recordFirstName.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => !recordFirstName.Contains(x.FirstName, StringComparer.InvariantCultureIgnoreCase));
            }

            if (recordLastName != null)
            {
                if (recordLastName.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => !recordLastName.Contains(x.LastName, StringComparer.InvariantCultureIgnoreCase));
            }

            if (recordDate != null)
            {
                if (recordDate.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => !recordDate.Contains(x.DateOfBirth));
            }

            if (recordWeight != null)
            {
                if (recordWeight.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => !recordWeight.Contains(x.Weight));
            }

            if (recordHeight != null)
            {
                if (recordHeight.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => !recordHeight.Contains(x.Height));
            }

            if (recordSex != null)
            {
                if (recordSex.Count > 1)
                {
                    return null;
                }

                mustBeDeleted.RemoveAll(x => !recordSex.Contains(x.Sex));
            }

            return mustBeDeleted;
        }

        private IEnumerable<FileCabinetRecord> SelectOr(List<int> recordId, List<string> recordFirstName, List<string> recordLastName, List<DateTime> recordDate, List<char> recordSex, List<decimal> recordWeight, List<short> recordHeight)
        {
            var sourceRecords = this.Service.GetRecords();
            var mustBeDeleted = new List<FileCabinetRecord>();

            if (recordId != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordId.Contains(x.Id)));
            }

            if (recordFirstName != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordFirstName.Contains(x.FirstName)).Where(y => !mustBeDeleted.Contains(y)));
            }

            if (recordLastName != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordLastName.Contains(x.LastName)).Where(y => !mustBeDeleted.Contains(y)));
            }

            if (recordDate != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordDate.Contains(x.DateOfBirth)).Where(y => !mustBeDeleted.Contains(y)));
            }

            if (recordWeight != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordWeight.Contains(x.Weight)).Where(y => !mustBeDeleted.Contains(y)));
            }

            if (recordHeight != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordHeight.Contains(x.Height)).Where(y => !mustBeDeleted.Contains(y)));
            }

            if (recordSex != null)
            {
                mustBeDeleted.AddRange(sourceRecords.Where(x => recordSex.Contains(x.Sex)).Where(y => !mustBeDeleted.Contains(y)));
            }

            return mustBeDeleted;
        }
    }
}
