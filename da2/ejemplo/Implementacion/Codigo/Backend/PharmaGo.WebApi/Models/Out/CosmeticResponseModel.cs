using PharmaGo.Domain.Entities;
using PharmaGo.WebApi.Models.In;

namespace PharmaGo.WebApi.Models.Out;

public class CosmeticResponseModel : ProductModel
{
    public string Description { get; set; }
    public string? PharmacyName { get; set; }
    
    public CosmeticResponseModel(Cosmetic cosmetic)
    {
        Id = cosmetic.Id;
        Code = cosmetic.Code;
        Name = cosmetic.Name;
        Description = cosmetic.Description;
        Price = cosmetic.Price;
        PharmacyName = cosmetic.Pharmacy.Name;
    }
}
