﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.ClassGroupResponse
{
    public class ClassGroupResponse
    {
        public int ClassGroupId { get; set; }
        public string? ClassGroupName { get; set; }
        public string? Hall { get; set; }
        public string? Status { get; set; }
    }
}
