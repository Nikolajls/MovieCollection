using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.Repository.EntityFramework6.Commands;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.DTO.Searchfolder;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.Commands.Filesystem
{
    public  class AddNewSearchfolderCommand : CommandBase<int>
    {
        private readonly SearchFolderDto _dto;
        public AddNewSearchfolderCommand(SearchFolderDto dto) 
        {
            this._dto = dto;
        }

        protected override int OnExecuting(ILifetimeScope lifetimeScope)
        {
            var entity = new Searchfolder()
            {
                Path = _dto.Path,
                Recursive = _dto.Recursive,
                Title = _dto.Title,
                Active = true
            };
            StartChildCommand(new InsertCommand<Searchfolder, int>(entity));
            return entity.Id;
        }
    }
}
