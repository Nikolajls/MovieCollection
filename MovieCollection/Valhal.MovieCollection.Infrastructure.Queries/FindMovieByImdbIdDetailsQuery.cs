using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using FoxTales.Infrastructure.QueryFramework;
using Valhal.MovieCollection.Models.Movies;
namespace Valhal.MovieCollection.Infrastructure.Queries
{
    public class FindMovieByImdbIdDetailsQuery : QueryBase<int?>
    {
        private readonly string _imdbId;

        public FindMovieByImdbIdDetailsQuery(string imdbId)
        {
            _imdbId = imdbId;
        }

        protected override IOrderedQueryable<int?> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var dbContext = lifetimeScope.Resolve<DbContext>();
            int? id = dbContext.Set<Movie>().Where(f => f.ImdbId == _imdbId).Select(m => m.Id).FirstOrDefault();
            return new EnumerableQuery<int?>(new List<int?>() { id });
        }


    }

}
