using Microsoft.EntityFrameworkCore.Storage;
using VSM.Contexts;
using VSM.Interfaces;

namespace VSM.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly VSMContext _context;

        public Repository(VSMContext context)
        {
            _context = context;
        }


        public async Task<T> Add(T item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;


        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item == null)
                throw new Exception("Item Not Found");
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public abstract Task<T> Get(K key);


        public abstract Task<IEnumerable<T>> GetAll(int pageNumber, int pageSize);


        public async Task<T> Update(K key, T item)
        {
            var olditem = await Get(key);
            if (olditem == null)
                throw new Exception("Item Not Found");
            var entry = _context.Entry(olditem);
            foreach (var property in entry.Properties)
            {
                if (!property.Metadata.IsPrimaryKey())
                {
                    var newValue = item.GetType().GetProperty(property.Metadata.Name)?.GetValue(item);
                    property.CurrentValue = newValue;
                }
            }

            await _context.SaveChangesAsync();
            return olditem;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

    }
}