namespace IdentityApi.Dto
{
    public class AddClaimToUserDto
    {
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
