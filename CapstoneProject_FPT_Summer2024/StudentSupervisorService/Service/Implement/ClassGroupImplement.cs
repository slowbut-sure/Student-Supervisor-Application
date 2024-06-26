﻿using AutoMapper;
using Azure.Core;
using Domain.Entity;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.ClassGroupRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ClassGroupResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class ClassGroupImplement : ClassGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassGroupImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<List<ClassGroupResponse>>> GetAllClassGroups(string sortOrder)
        {
            var response = new DataResponse<List<ClassGroupResponse>>();
            try
            {

                var classGroupEntities = await _unitOfWork.ClassGroup.GetAllClassGroups();
                if (classGroupEntities is null || !classGroupEntities.Any())
                {
                    response.Message = "The ClassGroup list is empty";
                    response.Success = true;
                    return response;
                }

                classGroupEntities = sortOrder == "desc"
                    ? classGroupEntities.OrderByDescending(r => r.ClassGroupName).ToList()
                    : classGroupEntities.OrderBy(r => r.ClassGroupName).ToList();

                response.Data = _mapper.Map<List<ClassGroupResponse>>(classGroupEntities);
                response.Message = "List ClassGroup";
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Message = "Oops! Something went wrong.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<ClassGroupResponse>> GetClassGroupById(int id)
        {
            var response = new DataResponse<ClassGroupResponse>();
            try
            {
                var classGroupEntity = await _unitOfWork.ClassGroup.GetClassGroupById(id);
                if (classGroupEntity == null)
                {
                    response.Message = "ClassGroup not found";
                    response.Success = false;
                    return response;
                }

                response.Data = _mapper.Map<ClassGroupResponse>(classGroupEntity);
                response.Message = "Found a ClassGroup";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Something went wrong.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ClassGroupResponse>>> SearchClassGroups(string? name, string? hall, int? slot, TimeSpan? time, string? status, string sortOrder)
        {
            var response = new DataResponse<List<ClassGroupResponse>>();

            try
            {
                var classGroupEntities = await _unitOfWork.ClassGroup.SearchClassGroups(name, hall, slot, time, status);
                if (classGroupEntities is null || classGroupEntities.Count == 0)
                {
                    response.Message = "No ClassGroup matches the search criteria";
                    response.Success = true;
                }
                else
                {
                    if (sortOrder == "desc")
                    {
                        classGroupEntities = classGroupEntities.OrderByDescending(r => r.ClassGroupName).ToList();
                    }
                    else
                    {
                        classGroupEntities = classGroupEntities.OrderBy(r => r.ClassGroupName).ToList();
                    }
                    response.Data = _mapper.Map<List<ClassGroupResponse>>(classGroupEntities);
                    response.Message = "List ClassGroups";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Something went wrong.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<ClassGroupResponse>> CreateClassGroup(ClassGroupCreateRequest request)
        {
            var response = new DataResponse<ClassGroupResponse>();
            try
            {
                var classGroupEntity = new ClassGroup
                {
                    ClassGroupName = request.ClassGroupName,
                    Hall = request.Hall,
                    Status = request.Status
                };

                var created = await _unitOfWork.ClassGroup.CreateClassGroup(classGroupEntity);

                response.Data = _mapper.Map<ClassGroupResponse>(created);
                response.Message = "ClassGroup created successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Create ClassGroup failed: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<ClassGroupResponse>> UpdateClassGroup(ClassGroupUpdateRequest request)
        {
            var response = new DataResponse<ClassGroupResponse>();
            try
            {
                var existingClassGroup = await _unitOfWork.ClassGroup.GetClassGroupById(request.ClassGroupID);
                if (existingClassGroup == null)
                {
                    response.Data = "Empty";
                    response.Message = "ClassGroup not found";
                    response.Success = false;
                    return response;
                }

                existingClassGroup.ClassGroupName = request.ClassGroupName ?? existingClassGroup.ClassGroupName;
                existingClassGroup.Hall = request.Hall ?? existingClassGroup.Hall;
                existingClassGroup.Status = request.Status ?? existingClassGroup.Status;

                await _unitOfWork.ClassGroup.UpdateClassGroup(existingClassGroup);

                response.Data = _mapper.Map<ClassGroupResponse>(existingClassGroup);
                response.Message = "ClassGroup updated successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Update ClassGroup failed: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ClassGroupResponse>> DeleteClassGroup(int id)
        {
            var response = new DataResponse<ClassGroupResponse>();
            try
            {
                var existingClassGroup = await _unitOfWork.ClassGroup.GetClassGroupById(id);
                if (existingClassGroup == null)
                {
                    response.Data = "Empty";
                    response.Message = "ClassGroup not found";
                    response.Success = false;
                    return response;
                }
                await _unitOfWork.ClassGroup.DeleteClassGroup(id);
                response.Data = "Empty";
                response.Message = "ClassGroup deleted successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Something went wrong.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
    }
}
