using AutoMapper;
using FluentValidation;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GiftManagement_Version2.Services
{
    public interface IRoleServices
    {
        string? CreateRole(RoleRequest request);
        string? CreatePermission(PermissionRequest request);
        string? Delete(int id);
        string? Update(int id, RoleRequest request);
        PermissionOfRoleRespone GetPermissionOfRole(int id);
        //Xóa permission ra khỏi role
        string? DeletePermissionOfRole(int roleId, string permissionCode);


    }
    public class RoleServices : IRoleServices
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly IValidator<RoleRequest> validator;

        public RoleServices(IRepositoryWrapper repository, IMapper mapper, IValidator<RoleRequest> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public string? CreateRole(RoleRequest request)
        {
            using IDbContextTransaction transaction = repository.Transaction();
            try
            {
                Role role = repository.Roles.Create(mapper.Map<RoleRequest, Role>(request));
                repository.Save();

                repository.RolePermissions.CreateMulti
                (
                    request.PermissionCodes!
                        .Select(x => new RolePermission { RoleId = role!.Id, PermissionCode = x.Trim() })
                        .ToList()
                );

                repository.Save();
                transaction.Commit();
                return null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return $"Create failed: {ex.Message}";
            }

        }

        public string? CreatePermission(PermissionRequest request)
        {
            Permission permission = repository.Permissions.Create(mapper.Map<PermissionRequest, Permission>(request));
            repository.Save();
            return null;
        }

        public string? Delete(int id)
        {
            Role? role = repository.Roles.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (role == null)
            {
                return "Role not found";
            }
            repository.Roles.Delete(role);
            repository.Save();
            return null;
        }

        public string? Update(int id, RoleRequest request)
        {
            using IDbContextTransaction transaction = repository.Transaction();
            try
            {
                Role? role = repository.Roles.FindByCondition(x => x.Id == id).FirstOrDefault();
                if (role is null)
                    return "Role not found";

                role = repository.Roles.Update(mapper.Map(request, role));

                List<string> permissionOfRole = role.RolePermissions?.Select(x => x.PermissionCode).ToList() ?? new List<string>();
                List<string> requestPermissionCodes = request.PermissionCodes ?? new List<string>();

                List<string> lstRemove = permissionOfRole.Except(requestPermissionCodes).ToList();
                repository.RolePermissions.DeleteMulti(lstRemove.Select(x => new RolePermission { RoleId = role!.Id, PermissionCode = x }).ToList());

                List<string> lstAdd = requestPermissionCodes.Except(permissionOfRole).ToList();
                repository.RolePermissions.CreateMulti(lstAdd.Select(x => new RolePermission { RoleId = role!.Id, PermissionCode = x }).ToList());

                repository.Save();
                transaction.Commit();
                return null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return $"Update failed: {ex.Message}";
            }
        }

        public PermissionOfRoleRespone GetPermissionOfRole(int id)
        {
            Role? role = repository.Roles
                .FindByCondition(r => r.Id == id)
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault();

            if (role == null)
            {
                return null;
            }

            PermissionOfRoleRespone permissionOfRoleRespone = new()
            {
                RoleId = role.Id,
                Permissions = role.RolePermissions
                       .Select(rp => new PermissionRespone
                       {
                           Code = rp.Permission.Code.Trim(),
                           Name = rp.Permission.Name.Trim()
                       })
                       .ToList()
            };

            return permissionOfRoleRespone;
        }

        public string? DeletePermissionOfRole(int roleId, string permissionCode)
        {
            RolePermission? rolePermission = repository.RolePermissions.FindByCondition(x => x.RoleId == roleId && x.PermissionCode == permissionCode).FirstOrDefault();
            if (rolePermission == null)
            {
                return "RolePermission not found";
            }
            repository.RolePermissions.Delete(rolePermission);
            repository.Save();
            return null;
        }

    }
}
