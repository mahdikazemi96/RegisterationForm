using RegisterationForm.DAL.DatabaseContext;
using RegisterationForm.Domain.Model;
using RegisterationForm.Infrastructure.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RegisterationFormDbContext _context;
        private IGenericRepository<Person> _PersonRepository;
        private IGenericRepository<Personality> _PersonalityRepository;
        private IGenericRepository<PersonPersonality> _PersonPersonalityRepository;
        public UnitOfWork(RegisterationFormDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<Person> PersonRepository => _PersonRepository ??= new GenericRepository<Person>(_context);
        public IGenericRepository<Personality> PersonalityRepository => _PersonalityRepository ??= new GenericRepository<Personality>(_context);
        public IGenericRepository<PersonPersonality> PersonPersonalityRepository => _PersonPersonalityRepository ??= new GenericRepository<PersonPersonality>(_context);


        public void Dispose()
        {
            _context.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
