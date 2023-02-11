using Binned.Model;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class CodeService
    {
        private readonly MyDbContext _context;

        public CodeService(MyDbContext context)
        {
            _context = context;
        }
        public List<PromoCode> GetAll()
        {
            return _context.PromoCodes
                .OrderBy(d => d.Id)
                .ToList();
        }
        public void AddOrder(PromoCode code)
        {
            _context.PromoCodes.Add(code);
            _context.SaveChanges();
        }

        public void UpdateOrder(PromoCode code)
        {
            _context.PromoCodes.Update(code);
            _context.SaveChanges();
        }
    }
}
