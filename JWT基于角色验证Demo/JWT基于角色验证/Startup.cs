using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWT基于角色验证
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //config the db connection string 
            var ConnectionString = Configuration.GetConnectionString("helpcenterdbContext");
            AuthToken.securitykey = Configuration["SecurityKey"];
            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间,
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ClockSkew = TimeSpan.FromSeconds(300), // 允许的服务器时间偏移量
                        ValidAudience = "huobiglobal.zendesk.com",//Audience
                        ValidIssuer = "huobiglobal.zendesk.com",//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyAdminAccess", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserAccess", policy => policy.RequireRole("User"));
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider  serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //启用验证
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        //public async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        //    string[] roleNames = { "Admin","User"};
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {       
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist) {
        //            //create the roles and seed them to the database: Question 1  
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }

        //        IdentityUser user = await UserManager.FindByEmailAsync("jignesh@gmail.com");

        //        if (user == null)
        //        {
        //            user = new IdentityUser()
        //            {
        //                UserName = "jignesh@gmail.com",
        //                Email = "jignesh@gmail.com",
        //            };
        //            await UserManager.CreateAsync(user, "Test@123");
        //        }
        //        await UserManager.AddToRoleAsync(user, "Admin");


        //        IdentityUser user1 = await UserManager.FindByEmailAsync("tejas@gmail.com");

        //        if (user1 == null)
        //        {
        //            user1 = new IdentityUser()
        //            {
        //                UserName = "tejas@gmail.com",
        //                Email = "tejas@gmail.com",
        //            };
        //            await UserManager.CreateAsync(user1, "Test@123");
        //        }
        //        await UserManager.AddToRoleAsync(user1, "User");

        //}
                    
       // }

    }
}
