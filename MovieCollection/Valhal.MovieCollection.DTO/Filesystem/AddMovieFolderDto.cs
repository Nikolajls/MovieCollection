using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxTales.Infrastructure.DTOFramework;

namespace Valhal.MovieCollection.DTO.Filesystem
{
    public class AddMovieFolderDto : IDTO
    {
        public string Folderpath { get; set; }
        public string Fanartpath { get; set; }
        public string Posterpath { get; set; }
        public string MovieFilePath { get; set; }
        public string Subtitlepath { get; set; }
        public string NfofilePath { get; set; }
        public string NfoContent { get; set; }
        public string DirectoryPath { get; set; }
    }
}
