using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Command.BeerCommand
{
    public class UpdateBeerByIdCommand
    {
        public long Id { get; }
        public Beer Beer { get; }
        public UpdateBeerByIdCommand(long id, Beer beer)
        {
            Id = id;
            Beer = beer;
        }
    }
}
