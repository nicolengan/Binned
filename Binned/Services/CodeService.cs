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
        public void AddCode(PromoCode code)
        {
            _context.PromoCodes.Add(code);
            _context.SaveChanges();
        }

        public void UpdateCode(PromoCode code)
        {
            _context.PromoCodes.Update(code);
            _context.SaveChanges();
        }
        public PromoCode GetCodeByName(string name)
        {
            PromoCode? code = _context.PromoCodes
                .FirstOrDefault(x => x.Name.Equals(name));
            return code;
        }
    }
}
