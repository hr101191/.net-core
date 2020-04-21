using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Event.BeerEvent
{
    public class GetBeerByIdEvent
    {
        public long Id { get; }
        public GetBeerByIdEvent(long id)
        {
            Id = id;
        }
    }
}
