namespace GiftManagement_Version2.Models
{
    public class PermissionOfRoleRespone
    {
        public int RoleId { get; set; }
        public List<PermissionRespone>? Permissions { get; set; }
    }
}
