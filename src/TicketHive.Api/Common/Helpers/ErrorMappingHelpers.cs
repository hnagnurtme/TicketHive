namespace TicketHive.Api.Common.Helpers;

using AutoMapper;
using ErrorOr;

public static class ErrorOrExtensions
{
    public static ErrorOr<TOut> MapTo<TIn, TOut>(this ErrorOr<TIn> result, IMapper mapper)
    {
        if (result.IsError)
            return result.Errors; 

        return mapper.Map<TOut>(result.Value);
    }
}