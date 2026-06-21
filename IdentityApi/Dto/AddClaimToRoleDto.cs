namespace IdentityApi.Dto
{
    public class AddClaimToRoleDto
    {
        public string  RoleName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
