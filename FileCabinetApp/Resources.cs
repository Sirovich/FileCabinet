using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace FileCabinetApp
{
    public static class Resources
    {
        static Resources()
        {
            Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);
        }

        public static ResourceManager Resource { get; private set; }
    }
}
