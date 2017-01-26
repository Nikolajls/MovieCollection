using System.Data.Entity;
using System.Linq;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.Repository.EntityFramework6.Extensions;
using Valhal.MovieCollection.Infrastructure.Queries;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Infrastructure.Commands.Movies
{


    public class MapMovieFolderCommand : CommandBase<string>
    {
        private readonly int _input;
        public MapMovieFolderCommand(int input)
        {
            _input = input;
        }

        protected override string OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            int i = 1;
            var nfoContent = new FindNfoContentForMovieFolder(_input).Execute(IsolationLevel.ReadUncommitted).GetAll(c => c).FirstOrDefault();

            int ix = 2;

            var movieNfoDto = new NfoToMovieNfoCommand(nfoContent).Execute(IsolationLevel.ReadUncommitted);
            var possibleMovieId = new FindMovieByImdbIdDetailsQuery(movieNfoDto.ImdbId).Execute(IsolationLevel.ReadUncommitted).GetAll(c => c).FirstOrDefault();
            int movieId;
            string ret = string.Empty;
            if (possibleMovieId.HasValue && possibleMovieId > 0)
            {
                movieId = possibleMovieId.Value;
                ret = $"MovieFolder {_input}\tmapped to:{possibleMovieId}\tImdb{movieNfoDto.ImdbId}";
            }
            else
            {
                var movie = new Movie()
                {
                    TagLine = movieNfoDto.TagLine,
                    ImdbId = movieNfoDto.ImdbId,
                    OriginalTitle = movieNfoDto.OriginalTitle,
                    Title = movieNfoDto.Title,
                    Plot = movieNfoDto.Plot,
                    PlotOutline = movieNfoDto.PlotOutline,
                    Rating = movieNfoDto.Rating,
                    Released = movieNfoDto.Released,
                    Votes = movieNfoDto.Votes,
                    RuntimeMinutes = movieNfoDto.RuntimeMinutes,
                    TrailerKey = movieNfoDto.TrailerKey
                };
                movieId = new AddMovieCommand(movie).Execute(IsolationLevel.ReadUncommitted);
                ret = $"MovieFolder {_input}\tADDED:{movieId}\tImdb{movieNfoDto.ImdbId}";
                new AssignGenresCommand(movieNfoDto, movieId).Execute(IsolationLevel.ReadUncommitted);
            }
            ret = ret + "\n" + $"AssingMovieToMovieFolder({_input},{movieId})";

            context.CallStoredProcedure("[dbo].[AssignMovieToMovieFolder]",
                _input,
                movieId
                );

          

            return ret;
        }

    }
}
