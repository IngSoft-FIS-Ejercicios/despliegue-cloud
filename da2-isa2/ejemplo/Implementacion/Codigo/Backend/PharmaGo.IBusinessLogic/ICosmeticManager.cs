using PharmaGo.Domain.Entities;

namespace PharmaGo.IBusinessLogic;

public interface ICosmeticManager
{
    Cosmetic Create(Cosmetic cosmetic, string token);
    
    IEnumerable<Cosmetic> GetAll(int pharmacyId);
}