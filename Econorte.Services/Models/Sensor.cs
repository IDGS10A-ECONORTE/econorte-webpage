namespace Econorte.Services.Models
{
    public class Sensor
    {
        public int Id_Sensor { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public Parameters LastParameters { get; set; } = new();
        public List<Parameters> LogParameters { get; set; } = new();
    }
}