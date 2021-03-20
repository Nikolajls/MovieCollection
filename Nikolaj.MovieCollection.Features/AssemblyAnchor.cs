using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Nikolaj.MovieCollection.Features
{
	public class AssemblyAnchor
	{
	}

	public class MediatorPipeline<TRequest, TResponse>
		: IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IRequestHandler<TRequest, TResponse> _inner;

		public MediatorPipeline(IRequestHandler<TRequest, TResponse> inner)
		{
			_inner = inner;
		}

		public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
