using Dapper;
using Proto;
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.Domain.Event.BeerEvent;
using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Actor.EventActor
{
    public class BeerEventActor : IActor
    {
        private readonly IDataAccessService dataAccessService;
        public BeerEventActor(IDataAccessService dataAccessService)
        {
            this.dataAccessService = dataAccessService;
            Console.WriteLine("beer event actor created");
        }

        public async Task ReceiveAsync(IContext context)
        {
            try 
            {
                switch (context.Message)
                {
                    case Started started:
                        {
                            Console.WriteLine("beer event actor started");
                            break;
                        }
                    case GetAllBeerEvent e:
                        {
                            await InvokeGetAllBeerEventAsync(context, e);
                            break;
                        }
                    case GetBeerByIdEvent e:
                        {
                            await InvokeGetBeerByIdEventAsync(context, e);
                            break;
                        }
                    case CreateBeerEvent e:
                        {
                            await InvokeCreateBeerEventAsync(context, e);
                            break;
                        }
                    case UpdateBeerByIdEvent e:
                        {
                            await InvokeUpdateBeerByIdEventAsync(context, e);
                            break;
                        }                        
                    case DeleteBeerByIdEvent e:
                        {
                            await InvokeDeleteBeerByIdEventAsync(context, e);
                            break;
                        }
                    case Stopping stopping:
                        {
                            Console.WriteLine("beer event actor stopping");
                            break;
                        }
                    case Stop stop:
                        {
                            Console.WriteLine("beer event actor stopped");
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
            }
        }

        //private methods to invoke the relavant event
        private async Task InvokeGetAllBeerEventAsync(IContext context, GetAllBeerEvent e)
        {
            var (IsSuccess, Result) = await dataAccessService.QueryAsync<Beer>("select * from beer");
            if (IsSuccess)
            {
                context.Respond(Result);
            }
        }
        private async Task InvokeGetBeerByIdEventAsync(IContext context, GetBeerByIdEvent e)
        {            
            var parameters = new DynamicParameters();
            parameters.Add("Id", e.Id);
            var (IsSuccess, Result) = await dataAccessService.QueryAsync<Beer>("select * from beer where Id = @Id", false, parameters);
            if (IsSuccess)
            {
                context.Respond(Result);
            }
        }

        private async Task InvokeCreateBeerEventAsync(IContext context, CreateBeerEvent e)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", e.Beer.Name);
            parameters.Add("Company", e.Beer.Company);
            parameters.Add("Style", e.Beer.Style);
            var (IsSuccess, RowsAffected) = await dataAccessService.ExecuteAsync("Insert into Beer (Name, Company, Style) values (@Name, @Company, @Style)", false, parameters);
            context.Respond(IsSuccess);
        }

        private async Task InvokeUpdateBeerByIdEventAsync(IContext context, UpdateBeerByIdEvent e)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", e.Id);
            parameters.Add("Name", e.Beer.Name);
            parameters.Add("Company", e.Beer.Company);
            parameters.Add("Style", e.Beer.Style);
            var (IsSuccess, RowsAffected) = await dataAccessService.ExecuteAsync("Update Beer set Name = @Name, Company = @Company, Style = @Style where Id = @Id", false, parameters);
            context.Respond((IsSuccess, RowsAffected));
        }

        private async Task InvokeDeleteBeerByIdEventAsync(IContext context, DeleteBeerByIdEvent e)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", e.Id);
            var (IsSuccess, RowsAffected) = await dataAccessService.ExecuteAsync("Delete from Beer where Id = @Id", false, parameters);
            context.Respond((IsSuccess, RowsAffected)); 
        }
    }
}
