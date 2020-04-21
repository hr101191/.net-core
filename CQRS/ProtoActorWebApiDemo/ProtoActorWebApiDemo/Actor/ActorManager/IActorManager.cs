using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.Domain.ActorManager
{
    public interface IActorManager
    {
        Task SendAsync<T>(object message) where T : IActor;
        Task<U> RequestAsync<T, U>(object message)
            where T : IActor;
    }
}
