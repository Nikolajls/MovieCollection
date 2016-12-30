using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Valhal.MovieCollection.DTO.Filesystem;
using Valhal.MovieCollection.Infrastructure.Commands.Filesystem;
using Valhal.MovieCollection.Infrastructure.Servicebus;
using Valhal.MovieCollection.Infrastructure.Servicebus.Messages;

namespace Valhal.MovieCollection.Presentation.Service.Consumers
{
    public class AddMovieConsumer : ConsumerBase<MovieFolderAddMessage>
    {
        protected override void Execute(MovieFolderAddMessage message)
        {
            var s = Stopwatch.StartNew();
            var x = new AddMovieFolderDto()
            {
             Folderpath = message.Folderpath,
                Fanartpath = message.Fanartpath,
                Posterpath = message.Posterpath,
                MovieFilePath = message.MovieFilePath,
                Subtitlepath = message.Subtitlepath,
                NfofilePath = message.NfofilePath,
                NfoContent = message.NfoContent,
                DirectoryPath = message.DirectoryPath,
            };
           new AddMovieFolderCommand(x).Execute(IsolationLevel.ReadUncommitted);
            s.Stop();
            Console.WriteLine(message.DirectoryPath + " Took time:" + s.Elapsed);
        }
    }
}
