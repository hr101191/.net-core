using Dapper;
using Proto;
using ProtoActorWebApiDemo.Actor.EventActor;
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.Domain.ActorManager;
using ProtoActorWebApiDemo.Domain.Command.BeerCommand;
using ProtoActorWebApiDemo.Domain.Event.BeerEvent;
using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.CommandActor
{
    public class BeerCommandActor : IActor
    {
        private readonly IActorManager actorManager;
        public BeerCommandActor(IActorManager actorManager)
        {
            this.actorManager = actorManager;
            Console.WriteLine("beer command actor created");
        }
        public async Task ReceiveAsync(IContext context)
        {
            try
            {
                switch (context.Message)
                {
                    case Started started:
                        {
                            Console.WriteLine("beer command actor started");
                            break;
                        }
                    case GetAllBeerCommand command:
                        {
                            await InvokeGetAllBeerCommandAsync(context, command);
                            break;
                        }
                    case GetBeerByIdCommand command:
                        {
                            await InvokeGetBeerByIdCommandAsync(context, command);
                            break;
                        }
                    case CreateBeerCommand command:
                        {
                            await InvokeCreateBeerCommandAsync(context, command);
                            break;
                        }
                    case DeleteBeerByIdCommand command:
                        {
                            await InvokeDeleteBeerByIdCommandAsync(context, command);
                            break;
                        }
                        
                    case Stopping stopping:
                        {
                            Console.WriteLine("beer command actor stopping");
                            break;
                        }
                    case Stop stop:
                        {
                            Console.WriteLine("beer command actor stopped");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message:" + ex.Message);
                Console.WriteLine("StackTrace:" + ex.StackTrace);
                Console.WriteLine("Inner Ex:" + ex.InnerException);
                context.Respond(ex);
            }

        }

        //private methods to invoke the relavant event
        private async Task InvokeGetAllBeerCommandAsync(IContext context, GetAllBeerCommand command)
        {
            var triggeredEvent = new GetAllBeerEvent();
            var result = await actorManager.RequestAsync<BeerEventActor, List<Beer>>(triggeredEvent);
            context.Respond(result);
        }
        private async Task InvokeGetBeerByIdCommandAsync(IContext context, GetBeerByIdCommand command)
        {
            var triggeredEvent = new GetBeerByIdEvent(command.Id);
            var result = await actorManager.RequestAsync<BeerEventActor, List<Beer>>(triggeredEvent);
            context.Respond(result);
        }

        private async Task InvokeCreateBeerCommandAsync(IContext context, CreateBeerCommand command)
        {
            var triggeredEvent = new CreateBeerEvent(command.Beer);
            bool isSuccess = await actorManager.RequestAsync<BeerEventActor, bool>(triggeredEvent);
            context.Respond(isSuccess);
        }

        private async Task InvokeDeleteBeerByIdCommandAsync(IContext context, DeleteBeerByIdCommand command)
        {
            var triggeredEvent = new DeleteBeerByIdEvent(command.Id);
            var (IsSuccess, RowsAffected) = await actorManager.RequestAsync<BeerEventActor, (bool, int)>(triggeredEvent);
            context.Respond((IsSuccess, RowsAffected));
        }
    }
}
