using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExamples.Dto
{
    public class Brewery
    {
        public string Name { get; set; }
        public string Country { get; set; }
        [MapTo("CEO")]
        public string Ceo { get; set; }
        [MapTo("Year_Est")]
        public int YearEstablished { get; set; }
    }
}
