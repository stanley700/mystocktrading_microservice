using System;
using System.Threading.Tasks;

namespace Stock.API.Infrastructure
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
