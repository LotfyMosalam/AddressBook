using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AddressBook.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next(cancellationToken);

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdMs)
                _logger.LogWarning(
                    "Slow request: {RequestName} completed in {ElapsedMs}ms (threshold: {ThresholdMs}ms)",
                    requestName, stopwatch.ElapsedMilliseconds, SlowRequestThresholdMs);
            else
                _logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMs}ms",
                    requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex,
                "Request {RequestName} failed after {ElapsedMs}ms",
                requestName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
