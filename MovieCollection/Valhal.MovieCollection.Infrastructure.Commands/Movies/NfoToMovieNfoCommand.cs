using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using Valhal.MovieCollection.DTO;

namespace Valhal.MovieCollection.Infrastructure.Commands.Movies
{
    public class NfoToMovieNfoCommand : CommandBase< MovieNfoDto>
    {
        private readonly XDocument Xml;

        public NfoToMovieNfoCommand(string input) 
        {
            Xml = XDocument.Parse(input);
        }



        protected override MovieNfoDto OnExecuting(ILifetimeScope lifetimeScope)
        {
            string tmpRead = "";
            double tmpParser = 0.0;
            int tmpIntParser = 0;
            MovieNfoDto ret = new MovieNfoDto
            {
                ImdbId = GetElementValue("id"),
                Title = GetElementValue("title"),
                OriginalTitle = GetElementValue("originaltitle"),
                TagLine = GetElementValue("tagline"),
                PlotOutline = GetElementValue("outline"),
                Plot = GetElementValue("plot")
            };
            tmpRead = GetElementValue("rating");
            if (!string.IsNullOrEmpty(tmpRead) && double.TryParse(tmpRead.Replace('.', ','), out tmpParser)) ret.Rating = tmpParser;
            tmpRead = GetElementValue("votes");
            if (!string.IsNullOrEmpty(tmpRead) && int.TryParse(tmpRead.Replace('.', ',').Replace(",", ""), out tmpIntParser)) ret.Votes = tmpIntParser;
            tmpRead = GetElementValue("releasedate");
            if (!string.IsNullOrEmpty(tmpRead)) ret.Released = GetAsDate(tmpRead);
            tmpRead = GetElementValue("runtime");
            if (!string.IsNullOrEmpty(tmpRead))
            {
                if (tmpRead.Contains(" ")) tmpRead = tmpRead.Remove(tmpRead.IndexOf(" ", StringComparison.Ordinal));
                if (int.TryParse(tmpRead, out tmpIntParser)) ret.Votes = tmpIntParser;
            }
            if (Xml.Root != null) ret.Genres = Xml.Root.Elements("genre").Select(c => c.Value).ToList();

            tmpRead = GetElementValue("trailer");
            if (!string.IsNullOrEmpty(tmpRead)) ret.TrailerKey = GetId(tmpRead);

            return ret;

        }
        private string GetElementValue(string elementname)
        {
            var elem = Xml.Root?.Element(elementname);
            return elem?.Value;
        }

        private string GetId(string get)
        {
            if (!get.Contains("videoid")) return "";
            var idAndRest = get.Substring(get.IndexOf("videoid", StringComparison.Ordinal) + 8);
            if (!idAndRest.Contains("&")) return idAndRest;
            return idAndRest.Substring(0, idAndRest.IndexOf("&", StringComparison.Ordinal));
        }



        public DateTime? GetAsDate(string datestring)
        {
            DateTime? x = null;
            if (!string.IsNullOrEmpty(datestring))
            {
                try
                {
                    x = DateTime.Parse(datestring, new CultureInfo("da-DK", false));
                }
                catch (FormatException fe)
                {
                    x = DateTime.Parse(datestring, new CultureInfo("en-US", false));

                }
            }
            return x;
        }

    }
}
