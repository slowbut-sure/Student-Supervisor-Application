﻿using AutoMapper;
using Infrastructures.Interfaces.IUnitOfWork;
using Microsoft.IdentityModel.Tokens;

using StudentSupervisorService.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Domain.Entity;
using Domain.Enums.Role;
using Infrastructures.Interfaces;
using Microsoft.Extensions.Configuration;

namespace StudentSupervisorService.Service.Implement
{
    public class LoginImplement : LoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly TokenBlacklistService _tokenBlacklistService;

        public LoginImplement(IUnitOfWork unitOfWork, IConfiguration config, TokenBlacklistService tokenBlacklistService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task<(bool success, string message, string token)> Login(LoginModel login, bool isAdmin)
        {
            if (isAdmin)
            {
                var admin = await AuthenticateAdmin(login);
                if (admin != null)
                {
                    var token = GenerateToken(admin, isAdmin);
                    return (true, "Login successful", token);
                }

                var existingAdmin = await _unitOfWork.Admin.GetAccountByPhone(login.Phone);
                return (existingAdmin == null) ? (false, "Invalid phone number.", null) : (false, "Invalid password.", null);
            }
            else
            {
                var user = await AuthenticateUser(login);
                if (user != null)
                {
                    var token = GenerateToken(user, isAdmin);
                    return (true, "Login successful", token);
                }

                var existingUser = await _unitOfWork.User.GetAccountByPhone(login.Phone);
                return (existingUser == null) ? (false, "Invalid phone number.", null) : (false, "Invalid password.", null);
            }
        }

        public void Logout(string token)
        {
            _tokenBlacklistService.BlacklistToken(token);
        }

        private async Task<Admin> AuthenticateAdmin(LoginModel login)
        {
            var admin = await _unitOfWork.Admin.GetAccountByPhone(login.Phone);
            if (admin != null && admin.Password == login.Password)
            {
                return admin;
            }
            return null;
        }

        private async Task<User> AuthenticateUser(LoginModel login)
        {
            var user = await _unitOfWork.User.GetAccountByPhone(login.Phone);
            if (user != null && user.Password == login.Password)
            {
                return user;
            }
            return null;
        }

        private string GenerateToken(dynamic user, bool isAdmin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Phone),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
