using Microsoft.AspNetCore.Authorization;
namespace DoctorAppointment.Misc
{
    public class MinimumExperienceHandler : AuthorizationHandler<MinimumExperienceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumExperienceRequirement requirement)
        {
            var experienceClaim = context.User.FindFirst("YearsOfExperience");

            if (experienceClaim != null && int.TryParse(experienceClaim.Value, out int y))
            {
                if (y >= requirement.MinimumYears)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
