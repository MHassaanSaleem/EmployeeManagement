using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>
                (options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            //  services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


            // used for custom password complexity. 

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 2;
            //    options.Password.RequiredUniqueChars = 1;
            //});

            services.AddMvc();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role")
                .RequireClaim("Create Role"));


                //options.AddPolicy("DeleteRolePolicy", 
                //    policy => policy.RequireClaim("Delete Role")
                //.RequireClaim("Create Role"));

                // for normal role
                //options.AddPolicy("EditRolePolicy",
                //   policy => policy.RequireClaim("Edit Role", "true")); 

                // for custom policy with mutiple checks of role and claims
                //options.AddPolicy("EditRolePolicy",
                //   policy => policy.RequireAssertion(context =>
                //   context.User.IsInRole("Admin") &&
                //   context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //   context.User.IsInRole("Super Admin")
                //   ));

                // for custom policy
                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));

            });


            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.ConfigureApplicationCookie(options =>
            options.AccessDeniedPath = new PathString("/Administration/AccessDenied"));

            // either use Mock Class or SQL Class
            // services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHnadler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Error");

                // app.UseStatusCodePages(); will display only status code. 

                app.UseStatusCodePagesWithRedirects("/Error/{0}");

            }

            app.UseStaticFiles();

            app.UseAuthentication(); // used for identity tables. 

            app.UseMvcWithDefaultRoute();

            //app.UseMvc(routes => {
            //    routes.MapRoute("default", "pragim/{controller=Home}/{action=Index}/{id?}");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World");
            //});

        }
    }
}
