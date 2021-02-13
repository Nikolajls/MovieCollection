using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikolaj.MovieCollection.Models.Filesystem
{
	[Table("Filesystem.FileSources")]
	public class Filesource
	{
		public int Id { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
		public bool Recursive { get; set; }
		public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
		public bool Enabled { get; set; }
	}
}
