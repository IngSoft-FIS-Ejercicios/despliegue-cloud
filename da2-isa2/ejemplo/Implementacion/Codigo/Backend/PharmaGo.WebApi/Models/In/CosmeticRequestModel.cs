using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.In;

public class CosmeticRequestModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Code { get; set; }
    public string PharmacyName { get; set; }
    
    public Cosmetic ToEntity()
    {
        return new Cosmetic()
        {
            Name = this.Name,
            Description = this.Description,
            Price = this.Price,
            Code = this.Code,
            Pharmacy = new Pharmacy() { Name = this.PharmacyName }
        };
    }
}