using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace realworldapp.Infrastructure
{
    public class ValidationPipeline<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationContext = new ValidationContext<TRequest>(request);
            var result = _validators.Select(v => v.Validate(validationContext)).SelectMany(e => e.Errors)
                .Where(error => error != null).ToList();

            if(result.Any())
                throw new Exception();

            return await next();
        }
    }
}
