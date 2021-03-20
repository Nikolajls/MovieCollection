using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nikolaj.MovieCollection.Extensions.Dapper;
using Nikolaj.MovieCollection.Models.Filesystem;
using Serilog;

namespace Nikolaj.MovieCollection.Features.Filesystem
{
	public class FilesourceGetAll
	{
		public class Query : RequestBase<List<Result>>
		{
		}



		public class Result
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Path { get; set; }
			public bool Recursive { get; set; }
			public bool Enabled { get; set; }
		}

		public class QueryHandler : IRequestHandler<Query, List<Result>>
		{
			private readonly ILogger _logger;
			private readonly IDbConnection _connection;

			public QueryHandler(IDbConnection connection, ILogger logger)
			{
				_connection = connection;
				_logger = logger;
			
			}

			public async Task<List<Result>> Handle(Query request, CancellationToken cancellationToken)
			{
				_logger.Information("Finding all filesources");
				var data = await _connection.QueryAsync<Result>(
					$@"SELECT 
								{nameof(Filesource.Id)},
								{nameof(Filesource.Name)},
								{nameof(Filesource.Path)},
								{nameof(Filesource.Recursive)},
								{nameof(Filesource.Enabled)}
								FROM {typeof(Filesource).GetTableName()}");

				var result = data.ToList();
				_logger.ForContext("FileSources", JsonConvert.SerializeObject(result)).Information("Found {Count} file sources", result.Count());
				return result;
			}
		}
	}
}
