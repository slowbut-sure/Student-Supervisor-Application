﻿using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface ISchoolYearRepository : IGenericRepository<SchoolYear>
    {
        Task<List<SchoolYear>> GetAllSchoolYears();
        Task<SchoolYear> GetSchoolYearById(int id);
    }
}
