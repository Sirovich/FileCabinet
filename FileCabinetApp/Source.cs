using System.Resources;

namespace FileCabinetApp
{
    /// <summary>
    /// Resources class.
    /// </summary>
    public static class Source
    {
        static Source()
        {
            Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);
            FieldsCount = 7;
        }

        /// <summary>
        /// Gets resource manager.
        /// </summary>
        /// <value>Resource manager.</value>
        public static ResourceManager Resource { get; private set; }

        /// <summary>
        /// Gets count of fields.
        /// </summary>
        /// <value>Fields count.</value>
        public static int FieldsCount { get; private set; }
    }
}
