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
using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NGP.Foundation.Service.Authentication
{
    /// <summary>
    /// 用户业务实现
    /// </summary>
    [ExceptionCallHandler]
    public class AuthenticationService : IAuthenticationService
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

        public AuthenticationService(IUnitRepository repository, NGPConfig config, IWebHelper webHelper)
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
        public NGPResponse<TokenReponse> Certification(TokenRequest userInfo)
        {
            // 对密码进行加密
            var password = StringExtend.Encrypt(userInfo.Password);

            // 数据库查询
            var dbUser = _repository.FirstOrDefault<Sys_Org_User>(s => s.LoginName == userInfo.LoginName);
            // 用户不存在
            if (dbUser == null)
            {
                return new NGPResponse<TokenReponse>
                {
                    ErrorCode = ErrorCode.NonExistent,
                    Status = OperateStatus.Error,
                    Message = string.Format(CommonResource.NotExist, ServiceResource.UserName)
                };
            }

            // 用户不存在
            var employee = _repository.FirstOrDefault<Sys_Org_Employee>(s => !s.IsDelete && !s.IsDelete && s.Id == dbUser.EmpId);
            if (employee == null)
            {
                return new NGPResponse<TokenReponse>
                {
                    ErrorCode = ErrorCode.NonExistent,
                    Status = OperateStatus.Error,
                    Message = string.Format(CommonResource.NotExist, ServiceResource.UserName)
                };
            }

            // 用户被删除
            if (dbUser.IsDelete)
            {
                return new NGPResponse<TokenReponse>
                {
                    ErrorCode = ErrorCode.CheckError,
                    Status = OperateStatus.Error,
                    Message = string.Format(ServiceResource.UserDeleted)
                };
            }

            // 用户被禁用
            if (!dbUser.UserDisabled)
            {
                return new NGPResponse<TokenReponse>
                {
                    ErrorCode = ErrorCode.CheckError,
                    Status = OperateStatus.Error,
                    Message = string.Format(ServiceResource.UserDisabled)
                };
            }

            // 密码不正确
            if (!string.Equals(dbUser.UserPwd, password, StringComparison.CurrentCulture))
            {
                return new NGPResponse<TokenReponse>
                {
                    ErrorCode = ErrorCode.CheckError,
                    Status = OperateStatus.Error,
                    Message = string.Format(ServiceResource.PasswordError)
                };
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
            var tokenInfo = new TokenReponse
            {
                AccessToken = tokenString,
                TokenType = "Bearer"
            };
            return new NGPResponse<TokenReponse>
            {
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
                Data = tokenInfo
            };
        }
    }
}
