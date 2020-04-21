using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Command.BeerCommand
{
    public class GetBeerByIdCommand
    {
        public long Id { get; }
        public GetBeerByIdCommand(long id)
        {
            Id = id;
        }
    }
}
