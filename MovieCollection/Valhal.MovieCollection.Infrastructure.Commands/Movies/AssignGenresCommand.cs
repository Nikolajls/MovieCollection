using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.Repository.EntityFramework6.Commands;
using FoxTales.Infrastructure.Repository.EntityFramework6.Extensions;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Infrastructure.Commands.Movies
{
    public class AssignGenresCommand : CommandBase
    {
        private readonly MovieNfoDto _dto;
        private readonly int _movieId;
        private static Dictionary<string, int> GenreKeys { get; set; } = new Dictionary<string, int>();

        public AssignGenresCommand(MovieNfoDto dto,int movieId)
        {
            _dto = dto;
            _movieId = movieId;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            var genresToAdd = new List<int>();
            foreach (var genre in _dto.Genres)
            {
                if (GenreKeys.ContainsKey(genre))
                {
                    genresToAdd.Add(GenreKeys[genre]);
                }
                else
                {
                    var id = new AddGenreCommand(new Genre() { Name = genre }).Execute(IsolationLevel.ReadUncommitted);
                    GenreKeys.Add(genre, id);
                    genresToAdd.Add(id);
                }
            }

            foreach(var id in genresToAdd)
            {
                context.CallStoredProcedure("[dbo].[AssignGenreMovie]",
                id,
                _movieId
                );
            }
            
        }
    }
}
