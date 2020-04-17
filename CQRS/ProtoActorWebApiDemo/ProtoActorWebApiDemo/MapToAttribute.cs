using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo
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
