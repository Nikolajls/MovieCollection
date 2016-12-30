using System.Data.Entity.ModelConfiguration;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.EF.Configurations.Filesystems
{
    public class NfoFileConfiguration : EntityTypeConfiguration<NfoFile>

    {
        public NfoFileConfiguration()
        {
            ToTable("NfoFile", "Filesystem");

        }
    }
}