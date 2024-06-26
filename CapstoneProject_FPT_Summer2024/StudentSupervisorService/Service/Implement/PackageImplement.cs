﻿using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.PackageRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PackageResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class PackageImplement : PackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<ResponseOfPackage>> CreatePackage(PackageRequest request)
        {
            var response = new DataResponse<ResponseOfPackage>();

            try
            {
                var createPackage = _mapper.Map<Package>(request);
                createPackage.Status = PackageEnum.ACTIVE.ToString();
                _unitOfWork.Package.Add(createPackage);
                _unitOfWork.Save();
                response.Data = _mapper.Map<ResponseOfPackage>(createPackage);
                response.Message = "Create Successfully.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Some thing went wrong.\n" + ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task DeletePackage(int id)
        {
            var vioGroup = _unitOfWork.Package.GetById(id);
            if (vioGroup is null)
            {
                throw new Exception("Can not found by" + id);
            }
            vioGroup.Status = PackageEnum.INACTIVE.ToString();
            _unitOfWork.Package.Update(vioGroup);
            _unitOfWork.Save();
        }

        public async Task<DataResponse<List<ResponseOfPackage>>> GetAllPackages(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfPackage>>();

            try
            {
                var package = await _unitOfWork.Package.GetAllPackages();
                if (package is null || !package.Any())
                {
                    response.Message = "The Package list is empty";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách Package theo yêu cầu
                var packageDTO = _mapper.Map<List<ResponseOfPackage>>(package);
                if (sortOrder == "desc")
                {
                    packageDTO = packageDTO.OrderByDescending(r => r.Price).ToList();
                }
                else
                {
                    packageDTO = packageDTO.OrderBy(r => r.Price).ToList();
                }
                response.Data = packageDTO;
                response.Message = "List Packages";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Some thing went wrong.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfPackage>> GetPackageById(int id)
        {
            var response = new DataResponse<ResponseOfPackage>();

            try
            {
                var package = await _unitOfWork.Package.GetPackageById(id);
                if (package is null)
                {
                    throw new Exception("The Package does not exist");
                }
                response.Data = _mapper.Map<ResponseOfPackage>(package);
                response.Message = $"PackageId {package.PackageId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Some thing went wrong.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfPackage>>> SearchPackages(string? name, int? minPrice, int? maxPrice, string? type, string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfPackage>>();

            try
            {
                var packages = await _unitOfWork.Package.SearchPackages(name, minPrice, maxPrice, type);
                if (packages is null || packages.Count == 0)
                {
                    response.Message = "No Package found matching the criteria";
                    response.Success = true;
                }
                else
                {
                    var packageDTO = _mapper.Map<List<ResponseOfPackage>>(packages);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        packageDTO = packageDTO.OrderByDescending(p => p.Price).ToList();
                    }
                    else
                    {
                        packageDTO = packageDTO.OrderBy(p => p.Price).ToList();
                    }

                    response.Data = packageDTO;
                    response.Message = "Packages found";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Something went wrong.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfPackage>> UpdatePackage(int id, PackageRequest request)
        {
            var response = new DataResponse<ResponseOfPackage>();

            try
            {
                var package = _unitOfWork.Package.GetById(id);
                if (package is null)
                {
                    response.Message = "Can not found Package";
                    response.Success = false;
                    return response;
                }

                package.Name = request.Name;
                package.Description = request.Description;
                package.RegisteredDate = DateTime.Now;
                package.Price = request.Price;
                package.Type = request.Type;

                _unitOfWork.Package.Update(package);
                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfPackage>(package);
                response.Success = true;
                response.Message = "Update Successfully.";
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Some thing went wrong.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }
    }
}
