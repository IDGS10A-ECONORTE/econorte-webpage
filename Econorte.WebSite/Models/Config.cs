namespace Econorte.WebSite.Models
{
    public class Config
    {
        public int IdApi { get; set; } = 0;
        public object BodyParams { get; set; } = new();
        public string Param { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}