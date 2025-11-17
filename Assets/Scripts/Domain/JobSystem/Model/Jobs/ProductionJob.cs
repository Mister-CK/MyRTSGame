namespace Domain.Model
{
    public class ProductionJob
    {
        public Resource[] Input { get; set; }
        public Resource Output { get; set; }
        public int Quantity { get; set; }
    }
}