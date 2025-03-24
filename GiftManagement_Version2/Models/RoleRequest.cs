namespace GiftManagement_Version2.Models
{
    public class RoleRequest
    {
        public string? Name { get; set; }
        public List<string>? PermissionCodes { get; set; }
    }
}
