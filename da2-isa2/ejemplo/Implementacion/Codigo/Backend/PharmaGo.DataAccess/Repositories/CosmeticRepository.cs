using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;

namespace PharmaGo.DataAccess.Repositories;

public class CosmeticRepository : BaseRepository<Cosmetic>
{
    
    private readonly PharmacyGoDbContext _context;
    
    public CosmeticRepository(PharmacyGoDbContext context) : base(context)
    {
        _context = context;
    }
    
    public override void InsertOne(Cosmetic cosmetic)
    {
        _context.Entry(cosmetic).State = EntityState.Added;
        _context.Set<Product>().Add(cosmetic);
    }
    
    public override IEnumerable<Cosmetic> GetAllByExpression(Expression<Func<Cosmetic, bool>> expression)
    {
        return _context.Set<Cosmetic>().Include(x => x.Pharmacy).Where(expression);
    }
    
}