using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.ActorManager
{
    public class ActorManager : IActorManager
    {
        private readonly ActorSystem _system;
        private readonly IActorFactory _actorFactory;
        public ActorManager(IActorFactory actorFactory, ActorSystem system)
        {
            _actorFactory = actorFactory;
            _system = system;
        }

        public async Task SendAsync<T>(object message) where T : IActor
        {
            await Task.Yield();
            _system.Root.Send(_actorFactory.GetActor<T>(), message);
        }

        public async Task<U> RequestAsync<T, U>(object message) 
            where T : IActor 
            where U : class
        {
            return await _system.Root.RequestAsync<U>(_actorFactory.GetActor<T>(), message);
        }

    }
}
