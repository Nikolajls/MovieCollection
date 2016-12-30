using System.Data.Entity.ModelConfiguration;
using Valhal.MovieCollection.Models.Persons;

namespace Valhal.MovieCollection.Infrastructure.EF.Configurations.Persons
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            ToTable("Person", "Person");
        }
    }
}
