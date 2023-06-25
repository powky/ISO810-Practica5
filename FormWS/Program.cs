using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;

public class Program
{
    public static async Task Main(string[] args)
    {

        AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
    {
        Console.WriteLine(eventArgs.ExceptionObject.ToString());
        Environment.Exit(1);
    };

    try
        {
            string xmlContent = await FetchDataFromAPI("http://localhost:5178/api/weatherforecast");

            var doc = XDocument.Parse(xmlContent);
            var asientos = doc.Descendants("Asiento");
            //Environment.SetEnvironmentVariable("TNS_ADMIN", "/Users/fernandez/instantclient_19_8/network/admin");

            string conString = "User Id=ADMIN;Password=Iso815810unapec;Data Source=iso8xx_low;";
            
            using (OracleConnection connection = new OracleConnection(conString))
            {
                connection.Open();

                foreach (var asiento in asientos)
                {
                    var numeroAsiento = (int)asiento.Element("NumeroAsiento");
                    var descripcionAsiento = (string)asiento.Element("DescripcionAsiento");
                    var fechaAsiento = DateTime.Parse((string)asiento.Element("FechaAsiento"));
                    var codigo = (string)asiento.Element("Codigo");
                    var nombre = (string)asiento.Element("Nombre");
                    var cuenta = (string)asiento.Element("Cuenta");
                    var tipoMovimiento = (string)asiento.Element("TipoMovimiento");
                    var monto = decimal.Parse((string)asiento.Element("Monto")?.Value ?? "0");

                    string sql = $"INSERT INTO asientos_p5 (ID, DESCRIPCION, ASIENTOFECHA, CODIGO, NOMBRE, CUENTA, MOVIMIENTO, MONTO) " +
                        $"VALUES (:1, :2, :3, :4, :5, :6, :7, :8)";

                    using (OracleCommand command = new OracleCommand(sql, connection))
                    {
                        command.Parameters.Add(new OracleParameter("1", numeroAsiento));
                        command.Parameters.Add(new OracleParameter("2", descripcionAsiento));
                        command.Parameters.Add(new OracleParameter("3", fechaAsiento));
                        command.Parameters.Add(new OracleParameter("4", codigo));
                        command.Parameters.Add(new OracleParameter("5", nombre));
                        command.Parameters.Add(new OracleParameter("6", cuenta));
                        command.Parameters.Add(new OracleParameter("7", tipoMovimiento));
                        command.Parameters.Add(new OracleParameter("8", monto));

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}
    

    private static async Task<string> FetchDataFromAPI(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetStringAsync(url);
        }
    }
    
}
