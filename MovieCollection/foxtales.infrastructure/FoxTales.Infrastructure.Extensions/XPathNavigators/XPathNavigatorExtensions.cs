using System.Xml;

namespace FoxTales.Infrastructure.Extensions.XPathNavigators
{
    public static class XPathNavigatorExtensions
    {
        public static string SelectSingleValue(this XmlElement me, string path, string def)
        {
            var node = me.SelectSingleNode(path);
            return node == null ? def : node.InnerText;
        }
    }
}