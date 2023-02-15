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
            var codeList = _context.PromoCodes
                .OrderBy(d => d.Id)
                .ToList();

            foreach (var code in codeList)
            {
                int result = DateTime.Compare(code.ExpiryDate, DateTime.Now);
                if (result < 0 && code.Active == true)
                {
                    code.Active = false;
                }
                _context.SaveChanges();
            }

            return codeList;
        }
        public void AddCode(PromoCode code)
        {
            _context.PromoCodes.Add(code);
            _context.SaveChanges();
        }

        public void UpdateCode(PromoCode code)
        {
            var current = _context.PromoCodes.FirstOrDefault(item => item.Id == code.Id);
            current = code;
            //_context.PromoCodes.Update(code);
            _context.SaveChanges();
        }
        public PromoCode GetCodeByName(string name)
        {
            PromoCode? code = _context.PromoCodes
                .FirstOrDefault(x => x.Name.Equals(name));
            return code;
        }
        public PromoCode GetCodeById(int id)
        {
            PromoCode? code = _context.PromoCodes
                .FirstOrDefault(x => x.Id == id);
            return code;
        }
    }
}
