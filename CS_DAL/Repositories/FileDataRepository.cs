using CS_DAL.Data;
using CS_DAL.Entities;
using CS_DAL.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS_DAL.Repositories
{
    public class FileDataRepository : IFileDataRepository
    {
        readonly FileDataDbContext context;

        public FileDataRepository(FileDataDbContext context)
        {
            this.context = context;
        }

        public Task AddAsync(FileDatum entity)
        {
            return context.FileData.AddAsync(entity).AsTask();
        }

        public void Delete(FileDatum entity)
        {
            context.FileData.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            return Task.Run(async () => context.FileData.Remove(await GetByIdAsync(id)));
        }

        public Task<IEnumerable<FileDatum>> GetAllAsync()
        {
            return Task.Run(() => context.FileData.AsEnumerable());
        }

        public Task<FileDatum> GetByIdAsync(int id)
        {
            return Task.Run(() => context.FileData.SingleOrDefaultAsync(fd => fd.Id == id));
        }

        public void Update(FileDatum entity)
        {
            context.FileData.Update(entity);
        }
    }
}
