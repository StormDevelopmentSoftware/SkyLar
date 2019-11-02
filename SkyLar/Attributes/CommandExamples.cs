// File CommandExamples.cs for the SkyLar Discord bot at 11/1/2019 11:03 PM.
// (C) Storm Development Software - 2019. All Rights Reserved
using System;
namespace SkyLar.Attributes
{
    public class CommandExamples : Attribute
    {
        public string[] Examples;

        public CommandExamples(params string[] examples)
        {
            Examples = examples;
        }
    }
}
