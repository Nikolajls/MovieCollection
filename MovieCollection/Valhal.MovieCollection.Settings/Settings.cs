using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DependencyInjection;

namespace Valhal.MovieCollection.Settings
{
    public abstract class Settings
    {
        public virtual string WebbookingContextConnectionString
        {
   
            get { return @"Server=.;Integrated Security=True;Persist Security Info=True;Initial Catalog=MovieCollection;"; }
        }

        public static Settings Instance
        {
            get
            {
                return new DebugSettings();/*
#if CONFIGDEBUG
                return new DebugSettings();
#elif CONFIGRELEASE
                return new ReleaseSettings();
#elif CONFIGTEST
                return new TestSettings();
#elif CONFIGSTAGING
                return new StagingSettings();
#else
                return new DebugSettings();
#endif
*/
            }
        }
        public AutofacBootstrapper GetAutofacBootstrapper(ApplicationType type)
        {
            return new AutofacBootstrapper(type);
        }
    }
}