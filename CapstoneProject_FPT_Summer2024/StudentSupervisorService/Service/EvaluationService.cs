﻿using StudentSupervisorService.Models.Request.EvaluationRequest;
using StudentSupervisorService.Models.Response.EvaluationResponse;
using StudentSupervisorService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service
{
    public interface EvaluationService
    {
        Task<DataResponse<List<EvaluationResponse>>> GetAllEvaluations(string sortOrder);
        Task<DataResponse<EvaluationResponse>> GetEvaluationById(int id);
        Task<DataResponse<List<EvaluationResponse>>> SearchEvaluations(int? schoolYearId, string? desciption, DateTime? from, DateTime? to, short? point, string sortOrder);
        Task<DataResponse<EvaluationResponse>> CreateEvaluation(EvaluationCreateRequest request);
        Task<DataResponse<EvaluationResponse>> UpdateEvaluation(EvaluationUpdateRequest request);
        Task<DataResponse<EvaluationResponse>> DeleteEvaluation(int id);
    }
}
