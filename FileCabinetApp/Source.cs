using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

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
        }

        /// <summary>
        /// Gets resource manager.
        /// </summary>
        /// <value>Resource manager.</value>
        public static ResourceManager Resource { get; private set; }
    }
}
