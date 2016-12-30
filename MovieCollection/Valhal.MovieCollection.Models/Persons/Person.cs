using System.Collections;
using System.Collections.Generic;
using FoxTales.Infrastructure.DomainFramework.Generics;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Models.Persons
{
    public class Person : EntityBase<int>
    {
        public string Name { get; set; }


        public ICollection<Movie> Directed { get; set; }

    }
}
