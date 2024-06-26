﻿using Domain.Entity;
using Infrastructures.Interfaces;
using Infrastructures.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repository
{
    public class ClassRepository : GenericRepository<Class>, IClassReposirory
    {
        public ClassRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<Class>> GetAllClasses()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class> GetClassById(int id)
        {
            return await _context.Classes.FirstOrDefaultAsync(x => x.ClassId == id);
        }

        public async Task<List<Class>> SearchClasses(int? schoolYearId, int? classGroupId, string? code, string? name, int? totalPoint)
        {
            var query = _context.Classes.AsQueryable();

            if (schoolYearId != null)
            {
                query = query.Where(p => p.SchoolYearId == schoolYearId);
            }
            if (classGroupId != null)
            {
                query = query.Where(p => p.ClassGroupId == classGroupId);
            }
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(p => p.Code.Contains(code));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            if (totalPoint != null)
            {
                query = query.Where(p => p.TotalPoint == totalPoint);
            }

            return await query.ToListAsync();
        }
        public async Task<Class> CreateClass(Class classEntity)
        {
            await _context.Classes.AddAsync(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }
        public async Task<Class> UpdateClass(Class classEntity)
        {
            _context.Classes.Update(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }
        public async Task DeleteClass(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
        }
    }
}
