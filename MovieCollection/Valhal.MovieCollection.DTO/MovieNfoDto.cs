using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DTOFramework;

namespace Valhal.MovieCollection.DTO
{
    public class MovieNfoDto : IDTO
    {
        public string ImdbId { get; set; }
        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string Plot { get; set; }
        public string PlotOutline { get; set; }
        public int RuntimeMinutes { get; set; }

        public string TagLine { get; set; }

        public string Director { get; set; }
        public string TrailerKey { get; set; }

        public double Rating { get; set; }
        public DateTime? Released { get; set; }
        public int Votes { get; set; }
        public ICollection<string> Genres { get; set; }


        public MovieNfoDto()
        {
            Genres = new List<string>();
        }



    }
}
