using System;
using System.Threading.Tasks;
using MediatR;
using Nikolaj.MovieCollection.Features.Filesystem;
using Serilog;

namespace Nikolaj.MovieCollection.Prototype
{
	class Program
	{
		private static IServiceProvider _container;
		static async Task Main(string[] args)
		{
			_container = ConfigureIoC.Configure();

			var testClass = (TestAddHandler)_container.GetService(typeof(TestAddHandler));
			await testClass.GetFilesources();
			Console.WriteLine("Prototype idling.");

		
			Log.CloseAndFlush();
		}


	}

	public class TestAddHandler
	{
		private readonly IMediator _mediator;

		public TestAddHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task GetFilesources()
		{
			var result = await _mediator.Send(new FilesourceOverview.Query());
			foreach (var filesource in result)
			{
				Console.WriteLine($"Id:{filesource.Id}\tPath:{filesource.Path}\tRecursive:{filesource.Recursive}\tEnabled:{filesource.Enabled}");
			}
		}

		public async Task AddFileSource()
		{
			await _mediator.Send(new AddFilesource.Command()
			{
				Name = "Nikolaj"
			});
		}
	}
}
