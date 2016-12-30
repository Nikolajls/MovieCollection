using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DomainFramework.Generics;

namespace Valhal.MovieCollection.Models.Movies
{
    public class Genre : EntityBase<int>
    {
        public string Name { get; set; }
        public  List<Movie> Movies { get; set; }
        public Genre()
        {
            Movies = new List<Movie>();
        }
    }
}
