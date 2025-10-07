using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IAddUserImageService
    {
        public Task<string> AddUserImage(ApplicationUser applicationUser, IFormFile file);
    }
}
