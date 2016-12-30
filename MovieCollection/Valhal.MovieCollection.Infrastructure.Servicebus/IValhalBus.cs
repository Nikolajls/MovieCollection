using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public interface IValhalBus : IBus, IBusHelper
    {
    }
}
