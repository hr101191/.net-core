using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Model.Dto
{
    public class Brewery
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        [MapTo("CEO")]
        public string Ceo { get; set; }
        [MapTo("Year_Est")]
        public int YearEstablished { get; set; }
    }
}
