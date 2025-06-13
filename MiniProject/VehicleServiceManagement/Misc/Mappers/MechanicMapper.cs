using VSM.DTO;
using VSM.Models;

namespace VSM.Misc.Mappers
{
    public class MechanicMapper()
    {

        public Mechanic MapMechanic(MechanicAddDto dto)
        {
            var mechanic = new Mechanic
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Status = "Active",

            };
            return mechanic;
        }
        public MechanicDisplayDto MapMechanicToDisplayDto(Mechanic mech)
        {
            var mechanic = new MechanicDisplayDto
            {
                MechanicId = mech.MechanicId,
                Name = mech.Name,
                Email = mech.Email,
                Phone = mech.Phone,

            };
            return mechanic;
        }
        public IEnumerable<MechanicDisplayDto> MapMechanicToDisplayDtos(IEnumerable<Mechanic> mechs)
        {
            List<MechanicDisplayDto> mechanics = [];
            foreach (var item in mechs)
            {
                mechanics.Add(MapMechanicToDisplayDto(item));
            }
            return mechanics;
        }

        internal MechanicDisplayDto? MapMechanicToDisplayDtos(Mechanic newmechanic)
        {
            throw new NotImplementedException();
        }
    }
   
}