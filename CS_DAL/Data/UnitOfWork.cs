using CS_DAL.Authentification;
using CS_DAL.Interfaces;
using CS_DAL.Repositories;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace CS_DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly FileDataDbContext fileContext;

        IFileDataRepository fileDataRepository;

        public UnitOfWork(FileDataDbContext fileContext)
        {
            this.fileContext = fileContext;
        }

        public IFileDataRepository FileDataRepository => fileDataRepository ??= new FileDataRepository(fileContext);

        public async Task SaveAsync()
        {            
            await fileContext.SaveChangesAsync();
        }
    }
}
