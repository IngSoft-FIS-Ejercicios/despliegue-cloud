using PharmaGo.Domain.Entities;
using PharmaGo.WebApi.Models.In;

namespace PharmaGo.WebApi.Models.Out
{
    public class DrugBasicModel : ProductModel
    {
        public string Symptom { get; set; }


        public DrugBasicModel(Drug drug)
        {
            Id = drug.Id;
            Code = drug.Code;
            Name = drug.Name;
            Symptom = drug.Symptom;
            Price = drug.Price;
            Pharmacy = new PharmacyBasicModel(drug.Pharmacy);
        }
    }
}
