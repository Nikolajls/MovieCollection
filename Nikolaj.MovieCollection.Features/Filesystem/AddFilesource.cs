using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Nikolaj.MovieCollection.Features.Filesystem
{
	public class AddFilesource
	{
		public class Command : IRequest
		{
			public string Name { get; set; }
			public bool Recursive { get; set; } = true;

		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			private readonly IDbConnection _connection;

			public CommandHandler(IDbConnection connection)
			{
				_connection = connection;
			}


			protected override Task Handle(Command request, CancellationToken cancellationToken)
			{
				Console.WriteLine($"Hello Mr.{request.Name}");
				return Task.CompletedTask;
			}
		}
	}
}
