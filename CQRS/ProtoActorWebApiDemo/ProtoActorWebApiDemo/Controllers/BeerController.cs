using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proto;
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.Domain.ActorManager;
using ProtoActorWebApiDemo.Domain.Command.Beer;
using ProtoActorWebApiDemo.Domain.CommandActor;
using ProtoActorWebApiDemo.Domain.Model.Dto;

namespace ProtoActorWebApiDemo.Controllers
{
    [Route("api/beer")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IActorManager _actorManager;
        public BeerController(IDataAccessService dataAccessService, IActorManager actorManager)
        {
            _dataAccessService = dataAccessService;
            _actorManager = actorManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBeer ()
        {
            //await _repositoryService.ExecuteNonQueryAsync("DROP TABLE IF EXISTS favorite_beers");
            //using (var connection = await _repositoryService.GetConnectionAsync()) 
            //{
            //    //await connection.OpenAsync();
            //    Console.WriteLine("Connection opened.");
            //}
            string id = Guid.NewGuid().ToString();
            int code = 200;
            var result = await _dataAccessService.QueryAsync<Beer>("select * from beer");
            List<Beer> beerList = new List<Beer>();
            if (result.IsSuccess)
            {
                beerList = result.Result;
            }
            //var message = await _context.TodoItems.ToListAsync();
            return Ok(beerList);
        }

        [Route("id/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetBeerById(long id)
        {
            try
            {
                
                var command = new GetBeerByIdCommand(id);
                //Beer beer = await _system.Root.RequestAsync<Beer>(_actorFactory.GetActor<BeerCommandActor>(), command);
                List<Beer> beerList = await _actorManager.RequestAsync<BeerCommandActor, List<Beer>>(command);
                return Ok(beerList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return Ok();
            }
            
        }
    }
}