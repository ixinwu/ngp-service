/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * UserService Description:
 * 用户业务实现
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NGP.Foundation.Service.System
{
    /// <summary>
    /// 用户业务实现
    /// </summary>
    [ExceptionCallHandler]
    public class UserService : IUserService
    {
        /// <summary>
        /// 数据库仓储
        /// </summary>
        private readonly IUnitRepository _repository;

        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly NGPConfig _config;

        /// <summary>
        /// web帮助
        /// </summary>
        private readonly IWebHelper _webHelper;

        public UserService(IUnitRepository repository, NGPConfig config, IWebHelper webHelper)
        {
            _repository = repository;
            _config = config;
            _webHelper = webHelper;
        }

        /// <summary>
        /// 认证生成token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        
        [TransactionCallHandler]
        public TokenResultInfo Certification(OAuthUserInfo userInfo)
        {
            // 对密码进行加密
            var password = StringExtend.Encrypt(userInfo.Password);

            // 数据库查询
            var dbUser = _repository.FirstOrDefault<Sys_Org_User>(s => !s.IsDelete && s.UserDisabled
            && s.LoginName == userInfo.UserName
            && s.UserPwd == password);

            if (dbUser == null)
            {
                return null;
            }

            var employee = _repository.FirstOrDefault<Sys_Org_Employee>(s => !s.IsDelete && !s.IsDelete && s.Id == dbUser.EmpId);
            if (employee == null)
            {
                return null;
            }
            var emplpyeeDept = _repository.FirstOrDefault<Sys_Org_Empl_Dept>(s => s.EmplId == employee.Id);

            // 生成token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Secret);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddHours(_config.TokenExpiresHour.To<int>());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    // 用户id
                    new Claim(JwtClaimTypes.Id,employee.Id),
                    // 登录名
                    new Claim(JwtClaimTypes.Name,employee.EmplName),
                    // 部门
                    new Claim("DeptId",emplpyeeDept.DeptId),
                    // 登录名
                    new Claim("LoginName",dbUser.LoginName),
                    // 用户编号
                    new Claim("EmplNo",employee.EmplNo),
                    // 是否管理员
                    new Claim("IsSystemAdmin",employee.IsSystemAdmin.ToString())
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 写入地址
            dbUser.UserLastLogonIp = _webHelper.GetCurrentIpAddress();
            dbUser.UserLastLogonTime = DateTime.Now;
            dbUser.UserLogonTimes = (dbUser.UserLogonTimes ?? 0) + 1;
            _repository.Update(dbUser);
            // 保存拦截
            var result = new TokenResultInfo
            {
                AccessToken = tokenString,
                TokenType = "Bearer"
            };
            return result;
        }
    }
}
