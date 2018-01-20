using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMaker
{
    class Config
    {
        private static Config instance;
        private Config() { }
        public static Config GetInstance()
        {
            if (instance == null)
                instance = new Config();
            return instance;
        }
    }
}
