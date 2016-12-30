using FoxTales.Infrastructure.DomainFramework.Generics;

namespace Valhal.MovieCollection.Models.FileSystems
{
    public class NfoFile :EntityBase<int>
    {
        public string Filepath { get; set; }
        public string Content { get; set; }
    }
}