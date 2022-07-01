using CS_DAL.Authentification;
using System.Threading.Tasks;

namespace CS_DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IFileDataRepository FileDataRepository { get; }
        
        Task SaveAsync();
    }
}
