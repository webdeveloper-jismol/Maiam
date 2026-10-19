using System.Text;
using System.Xml;
using Maiamnew.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    public class SeoController : Controller
    {
        /// <summary>XML sitemap of every indexable page.</summary>
        [Route("sitemap.xml")]
        [ResponseCache(Duration = 3600)]
        public IActionResult Sitemap()
        {
            var settings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
            using var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                foreach (var path in SiteNavigation.IndexablePaths)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", SiteInfo.AbsoluteUrl(path));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return File(stream.ToArray(), "application/xml; charset=utf-8");
        }
    }
}
