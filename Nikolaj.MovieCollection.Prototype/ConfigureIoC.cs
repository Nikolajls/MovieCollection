using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nikolaj.MovieCollection.Features;
using Serilog;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Nikolaj.MovieCollection.Prototype
{
	public static class ConfigureIoC
	{
		public static IServiceProvider Configure()
		{

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.WriteTo.Seq("http://192.168.0.151:5341")
				.CreateLogger();

			Log.Information("Hello, Serilog!");
			var services = new ServiceCollection();
			services.AddMediatR(typeof(AssemblyAnchor));
			services.AddTransient<TestAddHandler>();
			services.AddTransient<IDbConnection>(provider => new SqlConnection("Server=192.168.0.151;Database=MovieCollection;User Id=test;Password=test;MultipleActiveResultSets=true;"));
			services.AddSingleton<ILogger>(c => Log.Logger);

			return services.BuildServiceProvider();
		}
	}
}
