namespace PharmaGo.Domain.Entities;

public abstract class Product
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Pharmacy? Pharmacy { get; set; }
}