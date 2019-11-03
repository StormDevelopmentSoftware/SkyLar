using System;

namespace SkyLar.Attributes
{
    public class CommandCategoryAttribute : Attribute
    {
        public Category Category;

        public CommandCategoryAttribute(Category category)
        {
            this.Category = category;
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
