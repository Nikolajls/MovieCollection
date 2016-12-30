using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Valhal.MovieCollection.DTO.Filesystem;
using Valhal.MovieCollection.Infrastructure.Commands.Filesystem;
using Valhal.MovieCollection.Infrastructure.Commands.Movies;
using Valhal.MovieCollection.Infrastructure.Servicebus;
using Valhal.MovieCollection.Infrastructure.Servicebus.Messages;

namespace Valhal.MovieCollection.Presentation.Service.Consumers
{
    public class MapMoviefolderConsumer : ConsumerBase<MapMovieFolderMessage>
    {
        protected override void Execute(MapMovieFolderMessage message)
        {
            var s = Stopwatch.StartNew();
            var o = new MapMovieFolderCommand(message.MovieFolderId).Execute(IsolationLevel.ReadUncommitted);
            s.Stop();
            Console.WriteLine($"{o} took {s.Elapsed}");
        }
    }
}
