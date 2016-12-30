using System.Data.Entity.ModelConfiguration;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Infrastructure.EF.Configurations.Movies
{
    public class MovieConfiguration : EntityTypeConfiguration<Movie>
    {
        public MovieConfiguration()
        {
            ToTable("Movie","Movies");
            HasMany(p => p.MovieFolders).WithOptional(o => o.Movie).HasForeignKey(m => m.MovieId);
        }
    }
}
