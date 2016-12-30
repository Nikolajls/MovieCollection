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
   public class AddMovieCommand : CommandBase<int>
   {
       private readonly Movie movie;

       public AddMovieCommand(Movie m)
       {
           this.movie = m;
       }

       protected override int OnExecuting(ILifetimeScope lifetimeScope)
       {
           var context = lifetimeScope.Resolve<DbContext>();
            var movieIdFromDb = context.CallStoredProcedure<int>("[dbo].[AddMovie]",
                movie.CreatedDate,
                movie.ModifiedDate,
                movie.Title,
                movie.OriginalTitle,
                movie.ImdbId,
                movie.Votes,
                movie.TrailerKey,
                movie.Released,
                movie.Rating,
                movie.TagLine,
                movie.Plot,
                movie.PlotOutline,
                movie.RuntimeMinutes
                ).ToList().FirstOrDefault();
           return movieIdFromDb;
       }
   }
}
