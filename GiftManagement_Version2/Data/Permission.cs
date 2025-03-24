namespace GiftManagement_Version2.Data
{
    public class Permission
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
