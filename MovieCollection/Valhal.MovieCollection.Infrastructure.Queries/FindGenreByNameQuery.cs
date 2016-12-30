using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using FoxTales.Infrastructure.QueryFramework;
using Valhal.MovieCollection.Models.Movies;
namespace Valhal.MovieCollection.Infrastructure.Queries
{
    public class FindGenreByNameQuery : QueryBase<int?>
    {
        private readonly string _genre;

        public FindGenreByNameQuery(string genre)
        {
            _genre = genre;
        }

        protected override IOrderedQueryable<int?> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var dbContext = lifetimeScope.Resolve<DbContext>();
            int? id = dbContext.Set<Genre>().Where(f => f.Name == _genre).Select(m => m.Id).FirstOrDefault();
            return new EnumerableQuery<int?>(new List<int?>() { id });
        }


    }

}
