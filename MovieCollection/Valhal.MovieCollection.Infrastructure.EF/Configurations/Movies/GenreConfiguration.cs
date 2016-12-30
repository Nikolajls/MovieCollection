using System.Data.Entity.ModelConfiguration;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Infrastructure.EF.Configurations.Movies
{
    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            ToTable("Genre","Movies");
           
        }
    }
}
