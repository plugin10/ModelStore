using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public class GoodRepository : IGoodRepository
    {
        private readonly List<Good> _goods = new List<Good>();

        public Task<bool> CreateAsync(Good good)
        {
            _goods.Add(good);
            return Task.FromResult(true);

        }

        public Task<Good?> GetByIdAsync(Guid id)
        {
            var good = _goods.SingleOrDefault(g => g.Id == id);
            return Task.FromResult(good);
        }

        public Task<Good?> GetBySlugAsync(string slug)
        {
            var good = _goods.SingleOrDefault(g => g.Slug == slug);
            return Task.FromResult(good);
        }

        public Task<IEnumerable<Good>> GetAllAsync()
        {
            return Task.FromResult(_goods.AsEnumerable());
        }

        public Task<bool> UpdateGoodAsync(Good good)
        {
            var goodIndex = _goods.FindIndex(g => g.Id == good.Id);
            if (goodIndex == -1)
            {
                return Task.FromResult(false);
            }

            _goods[goodIndex] = good;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteGoodAsync(Guid id)
        {
            var removedCount = _goods.RemoveAll(g => g.Id == id);
            var goodRemoved = removedCount > 0;
            return Task.FromResult(goodRemoved);
        }
    }
}
