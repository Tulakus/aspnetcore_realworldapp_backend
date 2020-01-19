using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using realworldapp.Models;

namespace realworldapp.Infrastructure
{
    public class TransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AppDbContext _context;
        
        public TransactionPipeline(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse result;

            try
            {
                await _context.BeginTransactionAsync();

                result = await next();

               await _context.CommitTransactionAsync();
            }
            catch (Exception)
            {
                _context.RollbackTransaction();
                throw;
            }

            return result;
        }
    }
}