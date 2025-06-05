using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
namespace DoctorAppointment.Misc
{
    public class MinimumExperiencePolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "ExperienceOver";
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public MinimumExperiencePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var value = policyName.Substring(POLICY_PREFIX.Length);
                if (int.TryParse(value, out var minYears))
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new MinimumExperienceRequirement(minYears))
                        .Build();

                    return Task.FromResult<AuthorizationPolicy?>(policy);
                }
            }

            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
