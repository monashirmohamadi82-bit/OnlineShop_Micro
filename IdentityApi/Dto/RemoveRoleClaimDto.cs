namespace IdentityApi.Dto
{
    public class RemoveRoleClaimDto
    {
        public string RoleName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}