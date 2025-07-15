using VSM.Models;
using VSM.Interfaces;

namespace VSM.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Guid, ServiceCategory> _repo;

        public CategoryService(IRepository<Guid, ServiceCategory> repo)
        {
            _repo = repo;
        }

        public async Task<ServiceCategory> AddCategory(string name, float amt)
        {

            var existingCategories = await _repo.GetAll(1, 100); 
            var existing = existingCategories.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            if (existing != null)
            {
                throw new Exception("Category with the same name already exists.");
            }
            var category = new ServiceCategory { Name = name, Amount = amt ,Status="Active"};
            var added = await _repo.Add(category) ?? throw new Exception("Unable to add category");
            return added;
        }
        public async Task<IEnumerable<ServiceCategory>> GetAllCategory()
        {
            var category = await _repo.GetAll(1, 100);
            if (category.Count() == 0)
                throw new Exception("Category not found");
            return category;
        }
        public async Task<ServiceCategory> UpdateCategory(Guid id, float amt)
        {
            var oldcategory = await _repo.Get(id) ?? throw new Exception("Category Not Found");
            oldcategory.Amount = amt;
            var added = await _repo.Update(id, oldcategory) ?? throw new Exception("Unable to update category");
            return added;
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            var all = await _repo.GetAll(1, 100);
            var category = all.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
                throw new Exception("Category not found");
            if (category.Status == "Active")
            {
                category.Status = "InActive";
                var cat = await _repo.Update(id, category);
                if (cat == null)
                    throw new Exception("Category cannot be deactivated");
            }
            else
            {
                category.Status = "Active";
            var cat = await _repo.Update(id, category);
            if (cat == null)
                throw new Exception("Category cannot be activated");
        }
           
            return true;
        }
    }
}