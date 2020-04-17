using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Model.Dto
{
    public class Beer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Style { get; set; }
    }
}
