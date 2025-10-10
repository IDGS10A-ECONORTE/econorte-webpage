namespace Econorte.WebSite.Models
{
    public class Api
    {
        public int Id_API { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public bool IsGet { get; set; } = false;
        public bool IsPost { get; set; } = false;
        public object? BodyParams { get; set; } = new(); //Por si el parámetro es un objeto
        public string Param { get; set; } = string.Empty; //Por si el parámetro es un valor
    }
}
