using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.Repository.EntityFramework6.Extensions;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Infrastructure.Commands.Movies
{
    public class AddGenreCommand : CommandBase<int>
    {
        private readonly Genre _genre;

        public AddGenreCommand(Genre m)
        {
            this._genre = m;
        }

        protected override int OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            var genreID = context.CallStoredProcedure<int>("[dbo].[AddGenre]",
                _genre.CreatedDate,
                _genre.ModifiedDate,
                _genre.Name
                ).ToList().FirstOrDefault();
            return genreID;
        }
    }
}
