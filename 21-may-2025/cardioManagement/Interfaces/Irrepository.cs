namespace cardioManagement.Interfaces
{
    public interface IRepositor<K, T> where T : class
    {
        T Add(T item);
        T Delete(K id);
        T GetItemById(K id);
        ICollection<T> GetAll();
    }
}