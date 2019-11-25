using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base command handler class.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler commandHandler;

        /// <inheritdoc/>
        public virtual void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                return;
            }

            if (this.commandHandler != null)
            {
                this.commandHandler.Handle(commandRequest);
            }
            else
            {
                var similarCommands = FindSimilarCommands(commandRequest.Command);
                if (similarCommands.Count == 0)
                {
                    PrintMissedCommandInfo(commandRequest.Command);
                }
                else if (similarCommands.Count == 1)
                {
                    PrintMissedCommandInfo(commandRequest.Command);
                    Console.WriteLine(Source.Resource.GetString("similarCommand", CultureInfo.InvariantCulture));
                    Console.WriteLine(similarCommands[0]);
                }
                else
                {
                    PrintMissedCommandInfo(commandRequest.Command);
                    Console.WriteLine(Source.Resource.GetString("similarCommands", CultureInfo.InvariantCulture));

                    foreach (var command in similarCommands)
                    {
                        Console.WriteLine(command);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
            return this.commandHandler;
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static List<string> FindSimilarCommands(string source)
        {
            string[] commands = new string[]
            {
              "help",
              "remove",
              "edit",
              "purge",
              "find",
              "list",
              "stat",
              "export",
              "import",
              "exit",
            };
            var similarCommands = new List<string>();

            foreach (var command in commands)
            {
                if (LevensteinAlgo(source, command) < 4)
                {
                    similarCommands.Add(command);
                }
            }

            return similarCommands;

            int LevensteinAlgo(string source, string command)
            {
                int n = source.Length;
                int m = command.Length;
                int[][] matrix = new int[n + 1][];

                for (int i = 0; i < n + 1; i++)
                {
                    matrix[i] = new int[m + 1];
                }

                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                for (int i = 0; i <= n; i++)
                {
                    matrix[i][0] = i;
                }

                for (int j = 0; j <= m; j++)
                {
                    matrix[0][j] = j;
                }

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= m; j++)
                    {
                        int cost = (command[j - 1] == source[i - 1]) ? 0 : 1;

                        matrix[i][j] = Math.Min(
                            Math.Min(matrix[i - 1][j] + 1, matrix[i][j - 1] + 1),
                            matrix[i - 1][j - 1] + cost);
                    }
                }

                return matrix[n][m];
            }
        }
    }
}
