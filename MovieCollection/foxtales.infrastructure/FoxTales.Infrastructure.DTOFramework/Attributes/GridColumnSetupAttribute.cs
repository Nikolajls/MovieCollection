using System;

namespace FoxTales.Infrastructure.DTOFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GridColumnSetupAttribute : Attribute
    {
        public int MinimumWindowWidth { get; set; }
        public string CSSWidth { get; set; }
        public int Order { get; set; }
        public string SortProperty { get; set; }

        public TextAlignment TextAlignment { get; set; }
    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }
}
