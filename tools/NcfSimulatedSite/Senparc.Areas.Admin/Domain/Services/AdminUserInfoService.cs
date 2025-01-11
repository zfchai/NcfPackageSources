﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Senparc.Areas.Admin.ACL;
using Senparc.Areas.Admin.Domain.Models;
using Senparc.Areas.Admin.Domain.Models.Dto;
using Senparc.Ncf.Core.Config;
using Senparc.Ncf.Core.Exceptions;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Log;
using Senparc.Ncf.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Domain
{
    public class AdminUserInfoService : BaseClientService<AdminUserInfo>
    {

        private readonly Lazy<IHttpContextAccessor> _contextAccessor;

        public AdminUserInfoService(IAdminUserInfoRepository repository, Lazy<IHttpContextAccessor> httpContextAccessor, IServiceProvider serviceProvider)
            : base(repository, serviceProvider)
        {
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckUserNameExisted(long id, string userName)
        {
            userName = userName.Trim().ToUpper();

            return
            GetObject(
                z => z.Id != id && z.UserName.ToUpper() == userName /*z.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)*/) != null;
        }

        ////TODO：放到 OHS 的 Service
        ////[ApiBind]
        ////[Core.BackendJwtAuthorize]
        public async Task<AdminUserInfo> GetUserInfoAsync(string userName)
        {
            var obj = await base.GetObjectAsync(z => z.UserName.Equals(userName.Trim()));
            return obj;
        }


        public async Task LogoutAsync()
        {
            try
            {
                await _contextAccessor.Value.HttpContext.SignOutAsync(SiteConfig.NcfAdminAuthorizeScheme);
            }
            catch (Exception ex)
            {
                LogUtility.AdminUserInfo.ErrorFormat("退出登录失败。", ex);
            }
        }

        /// <summary>
        /// Performs the login operation for the specified admin user
        /// </summary>
        /// <param name="userInfo">The admin user information</param>
        /// <param name="rememberMe">Whether to persist the login</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public virtual async Task LoginAsync(AdminUserInfo userInfo, bool rememberMe)
        {
            #region 使用 .net core 的方法写入 cookie 验证信息

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.UserName),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString(), ClaimValueTypes.Integer),
                new Claim("AdminMember", "", ClaimValueTypes.String)
            };
            var identity = new ClaimsIdentity(SiteConfig.NcfAdminAuthorizeScheme);
            identity.AddClaims(claims);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                IsPersistent = false,
            };

            await LogoutAsync(); //退出登录
            await _contextAccessor.Value.HttpContext.SignInAsync(SiteConfig.NcfAdminAuthorizeScheme, new ClaimsPrincipal(identity), authProperties);

            #endregion
        }

        //public bool CheckPassword(string userName, string password)
        //{
        //    var userInfo = GetObject(z => z.UserName.Equals(userName));
        //    if (userInfo == null)
        //    {
        //        return false;
        //    }
        //    var codedPassword = GetPassword(password, userInfo.PasswordSalt, false);
        //    return userInfo.Password == codedPassword;
        //}

        /// <summary>
        /// 如果密码正确，则尝试登录
        /// </summary>
        /// <param name="adminUserInfo"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        public async Task<AdminUserInfo> TryLoginAsync(AdminUserInfo adminUserInfo, string password, bool rememberMe)
        {
            var cache = CO2NET.Cache.CacheStrategyFactory.GetObjectCacheStrategyInstance();
            var failedLoginKey = $"FailedLogin:{adminUserInfo.UserName}";
            var lockTimeKey = $"LockTime:{adminUserInfo.UserName}";

            // 检查是否在锁定期
            var lockEndTime = await cache.GetAsync<DateTime?>(lockTimeKey);
            if (lockEndTime.HasValue && lockEndTime.Value > DateTime.Now)
            {
                throw new LoginLockException(lockEndTime.Value);
            }

            if (adminUserInfo.CheckPassword(password))
            {
                // 登录成功，清除失败记录
                await cache.RemoveFromCacheAsync(failedLoginKey);
                await cache.RemoveFromCacheAsync(lockTimeKey);

                // 登录
                await LoginAsync(adminUserInfo, rememberMe);
                return adminUserInfo;
            }
            else
            {
                // 记录失败次数
                var failedCount = cache.Get<int>(failedLoginKey);
                failedCount++;

                // 设置锁定时间
                DateTime? lockTime = null;
                if (failedCount >= 20)
                {
                    lockTime = DateTime.Now.AddHours(2);
                }
                else if (failedCount >= 10)
                {
                    lockTime = DateTime.Now.AddMinutes(30);
                }
                else if (failedCount >= 5)
                {
                    lockTime = DateTime.Now.AddMinutes(5);
                }

                // 更新缓存
                await cache.SetAsync(failedLoginKey, failedCount, TimeSpan.FromHours(24));
                if (lockTime.HasValue)
                {
                    await cache.SetAsync(lockTimeKey, lockTime.Value, TimeSpan.FromHours(24));
                    throw new LoginLockException(lockTime.Value);
                }

                return null;
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="objDto"></param>
        public void CreateAdminUserInfo(CreateOrUpdate_AdminUserInfoDto objDto)
        {
            string userName = objDto.UserName;
            string password = objDto.Password;
            var obj = new AdminUserInfo(ref userName, ref password, null, null, objDto.Note);
            SaveObject(obj);
        }

        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="objDto"></param>
        /// <returns></returns>
        public async Task<AdminUserInfo> CreateAdminUserInfoAsync(CreateOrUpdate_AdminUserInfoDto objDto)
        {
            string userName = objDto.UserName;
            string password = objDto.Password;
            var obj = new AdminUserInfo(ref userName, ref password, null, null, objDto.Note);
            await SaveObjectAsync(obj);
            return obj;
        }

        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="objDto"></param>
        /// <returns></returns>
        public async Task<AdminUserInfo> UpdateAdminUserInfoAsync(CreateOrUpdate_AdminUserInfoDto objDto)
        {
            var obj = this.GetObject(z => z.Id == objDto.Id);
            if (obj == null)
            {
                throw new Exception("用户信息不存在！");
            }
            obj.UpdateObject(objDto);
            await SaveObjectAsync(obj);
            return obj;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="objDto"></param>
        public void UpdateAdminUserInfo(CreateOrUpdate_AdminUserInfoDto objDto)
        {
            var obj = this.GetObject(z => z.Id == objDto.Id);
            if (obj == null)
            {
                throw new Exception("用户信息不存在！");
            }
            obj.UpdateObject(objDto);
            SaveObject(obj);
        }

        public CreateOrUpdate_AdminUserInfoDto GetAdminUserInfo(int id, params string[] includes)
        {
            var obj = GetObject(z => z.Id == id, includes: includes);

            var objDto = base.Mapper.Map<CreateOrUpdate_AdminUserInfoDto>(obj);

            return objDto;
        }

        public List<AdminUserInfo> GetAdminUserInfo(List<int> ids, params string[] includes)
        {
            return GetFullList(z => ids.Contains(z.Id), z => z.Id, Ncf.Core.Enums.OrderingType.Ascending, includes: includes);
        }
        //TODO: 统一此处初始化方法为一个方法
        /// <summary>
        /// 初始化，随机生成用户名和密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AdminUserInfo Init(out string userName, out string password)
        {
            userName = null;
            return Init(userName, out password);
        }

        /// <summary>
        /// 初始化，随机生成密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public AdminUserInfo Init(string userName, out string password)
        {
            password = null;

            var oldAdminUserInfo = GetObject(z => true);
            if (oldAdminUserInfo != null)
            {
                return null;
            }
            var adminUserInfo = new AdminUserInfo(ref userName, ref password, null, null, "初始化数据");
            SaveObject(adminUserInfo);
            return adminUserInfo;
        }

        public void DeleteObject(int id)
        {
            var obj = GetObject(z => z.Id == id);
            if (obj == null)
            {
                throw new Exception("用户信息不存在！");
            }
            DeleteObject(obj);
        }

        public override void SaveObject(AdminUserInfo obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("AdminUserInfo{2}：{0}（ID：{1}）", obj.UserName, obj.Id, isInsert ? "新增" : "编辑");
        }

        public override void DeleteObject(AdminUserInfo obj)
        {
            base.DeleteObject(obj);
            LogUtility.WebLogger.InfoFormat("AdminUserInfo被删除：{0}（ID：{1}）", obj.UserName, obj.Id);
        }

        /// <summary>
        /// 登录
        /// <paramref name="loginDto">登录dto</paramref>
        /// </summary>
        /// <returns></returns>
        //[ApiBind(ApiRequestMethod = CO2NET.WebApi.ApiRequestMethod.Post)]
        public async Task<AccountLoginResultDto> LoginAsync(AccountLoginDto loginDto)
        {
            AccountLoginResultDto result = new AccountLoginResultDto();
            string token;
            var userInfo = await GetObjectAsync(z => z.UserName == loginDto.UserName);
            if (userInfo == null)
            {
                throw new NcfExceptionBase($"用户名不存在或密码不正确：{loginDto.UserName}！");
            }
            try 
            {
                var adminUserInfo = await TryLoginAsync(userInfo, loginDto.Password, false);
                if (adminUserInfo == null)
                {
                    throw new NcfExceptionBase("用户名不存在或密码不正确！");
                }
                token = GenerateToken(adminUserInfo.Id);
                var roles = await _serviceProvider.GetService<SysRoleAdminUserInfoService>().GetFullListAsync(o => o.AccountId == adminUserInfo.Id);
                var roleCodes = roles
                    .Select(o => o.RoleCode).Distinct().ToList();
                result.Token = token;
                var permissions = await _serviceProvider.GetService<SysPermissionService>().GetFullListAsync(p => roles.Select(o => o.RoleId).Contains(p.RoleId));
                result.MenuTree = await _serviceProvider.GetService<Domain.Services.SysMenuService>().GetAllMenusTreeAsync(false);
                result.UserName = adminUserInfo.UserName;
                result.RoleCodes = roleCodes;
                result.PermissionCodes = permissions.Select(o => o.ResourceCode);
            }
            catch (LoginLockException)
            {
                throw; // 直接向上层抛出登录锁定异常
            }
            
            return result;
        }

        /// <summary>
        /// 获取当前管理信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AccountLoginResultDto> GetAdminUserInfoAsync(int id)
        {
            AccountLoginResultDto result = new AccountLoginResultDto();
            var adminUserInfo = await GetObjectAsync(z => z.Id == id);
            var roles = await _serviceProvider.GetService<SysRoleAdminUserInfoService>().GetFullListAsync(o => o.AccountId == adminUserInfo.Id);
            var roleCodes = roles
                .Select(o => o.RoleCode).Distinct().ToList();
            var permissions = await _serviceProvider.GetService<SysPermissionService>().GetFullListAsync(p => roles.Select(o => o.RoleId).Contains(p.RoleId));
            result.MenuTree = await _serviceProvider.GetService<Domain.Services.SysMenuService>().GetAllMenusTreeAsync(false);
            result.UserName = adminUserInfo.UserName;
            result.RealName = adminUserInfo.RealName;
            result.RoleCodes = roleCodes;
            result.PermissionCodes = permissions.Select(o => o.ResourceCode);
            return result;
        }


        /// <summary>
        /// GenerateToken
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private string GenerateToken(int memberId)
        {
            var options = _serviceProvider.GetService<IOptionsSnapshot<JwtSettings>>();
            var jwtSettings = options.Get(JwtSettings.Position_Backend);
            byte[] keyBytes = System.Text.Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor securityToken = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, memberId.ToString(), ClaimValueTypes.Integer)
                }),
                Audience = jwtSettings.Audience,
                Issuer = jwtSettings.Issuer,
                Expires = DateTime.UtcNow.AddHours(jwtSettings.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(securityToken);
            string tokenStr = tokenHandler.WriteToken(token);
            return tokenStr;
        }
    }
}
