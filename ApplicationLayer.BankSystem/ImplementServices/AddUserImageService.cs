using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ImplementServices
{
 
    public class AddUserImageService : IAddUserImageService
    {
        #region Fields
        private IFormFileService _formFileService;
        private UserManager<ApplicationUser> _UserManager;

        #endregion

        #region Constructors
        public AddUserImageService(IFormFileService formFileService,UserManager<ApplicationUser> UserManager)
        {
            _UserManager = UserManager;
            _formFileService = formFileService;
        }
        #endregion


        #region HandleMethods
        #endregion




        public async Task<string> AddUserImage(ApplicationUser applicationUser, IFormFile file)
        {
            var ImageUrl = await _formFileService.UploadImage("ApplicationUser", file);

            if (ImageUrl == null)
            {
                return "ImageNotFound";
            }

            // اربط الصورة باليوزر
            applicationUser.Image = ImageUrl;

            // اعمل Update مش Create
            var result = await _UserManager.CreateAsync(applicationUser);

            if (result.Succeeded)
            {
                return "AddImageSuccess";
            }
            else
            {
                return result.Errors.FirstOrDefault()?.Description ?? "ErrorAddingImage";
            }
        }

    }
}
