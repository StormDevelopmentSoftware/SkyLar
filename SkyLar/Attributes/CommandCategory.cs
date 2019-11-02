// File CommandCategory.cs created by Animadoria (me@animadoria.cf) at 27/10/2019 21:16.
// (C) Animadoria 2019 - All Rights Reserved
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
