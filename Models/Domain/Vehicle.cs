namespace vehicle_stock_management_api.Models.Domain
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string plaka { get; set; }
        public int modelYear { get; set; }
        public DateTime? muayeneTarihi { get; set; }
        public string path { get; set; }
        public bool isActive { get; set; }
    }
}
