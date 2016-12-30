using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.Repository.EntityFramework6.Commands;
using Valhal.MovieCollection.DTO.Filesystem;
using Valhal.MovieCollection.Infrastructure.Servicebus;
using Valhal.MovieCollection.Infrastructure.Servicebus.Messages;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.Commands.Filesystem
{
    public class AddMovieFolderCommand : CommandBase<int>
    {
        private readonly AddMovieFolderDto dto;
        public AddMovieFolderCommand(AddMovieFolderDto _dto) 
        {
            dto = _dto;
        }
        protected override int OnExecuting(ILifetimeScope lifetimeScope)
        {
            var b = new MovieFolder()
            {
                DirectoryPath = dto.DirectoryPath,
                Fanartpath = dto.Fanartpath,
                Posterpath = dto.Posterpath,
                Subtitlepath = dto.Subtitlepath,
                MovieFilePath = dto.MovieFilePath
            };
            if (!string.IsNullOrEmpty(dto.NfofilePath))
            {
                b.Nfofile = new NfoFile() {Content = dto.NfoContent, Filepath = dto.NfofilePath};
            }
        

            StartChildCommand(new InsertCommand<MovieFolder, int>(b));
            var bus = lifetimeScope.Resolve<IValhalBus>();
            bus.Publish(new MapMovieFolderMessage() { MovieFolderId = b.Id,Path = b.DirectoryPath});
            return b.Id;

        }
    }
}
