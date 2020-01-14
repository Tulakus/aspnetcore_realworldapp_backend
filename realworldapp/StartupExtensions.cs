using MediatR;
using Microsoft.Extensions.DependencyInjection;
using realworldapp.Infrastructure;

namespace realworldapp
{
    //public static IServiceCollection AddMediatR(this IServiceCollection serviceCollection ) {

    //}
    public static class StartupExtensions
    {
        public static void AddTransactionPipeline(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));
        }
    }

}