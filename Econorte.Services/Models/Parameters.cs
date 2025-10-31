namespace Econorte.Services.Models
{
    public class Parameters
    {
        public int Id_Sensor { get; set; } = 0;
        public DateTime Date { get; set; } = new();
        public double Temperature { get; set; } = new();
        public double Humidity { get; set; } = new();
        public int Gas_Level { get; set; } = 0;
        public int Vibration { get; set; } = 0;
        public bool Earthquake_Status { get; set; } = false;
        public bool Fire_Status { get; set; } = false;
    }
}