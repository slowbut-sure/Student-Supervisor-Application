﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StudentSupervisorService.CloudinaryConfig;
using static System.Net.Mime.MediaTypeNames;

namespace StudentSupervisorService.Service.Implement
{
    public class ImageUrlImplement : ImageUrlService
    {
        private readonly Cloudinary cloudinary;
        public ImageUrlImplement(IOptions<CloudinarySetting> setting)
        {
            var account = new Account(setting.Value.CloudName, setting.Value.ApiKey, setting.Value.ApiSecret);
            cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile image)
        {
            try
            {
                var uploadResult = new ImageUploadResult();
                if (image.Length > 0)
                {
                    using var stream = image.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, stream),
                    };
                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Upload image failed: " + ex.Message);
            }
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await cloudinary.DestroyAsync(deletionParams);
            return result;
        }
    }
}
