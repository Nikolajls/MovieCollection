using System.Collections.Generic;
using Valhal.MovieCollection.Models.FileSystems;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Infrastructure.EF.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MovieCollectionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MovieCollectionContext context)
        {
         /*   context.Set<Searchfolder>().Add(new Searchfolder() {Title = "Film#1",Path = @"\\server\Raid1\", Active = true});
            context.Set<Searchfolder>().Add(new Searchfolder() {Title = "Film#2",Path = @"\\server\Raid2\", Active = true});
            context.Set<Searchfolder>().Add(new Searchfolder() {Title = "Sagas", Path = @"\\server\Sagas", Active = true});

            var genre1 = new Genre {Id = 1, Name = "Horror"};
            var genre2 = new Genre {Id = 2, Name = "Action"};
            context.Set<Genre>().Add(genre1);
            context.Set<Genre>().Add(genre2);


            var a = new Movie {Id=1,Title = "TEST1",Genres =  new List<Genre>() {genre1,genre2} };
            var a1 = new Movie { Id = 2, Title = "TEST2"};
            var a2 = new Movie { Id = 3, Title = "TEST3"};
            var a3 = new Movie { Id = 4, Title = "TEST4"};
            context.Set<Movie>().Add(a);
            context.Set<Movie>().Add(a1);
            context.Set<Movie>().Add(a2);
            context.Set<Movie>().Add(a3);


            var MovieFolder1 = new MovieFolder() {Id=1,MovieFilePath = "TEST1",MovieId = a.Id};

            var MovieFolder2 = new MovieFolder() { Id = 2, MovieFilePath = "TES21", MovieId = a1.Id };
            var MovieFolder3 = new MovieFolder() { Id = 3, MovieFilePath = "TEST3", MovieId = a2.Id };
            var MovieFolder4 = new MovieFolder() { Id = 4, MovieFilePath = "TEST4", MovieId = a3.Id };
            context.Set<MovieFolder>().Add(MovieFolder1);
            context.Set<MovieFolder>().Add(MovieFolder2);
            context.Set<MovieFolder>().Add(MovieFolder3);
            context.Set<MovieFolder>().Add(MovieFolder4);

            context.SaveChanges();*/

        }
    }
}
