using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyLar.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ArgumentConverterAttribute : Attribute
    {
        public string Name;
        public Type ArgumentType;
    }
}
