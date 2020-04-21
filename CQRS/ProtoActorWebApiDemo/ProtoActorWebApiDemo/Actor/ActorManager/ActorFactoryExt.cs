using Microsoft.Extensions.DependencyInjection;
using Proto;
using ProtoActorWebApiDemo.Actor.EventActor;
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.Domain.CommandActor;
using System;

namespace ProtoActorWebApiDemo.Domain.ActorManager
{
    public static class ActorFactoryExt    {
        public static void RegisterAllActors(this IServiceCollection services)
        {
            //Add actor properties below if required
            services.AddProtoActor(props =>
            {
                //register actor properties (optional)

                //attached console tracing
                props.RegisterProps<BeerCommandActor>(p => p.WithReceiverMiddleware(next => async (c, env) =>
                {
                    Console.WriteLine($"enter actor {env.Message.GetType().FullName} {c.Actor} {c.Sender} {c.Sender} {env.Message}");
                    await next(c, env);
                    Console.WriteLine($"exit actor {env.Message.GetType().FullName} {c.Actor} {c.Sender} {env.Message}");
                }));
                props.RegisterProps<BeerEventActor>(p => p.WithReceiverMiddleware(next => async (c, env) =>
                {
                    Console.WriteLine($"enter actor {env.Message.GetType().FullName} {c.Sender} {env.Message}");
                    await next(c, env);
                    Console.WriteLine($"exit actor {env.Message.GetType().FullName} {c.Sender} {env.Message}");
                }));
            });

            //Actors should be injected as Transient as it should be created with each request
            services.AddTransient<IActorManager, ActorManager>();
            services.AddTransient<IActor, BeerCommandActor>();
            services.AddTransient<IActor, BeerEventActor>();
        }
    }
}
