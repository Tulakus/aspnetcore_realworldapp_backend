using MediatR;
using Microsoft.Extensions.DependencyInjection;
using realworldapp.Infrastructure;

namespace realworldapp
{
    public static class StartupExtensions
    {
        public static void AddTransactionPipeline(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));
        }

        public static void AddValidationPipeline(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
        }
    }

}