using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Ferreteria.Controllers
{
    public class AsientoDB
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("siting")]
        public int? NumeroAsiento { get; set; }

        [BsonElement("desc")]
        public string? DescripcionAsiento { get; set; }

        [BsonElement("date")]
        public DateTime FechaAsiento { get; set; }

        [BsonElement("code")]
        public string? Codigo { get; set; }

        [BsonElement("name")]
        public string? Nombre { get; set; }

        [BsonElement("acc")]
        public string? Cuenta { get; set; }

        [BsonElement("movement")]
        public string? TipoMovimiento { get; set; }

        [BsonElement("amount")]
        public decimal Monto { get; set; }
    }

    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IMongoCollection<AsientoDB> _asientosCollection;
        private MongoClient _client;
        private IMongoDatabase _database;

        public WeatherForecastController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("unapec");
            _asientosCollection = _database.GetCollection<AsientoDB>("asientos");
        }
        private async Task<List<AsientoDB>> GetAsientosAsync()
        {
            try 
            {
                var asientos = await _asientosCollection.Find(_ => true).ToListAsync();
                return asientos;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e); 
                throw;
            }

        }

        [HttpGet]
        private string GenerateXml(List<AsientoDB> asientos)
        {
            try
            {
                using (var stringWriter = new System.IO.StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(stringWriter))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("AsientoActivos");

                        foreach (var asiento in asientos)
                        {
                            writer.WriteStartElement("Asiento");

                            writer.WriteElementString("NumeroAsiento", asiento.NumeroAsiento?.ToString());
                            writer.WriteElementString("DescripcionAsiento", asiento.DescripcionAsiento);
                            writer.WriteElementString("FechaAsiento", asiento.FechaAsiento.ToString("yyyy-MM-dd"));
                            writer.WriteElementString("Codigo", asiento.Codigo);
                            writer.WriteElementString("Nombre", asiento.Nombre);
                            writer.WriteElementString("Cuenta", asiento.Cuenta);
                            writer.WriteElementString("TipoMovimiento", asiento.TipoMovimiento);
                            writer.WriteElementString("Monto", asiento.Monto.ToString("F2"));

                            writer.WriteEndElement(); 
                        }

                        writer.WriteEndElement(); 
                        writer.WriteEndDocument();
                    }
                    var xmlString = stringWriter.ToString();
                    System.IO.File.WriteAllText("debug.xml", xmlString);

                    return xmlString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GenerateXml: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                return string.Empty;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetXmlData()
        {
            var asientos = await _asientosCollection.Find(_ => true).ToListAsync();

            if (asientos.Count == 0)
            {
                return NotFound("No hay asientos disponibles.");
            }

            var xmlData = GenerateXml(asientos);

            Console.WriteLine(xmlData); 

            return Content(xmlData, "application/xml");
        }

    }
}
