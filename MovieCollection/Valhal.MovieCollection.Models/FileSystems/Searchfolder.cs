using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DomainFramework.Generics;

namespace Valhal.MovieCollection.Models.FileSystems
{
    public class Searchfolder : EntityBase<int>
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public bool Recursive { get; set; }
        public DateTime? LastScan { get; set; }
    }
}
