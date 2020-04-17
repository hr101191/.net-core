using Microsoft.Extensions.DependencyInjection;
using Proto;
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
                    Console.WriteLine($"enter diactor1 {env.Message.GetType().FullName}");
                    await next(c, env);
                    Console.WriteLine($"exit diactor1 {env.Message.GetType().FullName}");
                }));
                //props.RegisterProps<DIActor2>(p => p.WithReceiverMiddleware(next => async (c, env) =>
                //{
                //    Console.WriteLine($"enter diactor2 {env.Message.GetType().FullName}");
                //    await next(c, env);
                //    Console.WriteLine($"exit diactor2 {env.Message.GetType().FullName}");
                //}));
            });

            //services.AddTransient<IActorManager, ActorManager>();
            services.AddSingleton<IActorManager, ActorManager>();

            //Actors should be injected as Transient as it should be created with each request
            services.AddTransient<IBeerCommandActor, BeerCommandActor>();
            //services.AddTransient<IDIActor2, DIActor2>();
        }
    }
}
