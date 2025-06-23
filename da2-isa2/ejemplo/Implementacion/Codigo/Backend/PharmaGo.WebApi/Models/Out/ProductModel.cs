using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Models.In;

public abstract class ProductModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public PharmacyBasicModel? Pharmacy { get; set; }
}