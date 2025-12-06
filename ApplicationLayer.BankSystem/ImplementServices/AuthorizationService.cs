using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Helper;
using Domainlayer.BankSystem.Requests;
using Domainlayer.BankSystem.Results;
using InfrastructureLayer.BankSystem.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Domainlayer.BankSystem.Results.ManageUserRoleResult;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class AuthorizationService : IAuthorizationService
    {
        public RoleManager<Role> _rolemanager { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }//زي ال repositories عندي
        public ApplicationDbContext _dbcontext { get; set; }

        public AuthorizationService(RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext)
        {
            _rolemanager = roleManager;
            _userManager = userManager;
            _dbcontext = dbcontext;
        }


        #region HandlerFunction
        public async Task<string> AddRoleAsync(string roleName)
        {
            var IdentityRole = new Role();
            IdentityRole.Name = roleName;

            var result = await _rolemanager.CreateAsync(IdentityRole);  

            if (result.Succeeded)
                   return "Successful";
            else
                return "Failed";
        
        
        }
        public async Task<string> EditRoleAsync(EditRoleRequest Role)
        { 
            var oldName = await _rolemanager.FindByIdAsync(Role.Id.ToString());
            if (oldName == null)
                return "RoleNameIsNotExist";
                oldName.Name = Role.Name;
            var result = await _rolemanager.UpdateAsync(oldName);
            if (result.Succeeded)
                return "Successful";
            else
                return "Failed";
               
        }
        public async Task<string> DeleteRoleAsync(int id)
        {
            var ExistRole = await _rolemanager.FindByIdAsync(id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(ExistRole.Name);
            //return exception 
            if (users != null && users.Count() > 0) return "Used";

            if (ExistRole == null)
                return "RoleNameIsNotExist";
           
            var result = await _rolemanager.DeleteAsync(ExistRole);
            if (result.Succeeded)
                return "Successful";
            else
                return "Failed";

        }


        public async Task<List<string>> GetRolesListAsync()
        {
            var roles = _rolemanager.Roles.Select(r=>r.Name).ToList();
            if (roles == null || roles.Count() <= 0)
                return null;
            return roles;
        }

        public async Task<bool> IsRoleExistAsync(string roleName)
        {
            var roleExist = await _rolemanager.RoleExistsAsync(roleName);
            return roleExist;
        }


        public async Task<string> AddRoleToUser(int id ,string RoleName )
        { 
          var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return "UserNotFound";
            var roleExist = await _rolemanager.RoleExistsAsync(RoleName);
            if (!roleExist)
                return "RoleNotFound";
            var result = await _userManager.AddToRoleAsync(user, RoleName);
            if (result.Succeeded)
                return "Successful";
            else
                return "Failed";
        
        }

        public async Task<string> EditRoleFromUser(int id , string newName ,string oldName)
        { 
           var  user =  await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return "UserNotFound";
            var roleExist = await _rolemanager.RoleExistsAsync(oldName);
            if (!roleExist)
                return "OldRoleNotFound";
            var removeResult = await _userManager.RemoveFromRoleAsync(user, oldName);
            if (!removeResult.Succeeded)
                return "FailedToRemoveOldRole";
            
            
            var Addresult  = await  _userManager.AddToRoleAsync(user, newName);
            if (Addresult.Succeeded)
                return "Successful";
            else
                return "Failed";
        }
        public async Task<string> RemoveRoleFromUser(int id)
        { 
        
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return "UserNotFound";
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any())
                return "NoRolesFound";
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (result.Succeeded)
                return "Successful";
            else
                return "Failed";
        }

        public async Task<ManageUserRoleResult> ManageUserRoles(int id)
        {
            var response = new ManageUserRoleResult();
             
            var user =  await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return null;
            response.UserId = id;
            var userRoles = await _userManager.GetRolesAsync(user);

            if(!userRoles.Any())
                return null;

            var allRoles = _rolemanager.Roles.ToList();
            if (allRoles == null || allRoles.Count() <= 0)
                return null;
            //var UserRolesList = new List<UserRole>();
            //foreach (var role in allRoles)
            //{
            //    var userRole = new UserRole
            //    {
            //        Id = role.Id,
            //        RoleName = role.Name,
            //        HasRole = userRoles.Contains(role.Name)
            //    };
            //    UserRolesList.Add(userRole);
            //}
            var UserRoleList = allRoles.Select(role => new UserRole//للعرض
            {
                Id = role.Id,
                RoleName = role.Name,
                HasRole = userRoles.Contains(role.Name)
            }).ToList();

            response.userRoles = UserRoleList;
            return response;

        }

        public async Task<string> UpdateUserRoles(UpdateUserRolesRequest request)
        {
            var transact = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                //GetUser
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                //OldRoles
                var oldRoles = await _userManager.GetRolesAsync(user);
                if (oldRoles == null)
                    return "NotFoundOldRoles";
                var removeResult = await _userManager.RemoveFromRolesAsync(user, oldRoles);
                //RemoveOldRoles
                var selectedRoles = request.userRoles.Where(r => r.HasRole == true).Select(r => r.RoleName).ToList();
                //AddNewRoles
                var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addResult.Succeeded)
                    return "FailedaddResult";
                await transact.RollbackAsync();
                return "Successful";
            }
            catch (Exception ex) 
            {
                await transact.RollbackAsync();
                return "FailedToUpdateRoles";
            }
              
        }
        public async Task<ManageUserClaimResult> ManageUserClaim(ApplicationUser User)
        { 
            var response = new ManageUserClaimResult();
            response.UserId = User.Id;
            var ClaimList = new List<UserClaim>();
            //GetUserClaims
            var UserClaims = await _userManager.GetClaimsAsync(User);
            //pagination
            foreach (var claim in ClaimsStore.Claims)
            {
                var UserClaim = new UserClaim();
                UserClaim.ClaimType = claim.Type;
                //Checktype
                if (UserClaims.Any(x => x.Type == claim.Type))
                {
                    UserClaim.ClaimValue = true;
                }
                else
                {
                    UserClaim.ClaimValue = false;
                }
              //fillList
                ClaimList.Add(UserClaim);
            }
            //return
            response.UserClaims = ClaimList;
            return response;
        
        }
        public async Task<string> UpdateUserClaim(ManageUserClaimResult request)
        { 
            var transact = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                    return "UserNotFound";
                //GetOldClaims
                var oldClaims = await _userManager.GetClaimsAsync(user);
                //removeoldclaims
                var removeResult = await _userManager.RemoveClaimsAsync(user, oldClaims);

                if (!removeResult.Succeeded)
                    return "FailedToRemoveOldClaims";
                //AddNewClaims
                var selectedClaims = request.UserClaims.Where(c => c.ClaimValue == true)
                                             .Select(c => new Claim(c.ClaimType, c.ClaimType)).ToList();
                var addResult = await _userManager.AddClaimsAsync(user, selectedClaims);
                if (addResult.Succeeded)
                {
                    await transact.CommitAsync();
                    return "Successful";
                }
                else
                {
                    return "FailedToAddNewClaims";
                }
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "FailedToAddNewClaims";
            }
        }

        #endregion
    }
}
