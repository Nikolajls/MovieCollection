using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using FoxTales.Infrastructure.QueryFramework;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.DTO.Searchfolder;
using Valhal.MovieCollection.Models.FileSystems;
using Valhal.MovieCollection.Models.Movies;
namespace Valhal.MovieCollection.Infrastructure.Queries
{
    public class FindActiveSearchFoldersQuery : QueryBase<SearchFolderDto>
    {


        protected override IOrderedQueryable<SearchFolderDto> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var dbContext = lifetimeScope.Resolve<DbContext>();
            var id = dbContext.Set<Searchfolder>().Where(f => f.Active).Select(m => new SearchFolderDto
            {
                Id = m.Id,
                Path = m.Path,
                Recursive = m.Recursive,
                Title = m.Title
            });

            var list = new List<SearchFolderDto>();
            list.AddRange(id);
            return new EnumerableQuery<SearchFolderDto>(list);
        }
    }
}
