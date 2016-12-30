using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valhal.MovieCollection.Infrastructure.Servicebus;
using Valhal.MovieCollection.Presentation.Service.Consumers;

namespace Valhal.MovieCollection.Presentation.Service
{
    public class Servicen
    {

        private readonly IValhalBus _bus;
        private readonly AddMovieConsumer _addMovieConsumer;
        private readonly MapMoviefolderConsumer _mapMoviefolderConsumer;

        public Servicen(IValhalBus bus, AddMovieConsumer testconsumer, MapMoviefolderConsumer mapMoviefolderConsumer)
        {
            _bus = bus;
            _addMovieConsumer = testconsumer;
            _mapMoviefolderConsumer = mapMoviefolderConsumer;
        }

        public void Start()
        {
            _bus.Subscribe("AddMovieFolder", _addMovieConsumer.Process());
            _bus.Subscribe("MapMovieFolder", _mapMoviefolderConsumer.Process());
            Console.WriteLine("Started");
        }

        public void Stop()
        {
            _bus.Dispose();
            Console.WriteLine("Stopped");
        }
    }
}
