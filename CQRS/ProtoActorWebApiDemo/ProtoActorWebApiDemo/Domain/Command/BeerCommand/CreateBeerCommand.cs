using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.Command.BeerCommand
{
    public class CreateBeerCommand
    {
        public Beer Beer { get; }
        public CreateBeerCommand(Beer beer)
        {
            Beer = beer;
        }
    }
}
