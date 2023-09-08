using MediatR;

namespace PT.Common.Commands
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    { 
    }
}
