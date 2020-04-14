using System;

namespace DapperExamples
{
    public class MapToAttribute : Attribute
    {
        public string Column { get; set; }
        public MapToAttribute(string column)
        {
            Column = column;
        }
    }
}
