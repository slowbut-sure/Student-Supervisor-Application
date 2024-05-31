﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.UserResponse
{
    public class ResponseOfUser
    {
        public int UserId { get; set; }

        public int? SchoolAdminId { get; set; }

        public byte RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Address { get; set; }

    }
}