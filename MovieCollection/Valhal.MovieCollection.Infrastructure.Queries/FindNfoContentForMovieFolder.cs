using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using FoxTales.Infrastructure.QueryFramework;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.Models.FileSystems;
using Valhal.MovieCollection.Models.Movies;
namespace Valhal.MovieCollection.Infrastructure.Queries
{
    public class FindNfoContentForMovieFolder : QueryBase<string>
    {
        private readonly int _movieFolderId;

        public FindNfoContentForMovieFolder(int movieFolderId)
        {
            _movieFolderId = movieFolderId;
        }

        protected override IOrderedQueryable<string> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var dbContext = lifetimeScope.Resolve<DbContext>();
            var nfo = dbContext.Set<MovieFolder>().Where(f => f.Id == _movieFolderId && f.Nfofile != null).Select(m => m.Nfofile.Content).FirstOrDefault();
            var list = new List<string>() {nfo};
            return new EnumerableQuery<string>(list);
        }
    }
}
