using System;
using System.Collections.Generic;
using FoxTales.Infrastructure.DomainFramework.Generics;
using Valhal.MovieCollection.Models.FileSystems;
using Valhal.MovieCollection.Models.Persons;

namespace Valhal.MovieCollection.Models.Movies
{
    public class Movie : EntityBase<int>
    {
        public string ImdbId { get; set; }
        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string Plot { get; set; }
        public string PlotOutline { get; set; }
        public int RuntimeMinutes { get; set; }

        public string TagLine { get; set; }

        public Person Director { get; set; }

        public int? DirectorId { get; set; }
        public string TrailerKey { get; set; }
        public double Rating { get; set; }
        public DateTime? Released { get; set; }
        public int Votes { get; set; }
        public ICollection<Genre> Genres { get; set; }

        public ICollection<MovieFolder> MovieFolders { get; set; }

        public Movie()
        {
            Genres = new List<Genre>();
            MovieFolders = new List<MovieFolder>();
        }
    }
}
