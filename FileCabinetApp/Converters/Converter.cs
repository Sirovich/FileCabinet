using System;

namespace FileCabinetApp.Converters
{
    /// <summary>
    /// Provides methods to converte input from console.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Converte string to string.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>True if posible to converte and result string.</returns>
        public static Tuple<bool, string, string> StringConverter(string source)
        {
            return new Tuple<bool, string, string>(true, source, source);
        }

        /// <summary>
        /// Converte string to DateTime.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>True if posible to converte and result date.</returns>
        public static Tuple<bool, string, DateTime> DateOfBirthConverter(string source)
        {
            DateTime result;
            if (DateTime.TryParse(source, out result))
            {
                return new Tuple<bool, string, DateTime>(true, source, result);
            }

            return new Tuple<bool, string, DateTime>(false, source, result);
        }

        /// <summary>
        /// Converte string to char.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>True if posible to converte and result char.</returns>
        public static Tuple<bool, string, char> SexConverter(string source)
        {
            char result;
            if (char.TryParse(source, out result))
            {
                return new Tuple<bool, string, char>(true, source, result);
            }

            return new Tuple<bool, string, char>(false, source, result);
        }

        /// <summary>
        /// Converte string to decimal.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>True if posible to converte and result decimal.</returns>
        public static Tuple<bool, string, decimal> WeightConverter(string source)
        {
            decimal result;
            if (decimal.TryParse(source, out result))
            {
                return new Tuple<bool, string, decimal>(true, source, result);
            }

            return new Tuple<bool, string, decimal>(false, source, result);
        }

        /// <summary>
        /// Converte string to short.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>True if posible to converte and result short.</returns>
        public static Tuple<bool, string, short> HeightConverter(string source)
        {
            short result;
            if (short.TryParse(source, out result))
            {
                return new Tuple<bool, string, short>(true, source, result);
            }

            return new Tuple<bool, string, short>(false, source, result);
        }
    }
}
