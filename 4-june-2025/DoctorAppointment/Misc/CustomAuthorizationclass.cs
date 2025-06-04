using Microsoft.AspNetCore.Authorization;
namespace DoctorAppointment.Misc
{
    public class MinimumExperienceRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; }

        public MinimumExperienceRequirement(int minimumYears)
        {
            MinimumYears = minimumYears;
        }
    }
}
