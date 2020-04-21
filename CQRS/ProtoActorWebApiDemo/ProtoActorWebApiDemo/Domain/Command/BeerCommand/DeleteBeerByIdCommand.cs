using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Command.BeerCommand
{
    public class DeleteBeerByIdCommand
    {
        public long Id { get; }
        public DeleteBeerByIdCommand(long id)
        {
            Id = id;
        }
    }
}
