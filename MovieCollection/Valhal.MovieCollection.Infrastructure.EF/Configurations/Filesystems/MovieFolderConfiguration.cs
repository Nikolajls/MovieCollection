using System.Data.Entity.ModelConfiguration;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.EF.Configurations.Filesystems
{
    public class MovieFolderConfiguration : EntityTypeConfiguration<MovieFolder>
    {
        public MovieFolderConfiguration()
        {
            ToTable("MovieFolder", "Filesystem");
        }
    }
}
