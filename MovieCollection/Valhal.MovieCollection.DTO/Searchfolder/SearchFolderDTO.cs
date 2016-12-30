using FoxTales.Infrastructure.DTOFramework;

namespace Valhal.MovieCollection.DTO.Searchfolder
{
    public class SearchFolderDto : ILinkedDTO<int>
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public bool Recursive { get; set; }
    }
}
