using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Reflection;
using FoxTales.Infrastructure.Repository.EntityFramework6;
using Valhal.MovieCollection.Settings;

namespace Valhal.MovieCollection.Infrastructure.EF
{
    public class MovieCollectionContext : EF6ContextBase
    {
        public MovieCollectionContext() : base(Assembly.GetExecutingAssembly(), Settings.Settings.Instance.WebbookingContextConnectionString)
        {

            DbConfiguration.SetConfiguration(new WebbookingContextConfiguration());
        }
    }
    public class WebbookingContextConfiguration : DbConfiguration
    {
        public WebbookingContextConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlConnectionFactory(Settings.Settings.Instance.WebbookingContextConnectionString));
            SetExecutionStrategy("System.Data.SqlClient", () => new DefaultExecutionStrategy());
        }
    }
}

