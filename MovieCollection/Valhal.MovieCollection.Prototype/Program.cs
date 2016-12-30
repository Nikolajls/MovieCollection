
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.Repository.EntityFramework6.Extensions;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.DTO.Searchfolder;
using Valhal.MovieCollection.Infrastructure.Commands.Filesystem;
using Valhal.MovieCollection.Infrastructure.Commands.Movies;
using Valhal.MovieCollection.Infrastructure.EF;
using Valhal.MovieCollection.Infrastructure.EF.Configurations.Movies;
using Valhal.MovieCollection.Infrastructure.Queries;
using Valhal.MovieCollection.Infrastructure.Servicebus;
using Valhal.MovieCollection.Infrastructure.Servicebus.Messages;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Prototype
{
    internal class Program
    {
        private static IContainer container { get; set; }
        private static void Main(string[] args)
        {
            container = SetupAutofac();

            Console.WriteLine("Press enter to start");
            Console.ReadLine();

            // SetupSearchFolders();
               SearchFolders();
            //	MatchMovies();
      


            System.Console.ReadKey();
            var bus = container.Resolve<IValhalBus>();
            bus?.Dispose();
        }

        private static void SetupSearchFolders()
        {
            var searchfolders = new FindActiveSearchFoldersQuery().Execute(IsolationLevel.ReadUncommitted).GetAll(c => c).ToList();
            Console.WriteLine("Count:" + searchfolders.Count);
            if (!searchfolders.Any(c => c.Title == "Film1"))
            {
                new AddNewSearchfolderCommand(new SearchFolderDto()
                {
                    Recursive = true,
                    Path = @"\\server\Raid1\",
                    Title = "Film1"
                }).Execute(IsolationLevel.ReadCommitted);
            }
            if (searchfolders.All(c => c.Title != "Film2"))
            {
                new AddNewSearchfolderCommand(new SearchFolderDto()
                {
                    Recursive = true,
                    Path = @"\\server\Raid2\",
                    Title = "Film2"
                }).Execute(IsolationLevel.ReadCommitted);
            }
            if (!searchfolders.Any(c => c.Title == "Sagas"))
            {
                new AddNewSearchfolderCommand(new SearchFolderDto()
                {
                    Recursive = true,
                    Path = @"\\server\Sagas\",
                    Title = "Sagas"
                }).Execute(IsolationLevel.ReadCommitted);
            }

            Console.WriteLine("Fetching new list");
            searchfolders = new FindActiveSearchFoldersQuery().Execute(IsolationLevel.ReadCommitted).GetAll(c => c).ToList();
            Console.WriteLine("Count:" + searchfolders.Count);
            foreach (var sf in searchfolders)
            {
                Console.WriteLine($"Title:{sf.Title}\tPath:{sf.Path}");
            }

        }

        private static void SendMessages()
        {
            var bus = container.Resolve<IValhalBus>();
            while (true)
            {
                var x = Console.ReadLine();
                if (x == "ABC")
                {
                    break;
                }
                else
                {
                    bus.Publish(new MapMovieFolderMessage() { Path = "", MovieFolderId = Convert.ToInt32(x) });
                    Console.WriteLine($"SENDING MESSAGE WITH ID:{x}");
                }

            }
        }



        private static void SearchFolders()
        {
            var x = new FindActiveSearchFoldersQuery();
            var searchFolders = x.Execute(IsolationLevel.ReadUncommitted).GetAll(c => c).ToList();
            foreach (var searchfolder in searchFolders)
            {
                var command = new ProcessSearchFolderCommand(searchfolder);
                command.Execute(IsolationLevel.ReadUncommitted);

            }

        }
        private static IContainer SetupAutofac()
        {
            var setup = new AutofacBootstrapper(ApplicationType.Desktop).
                ConfigureEntityFramework6<MovieCollectionContext, MovieConfiguration>().ConfigureServiceBus();

            return setup.Setup();
        }


    }
}
