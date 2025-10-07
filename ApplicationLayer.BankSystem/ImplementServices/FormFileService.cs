using ApplicationLayer.BankSystem.AbstractServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class FormFileService : IFormFileService
    {
        #region fields
         private IWebHostEnvironment ?_webHostEnvironment;

        #endregion



        #region Instructors

        public FormFileService(IWebHostEnvironment? webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        #endregion


        #region HandleMethods
        #endregion

        public async Task<string> UploadImage(string location, IFormFile file)
        {
            var path =_webHostEnvironment.WebRootPath+"/"+ location +"/";
            var extention = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString().Replace("-",string.Empty)+extention;
            ///return pathof image
            if (file.Length > 0)
            {

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = File.Create(path + fileName))
                { 
                     file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    return $"{path}/{fileName}";
                
                }

            }
            else
            { 
                return "NoImageUploaded"; 
            }
        }
    }
}
