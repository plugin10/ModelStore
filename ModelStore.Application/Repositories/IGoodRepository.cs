using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public interface IGoodRepository
    {
        Task<bool> CreateAsync(Good good);

        Task<Good?> GetByIdAsync(Guid id);

        Task<IEnumerable<Good>> GetAllAsync();

        Task<bool> UpdateGoodAsync(Good good);

        Task<bool> DeleteGoodAsync(Guid id);
    }
}
