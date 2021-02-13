using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikolaj.MovieCollection.Extensions.Dapper
{
	public static class DapperExtensions
	{
		// Taken from https://github.com/StackExchange/Dapper/blob/d45b79ff9d376e1b650e0a2fe48a89801c493ca7/Dapper.Contrib/SqlMapperExtensions.cs#L265
		private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName =
			new ConcurrentDictionary<RuntimeTypeHandle, string>();

		public static string GetTableName(this Type type)
		{
			if (TypeTableName.TryGetValue(type.TypeHandle, out string name)) return name;


			//NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
			var tableAttr = type
				.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
			if (tableAttr != null)
			{
				name = tableAttr.Name;
			}

			TypeTableName[type.TypeHandle] = name;
			return name;
		}
	}
}
