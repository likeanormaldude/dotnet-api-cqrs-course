using FluentValidation;
using MediatR;

namespace Restaurants.API.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(
                validators.Select(_validator => _validator.ValidateAsync(context, cancellationToken))
            );
            var errors = results.SelectMany(r => r.Errors).Where(e => e != null).ToList();

            if (errors.Any())
                throw new ValidationException(errors);
        }

        return await next();
    }
}
