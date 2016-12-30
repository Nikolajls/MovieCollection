using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DTOFramework;

namespace Valhal.MovieCollection.DTO
{
    public class MapMovieFolderDto : ILinkedDTO<int>
    {
        public int Id { get; set; }

    }
}
