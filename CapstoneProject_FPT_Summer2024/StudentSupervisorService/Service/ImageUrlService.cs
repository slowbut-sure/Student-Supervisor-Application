﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service
{
    public interface ImageUrlService
    {
        Task<ImageUploadResult> UploadImage(IFormFile images);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}
