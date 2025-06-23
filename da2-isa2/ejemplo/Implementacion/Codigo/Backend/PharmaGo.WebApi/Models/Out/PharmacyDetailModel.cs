using PharmaGo.Domain.Entities;
using PharmaGo.WebApi.Models.In;

namespace PharmaGo.WebApi.Models.Out
{
    public class PharmacyDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<UserBasicModel> Users { get; set; }
        public ICollection<ProductModel> Products { get; set; }


        public PharmacyDetailModel(Pharmacy pharmacy)
        {
            Id = pharmacy.Id;
            Name = pharmacy.Name;
            Address = pharmacy.Address;
            Users = new List<UserBasicModel>();
            foreach (var user in pharmacy.Users)
            {
                Users.Add(new UserBasicModel(user));
            }

            Products = new List<ProductModel>();
            foreach (var product in pharmacy.Products)
            {
                if (product is Drug)
                {
                    Products.Add(new DrugBasicModel((Drug)product));
                }
                else if (product is Cosmetic)
                {
                    Products.Add(new CosmeticResponseModel((Cosmetic)product));
                }
            }
        }
    }
}

