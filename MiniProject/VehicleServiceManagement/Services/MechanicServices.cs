using Microsoft.EntityFrameworkCore;
using Npgsql;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc.Mappers;
using VSM.Models;

namespace VSM.Services
{
    public class MechanicServices : IMechanicService
    {
        readonly IRepository<Guid, Mechanic> _repo;
        readonly IRepository<string, User> _urepo;
        readonly ITokenService _tokenService;
        readonly IEncryptionService _encryptionService;
        readonly MechanicMapper mapper;

        readonly ILogger<MechanicServices> _logger;
        public MechanicServices(IRepository<Guid, Mechanic> repo, IRepository<string, User> urepo, ITokenService tokenService, IEncryptionService encryptionService, ILogger<MechanicServices> logger)
        {
            _repo = repo;
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _logger = logger;
            mapper = new MechanicMapper();
            _urepo = urepo;
        }
        public async Task<MechanicDisplayDto> AddMechanic(MechanicAddDto dto)
        {

            var transaction = _repo.BeginTransaction();
            try
            {
                var mechanic = mapper.MapMechanic(dto);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = dto.Password
                });
                var user = new User
                {

                    Email = dto.Email,
                    Role = "Mechanic",
                    IsActive = true,
                    Password = encryptedData.EncryptedData,
                    HashKey = encryptedData.HashKey
                };
                
                var existinguser = await _urepo.Get(dto.Email);

                if (existinguser != null)
                {
                   throw new Exception("User already exists");
                }

                var response = await _urepo.Add(user) ?? throw new Exception("Cannot Add The user");
                var AddedMechaic = await _repo.Add(mechanic) ?? throw new Exception("Cannot Add The Mechanic");
                var mech =  mapper.MapMechanicToDisplayDto(AddedMechaic);
                await transaction.CommitAsync();
                return mech;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeleteMechanic(string email)
        {
            var mechanics = await _repo.GetAll(1,100);
            var mechanic = mechanics.FirstOrDefault(m => m.Email == email && m.Status != "Deleted");

            if (mechanic == null)
            {
                throw new Exception($"DeleteMechanic: Mechanic with email '{email}' not found.");
            }

            var user = await _urepo.Get(email);
            if (user == null)
            {
                throw new Exception($"DeleteMechanic: User with email '{email}' not found.");
            }

          
            user.IsActive = false;
            mechanic.Status = "Deleted";

            await _urepo.Update(email, user);
            await _repo.Update(mechanic.MechanicId, mechanic);

            return true;
        }

        public async Task<MechanicDisplayDto?> GetByEmail(string email)
        {
            var mechanics = await _repo.GetAll(1,100);
            var mechanic = mechanics.FirstOrDefault(m => m.Email == email && m.Status != "Deleted");
            if (mechanic == null)
            {
                throw new Exception($"GetByEmail: Mechanic with email '{email}' not found.");
            }
             var mech =  mapper.MapMechanicToDisplayDto(mechanic);
            return mech;
        }

        public async Task<IEnumerable<MechanicDisplayDto>> GetByName(string name)
        {
            var mechanics = await _repo.GetAll(1,100);
            var filtered = mechanics.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && m.Status != "Deleted");

            if (!filtered.Any())
            {
                throw new Exception($"GetByName: No mechanics found with name containing '{name}'.");
            }
            var mech = mapper.MapMechanicToDisplayDtos(filtered);
            return mech;
        }

        public async Task<IEnumerable<MechanicDisplayDto>> GetAll()
        {
            var mechanics = await _repo.GetAll(1,100);
            var filtered = mechanics.Where(m => m.Status != "Deleted");

            if (!filtered.Any())
            {
                throw new Exception("GetAll: No mechanics found.");
            }
            var mech = mapper.MapMechanicToDisplayDtos(filtered);
            return mech;
        }

        public async Task<MechanicDisplayDto?> UpdateMechanic(string email, MechanicUpdateDto dto)
        {
            var mechanics = await _repo.GetAll(1,100);
            var mechanic = mechanics.FirstOrDefault(m => m.Email == email && m.Status != "Deleted");

            if (mechanic == null)
            {
                throw new Exception($"UpdateMechanic: Mechanic with email '{email}' not found.");
            }

            mechanic.Name = dto.Name;
            mechanic.Phone = dto.Phone;

            var newmechanic = await _repo.Update(mechanic.MechanicId, mechanic);
             var mech = mapper.MapMechanicToDisplayDto(newmechanic);
             return mech;
        }

       
    }

}