using Dapper;
using Proto;
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.Domain.Command.Beer;
using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.CommandActor
{
    public interface IBeerCommandActor : IActor
    { 
    }
    public class BeerCommandActor : IBeerCommandActor
    {
        private readonly ActorSystem _actorSystem;
        private readonly IDataAccessService _dataAccessService;

        public BeerCommandActor(ActorSystem actorSystem, IDataAccessService dataAccessService)
        {
            _actorSystem = actorSystem;
            _dataAccessService = dataAccessService;
            Console.WriteLine("actor created");
        }
        public async Task ReceiveAsync(IContext context)
        {
            if (context.Message is GetBeerByIdCommand)
            {
                Console.WriteLine("command is correct");
            }
            switch (context.Message)
            {
                case GetBeerByIdCommand command:
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Id", command.Id);
                        var result = await _dataAccessService.QueryAsync<Beer>("select * from beer where Id = @Id", false, parameters);
                        if (result.IsSuccess)
                        {
                            TimeSpan t = new TimeSpan(1);
                            context.SetReceiveTimeout(t);
                            context.Respond(result.Result);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        //private methods to invoke the relavant event
    }
}
