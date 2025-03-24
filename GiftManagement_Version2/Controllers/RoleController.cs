using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices roleServices;

        public RoleController(IRoleServices roleServices)
        {
            this.roleServices = roleServices;
        }

        [HttpPost("CreateRole")]
        public IActionResult CreateRole(RoleRequest request)
        {
            string? result = roleServices.CreateRole(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpPost("CreatePermission")]
        public IActionResult CreatePermission(PermissionRequest request)
        {
            string? result = roleServices.CreatePermission(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string? result = roleServices.Delete(id);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpPut("UpdateRole/{id}")]
        public IActionResult Update(int id, RoleRequest request)
        {
            string? result = roleServices.Update(id, request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("GetPermissionOfRole/{id}")]
        public IActionResult Get(int id)
        {
            PermissionOfRoleRespone permissionOfRoleRespone = roleServices.GetPermissionOfRole(id);
            if (permissionOfRoleRespone == null)
            {
                return NotFound();
            }
            return Ok(permissionOfRoleRespone);
        }

        [HttpDelete("DeletePermissionOfRole/{roleId}/{permissionCode}")]
        public IActionResult DeletePermissionOfRole(int roleId, string permissionCode)
        {
            string? result = roleServices.DeletePermissionOfRole(roleId, permissionCode);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }
    }
}
