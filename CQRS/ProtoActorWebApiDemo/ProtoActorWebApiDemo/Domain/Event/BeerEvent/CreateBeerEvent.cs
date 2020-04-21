using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Event.BeerEvent
{
    public class CreateBeerEvent
    {
        public Beer Beer { get; }
        public CreateBeerEvent(Beer beer)
        {
            Beer = beer;
        }
    }
}
