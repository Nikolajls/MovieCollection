using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DTOFramework;

namespace Valhal.MovieCollection.DTO
{
    public class MovieGet :IDTO

    {
        public string Title { get; set; }
    }
}
