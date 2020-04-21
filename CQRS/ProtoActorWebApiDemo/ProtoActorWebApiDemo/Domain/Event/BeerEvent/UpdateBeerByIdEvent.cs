using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Event.BeerEvent
{
    public class UpdateBeerByIdEvent
    {
        public long Id { get; }
        public Beer Beer { get; }
        public UpdateBeerByIdEvent(long id, Beer beer)
        {
            Id = id;
            Beer = beer;
        }
    }
}
