using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using FoxTales.Infrastructure.QueryFramework;
using Valhal.MovieCollection.Models.FileSystems;
using Valhal.MovieCollection.Models.Movies;
namespace Valhal.MovieCollection.Infrastructure.Queries
{
    public class MovieFolderExistsByPathQuery : QueryBase<bool>
    {
        private readonly string _path;

        public MovieFolderExistsByPathQuery(string path)
        {
            _path = path;
        }

        protected override IOrderedQueryable<bool> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var dbContext = lifetimeScope.Resolve<DbContext>();
            var ex = dbContext.Set<MovieFolder>().Any(f => f.DirectoryPath == _path);
            return new EnumerableQuery<bool>(new List<bool>() { ex });
        }
    }
}
