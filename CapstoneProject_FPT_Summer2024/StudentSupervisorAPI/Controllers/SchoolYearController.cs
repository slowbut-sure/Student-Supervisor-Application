﻿using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.SchoolYearRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.SchoolYearResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/school-years")]
    [ApiController]
    public class SchoolYearController : ControllerBase
    {
        private SchoolYearService _service;
        public SchoolYearController(SchoolYearService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ResponseOfSchoolYear>>>> GetSchoolYears(string sortOrder = "asc")
        {
            try
            {
                var schoolYear = await _service.GetAllSchoolYears(sortOrder);
                return Ok(schoolYear);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> GetSchoolYearById(int id)
        {
            try
            {
                var staff = await _service.GetSchoolYearById(id);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSchoolYears(short? year = null, DateTime? startDate = null, DateTime? enddate = null, string sortOrder = "asc")
        {
            try
            {
                var schoolYears = await _service.SearchSchoolYears(year, startDate, enddate, sortOrder);
                return Ok(schoolYears);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> CreateSchoolYear(RequestCreateSchoolYear request)
        {
            var createdSchoolYear = await _service.CreateSchoolYear(request);
            return createdSchoolYear == null ? NotFound() : Ok(createdSchoolYear);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> UpdateSchoolYear(int id, RequestCreateSchoolYear request)
        {
            var updatedSchoolYear = await _service.UpdateSchoolYear(id, request);
            return updatedSchoolYear == null ? NotFound() : Ok(updatedSchoolYear);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchoolYear(int id)
        {
            var deletedSchoolYear = _service.DeleteSchoolYear(id);
            return deletedSchoolYear == null ? NoContent() : Ok(deletedSchoolYear);
        }
    }
}
