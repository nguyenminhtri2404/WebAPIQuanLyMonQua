namespace GiftManagement_Version2.Data
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string PermissionCode { get; set; }
        public Permission Permission { get; set; }
    }
}
