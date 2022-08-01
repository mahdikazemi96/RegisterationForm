using RegisterationForm.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.Infrastructure.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Person> PersonRepository { get; }
        IGenericRepository<Personality> PersonalityRepository { get; }
        IGenericRepository<PersonPersonality> PersonPersonalityRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
