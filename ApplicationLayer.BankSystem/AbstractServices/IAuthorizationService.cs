using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Requests;
using Domainlayer.BankSystem.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IAuthorizationService
    {
        public Task<string> AddRoleAsync(string roleName);
        public Task<string> EditRoleAsync(EditRoleRequest Role);
        public Task<string> DeleteRoleAsync(int id);
        public Task<List<string>> GetRolesListAsync();
        public Task<bool> IsRoleExistAsync(string roleName);
        public Task<string> RemoveRoleFromUser(int id);
        public  Task<string> EditRoleFromUser(int id, string newName, string oldName);
        public  Task<string> AddRoleToUser(int id, string RoleName);

        public Task<ManageUserRoleResult> ManageUserRoles(int id);
        public Task<string> UpdateUserRoles(UpdateUserRolesRequest request);
        public Task<ManageUserClaimResult> ManageUserClaim(ApplicationUser User);
        public Task<string> UpdateUserClaim(ManageUserClaimResult request);
    }
}

