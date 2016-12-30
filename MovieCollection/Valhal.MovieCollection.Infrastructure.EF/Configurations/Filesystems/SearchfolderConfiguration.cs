using System.Data.Entity.ModelConfiguration;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.EF.Configurations.Filesystems
{
    public class SearchfolderConfiguration : EntityTypeConfiguration<Searchfolder>
    {
        public SearchfolderConfiguration()
        {
            ToTable("Searchfolder", "Filesystem");
            Property(p => p.LastScan).IsOptional();
        }
    }
}
