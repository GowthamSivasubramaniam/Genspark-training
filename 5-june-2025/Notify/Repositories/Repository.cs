using Notify.Contexts;
using Notify.Interfaces;

namespace Notify.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        public readonly NotifyContext _context;

        public Repository(NotifyContext context)
        {
            _context = context;
        }
        public virtual async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();//generate and execute the DML quries for the objects whse state is in ['added','modified','deleted'],
            return item;
        }



        public abstract Task<T> Get(K key);


        public abstract Task<IEnumerable<T>> GetAll();


        public virtual async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                var entry = _context.Entry(myItem);

                foreach (var property in entry.Properties)
                {
                    if (!property.Metadata.IsPrimaryKey())
                    {
                        var newValue = item.GetType().GetProperty(property.Metadata.Name)?.GetValue(item);
                        property.CurrentValue = newValue;
                    }
                }

                await _context.SaveChangesAsync();
                return myItem;
            }

            throw new Exception("No such item found for updation");

        }
         public virtual async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }
    }
}