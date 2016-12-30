using System;
using System.Data.Entity;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.DTO.Searchfolder;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.Commands.Filesystem
{
    public class ProcessSearchFolderCommand : CommandBase
    {
        private readonly SearchFolderDto _input;

        public ProcessSearchFolderCommand(SearchFolderDto input) 
        {
            _input = input;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {

            var context = lifetimeScope.Resolve<DbContext>();

            var newCommand = new FindMovieFoldersCommand(_input);
            newCommand.Execute(IsolationLevel.ReadUncommitted);

            var upd = new Searchfolder
            {
                Id = _input.Id,
                LastScan = DateTime.Now
            };
            var entry = context.Entry(context.Set<Searchfolder>().Attach(upd));
            entry.Property(p => p.LastScan).IsModified = true;
            context.SaveChanges();
        }
    }
}
