using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valhal.MovieCollection.Infrastructure.Servicebus.Messages
{
   public class MapMovieFolderMessage
    {
        public int MovieFolderId { get; set; }
       public string Path { get; set; }
    }
}
