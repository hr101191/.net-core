using Microsoft.Extensions.Logging;
using Proto;
using ProtoActorWebApiDemo.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.ActorManager
{
    public class ActorManager : IActorManager
    {
        private readonly ILogger<ActorManager> logger;
        private readonly ActorSystem actorSystem;
        private readonly IActorFactory actorFactory;
        private readonly ITestService testService;
        public ActorManager(ILogger<ActorManager> logger, ActorSystem actorSystem, IActorFactory actorFactory)
        {
            this.logger = logger;
            this.actorSystem = actorSystem;
            this.actorFactory = actorFactory;
            logger.LogInformation("ActorManager Instance created " + System.Threading.Thread.GetCurrentProcessorId());
        }

        public async Task SendAsync<T>(object message) where T : IActor
        {
            try 
            {
                await Task.Yield();
                actorSystem.Root.Send(actorFactory.GetActor<T>(), message); 
            }
            catch (Exception ex)
            {
                logger.LogError("", ex);
            }           
        }

        public async Task<U> RequestAsync<T, U>(object message) 
            where T : IActor 
        {
            try
            {
                //testService.Test();
                logger.LogInformation("Sending async request" + System.Threading.Thread.GetCurrentProcessorId());
                //if (!(await actorSystem.Root.RequestAsync<U>(actorFactory.GetActor<T>(), message) is U))
                //{
                //    //handle the error
                //}
                var result = await actorSystem.Root.RequestAsync<U>(actorFactory.GetActor<T>(), message);                
                //await actorSystem.Root.StopAsync(actorFactory.GetActor<T>());
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error... stacktrace: " + ex.StackTrace);
                return Activator.CreateInstance<U>();
            }
            
        }

    }
}
