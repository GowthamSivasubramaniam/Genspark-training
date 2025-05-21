using cardioManagement.Interfaces;
using cardioManagement.Exceptions; 
using System.Collections.Generic;

namespace cardioManagement.Repositories {
    public abstract class Repository<K, T> : IRepositor<K,T> where T : class
    {
        public List<T> _items = new List<T>();
        public abstract T GetItemById(K id);
        public abstract ICollection<T> GetAll();
        public abstract K GenerateID();
        public T Add(T item)
        {
            K id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }
            if (_items.Contains(item))
            {
                throw new DuplicateEntityException("Appointment already exists");
            }
            _items.Add(item);
            return item;
        }

        public T Delete(K id)
        {
            T item = GetItemById(id);
            int index = _items.IndexOf(item);
            if (index == -1)
            {
                throw new NullReferenceException("No such item Exists");

            }
            _items.RemoveAt(index);
            return item;
        }


    }
}