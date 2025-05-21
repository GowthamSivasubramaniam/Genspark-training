
using cardioManagement.Models;

namespace cardioManagement.Repositories
{

    public class AppointmentRepository : Repository<int, Appointment>
    {
        public override Appointment GetItemById(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                throw new KeyNotFoundException($"No Appointment found with id:{id}");
            }
            return item;
        }

        public override ICollection<Appointment> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new NullReferenceException($"No Appointments Added");
            }
            return _items;
        }
        public override int GenerateID()
        {
            if(_items.Count == 0)
            {
                return 101;
            }
            else
            {
                return _items.Max(e => e.Id) + 1;
            }
        }
    }
}