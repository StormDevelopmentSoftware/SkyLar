// File CommandCategory.cs for the SkyLar Discord bot at 27/10/2019 21:16.
// (C) Storm Development Software - 2019. All Rights Reserved
using System;
namespace SkyLar.Attributes
{
    public class CommandCategory : Attribute
    {
        public Category Category;

        public CommandCategory(Category category)
        {
            Category = category;
        }
    }

    public enum Category
    {
        Developer,
        Info,
        Utility,
        Fun
    }
}
