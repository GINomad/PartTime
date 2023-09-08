using MediatR;

namespace PT.Common.Queries
{
    public interface IQuery
    {
    }

    public interface IQuery<out TResult> : IQuery, IRequest<TResult>
    {
    }
}
