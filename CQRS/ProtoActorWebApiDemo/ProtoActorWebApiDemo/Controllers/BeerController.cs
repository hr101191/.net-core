using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proto;
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.Domain.ActorManager;
using ProtoActorWebApiDemo.Domain.Command.BeerCommand;
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

        /// <summary>
        /// Fetch all existing Beer objects in database
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllBeer ()
        {
            try
            {
                var command = new GetAllBeerCommand();
                List<Beer> beerList = await _actorManager.RequestAsync<BeerCommandActor, List<Beer>>(command);
                return Ok(beerList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get an existing Beer object in database by Id
        /// </summary>
        [Route("id/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetBeerById(long id)
        {
            try
            {
                var command = new GetBeerByIdCommand(id);
                List<Beer> beerList = await _actorManager.RequestAsync<BeerCommandActor, List<Beer>>(command);
                return Ok(beerList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }

        }

        /// <summary>
        /// Create a new Beer object
        /// 
        /// Payload:
        /// {
	    ///     "name": "Miller Blonde",
        ///     "company": "Molson Coors Beverage Company",
        ///     "style": "Pale Lager"
        /// }
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateBeer(Beer beer)
        {
            try
            {
                var command = new CreateBeerCommand(beer);
                bool isSuccess = await _actorManager.RequestAsync<BeerCommandActor, bool>(command);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }

        }

        /// <summary>
        /// Update an existing new Beer object in database by Id
        /// 
        /// Payload:
        /// {
        ///     "name": "Miller Blonde",
        ///     "company": "Molson Coors Beverage Company",
        ///     "style": "Pale Lager"
        /// }
        /// </summary>
        [Route("id/{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateBeerById(long id, Beer beer)
        {
            try
            {
                var command = new UpdateBeerByIdCommand(id, beer);
                var (isSuccess, rowsAffected) = await _actorManager.RequestAsync<BeerCommandActor, (bool, int)>(command);
                if (isSuccess && rowsAffected > 0)
                {
                    return StatusCode(204);
                }
                else if (isSuccess && rowsAffected == 0)
                {
                    return StatusCode(404);
                }
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete an existing Beer object in database by Id
        /// </summary>
        [Route("id/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBeerById(long id)
        {
            try
            {
                var command = new DeleteBeerByIdCommand(id);
                var (isSuccess, rowsAffected) = await _actorManager.RequestAsync<BeerCommandActor, (bool, int)>(command);
                if (isSuccess && rowsAffected > 0)
                {
                    return StatusCode(204);
                }
                else if (isSuccess && rowsAffected == 0)
                {
                    return StatusCode(404);
                }
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }

        }
    }
}