namespace IdentityApi.Dto
{
    public class AddCliamDto
    {
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
