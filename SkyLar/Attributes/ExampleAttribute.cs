using System;
namespace SkyLar.Attributes
{
    public class ExampleAttribute : Attribute
    {
        public string[] Examples;

        public ExampleAttribute(params string[] examples)
        {
            this.Examples = examples;
        }
    }
}
