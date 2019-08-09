using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT基于角色验证
{
    public class AuthToken
    {

        public static string securitykey { get; set; }

        public AuthToken()
        {
        }

        /// <summary>
        /// 根据用户信息生成Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetToken(User user)
        {

            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new[]
            {
                   new Claim(ClaimTypes.Email, user.LoginEmail),
                   new Claim(ClaimTypes.Role,user.UserType == 1?"Admin":"User"),
                   new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                   new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(1440)).ToUnixTimeSeconds()}"),
               };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitykey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            /**
             * Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
                iss: The issuer of the token，token 是给谁的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            var token = new JwtSecurityToken(
                issuer: "huobiglobal.zendesk.com",
                audience: "huobiglobal.zendesk.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(1440),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
