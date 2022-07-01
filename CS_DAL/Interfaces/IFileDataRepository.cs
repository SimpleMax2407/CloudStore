using CS_DAL.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS_DAL.Interfaces
{
    public interface IFileDataRepository
    {
        Task<IEnumerable<FileDatum>> GetAllAsync();

        Task<FileDatum> GetByIdAsync(int id);

        Task AddAsync(FileDatum entity);

        void Delete(FileDatum entity);

        Task DeleteByIdAsync(int id);

        void Update(FileDatum entity);
    }
}
