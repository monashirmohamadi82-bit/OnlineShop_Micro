namespace IdentityApi.Dto
{
    public class RemoveUserRoleDto
    {
        public Guid UserId { get; set; }
        public string roleName { get; set; }
    }
}
