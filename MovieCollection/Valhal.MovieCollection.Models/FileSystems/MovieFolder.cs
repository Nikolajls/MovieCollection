using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DomainFramework.Generics;
using Valhal.MovieCollection.Models.FileSystems;
using Valhal.MovieCollection.Models.Movies;

namespace Valhal.MovieCollection.Models.FileSystems
{
    public class MovieFolder : EntityBase<int>
    {
        public string Folderpath { get; set; }
        public string Fanartpath { get; set; }
        public string Posterpath { get; set; }
        public string MovieFilePath { get;set; }
        public string Subtitlepath { get; set; }
        public NfoFile Nfofile { get; set; }

        public Movie Movie { get; set; }
        public int? MovieId { get; set; }
        public string DirectoryPath { get; set; }
    }
}
