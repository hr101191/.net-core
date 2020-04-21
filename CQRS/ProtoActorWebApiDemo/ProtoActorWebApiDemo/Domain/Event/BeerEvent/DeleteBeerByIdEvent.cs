using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Event.BeerEvent
{
    public class DeleteBeerByIdEvent
    {
        public long Id { get; }
        public DeleteBeerByIdEvent(long id)
        {
            Id = id;
        }
    }
}
