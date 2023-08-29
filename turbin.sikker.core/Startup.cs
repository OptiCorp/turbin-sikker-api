﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using turbin.sikker.core.Services;
using Duende.IdentityServer.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using FluentValidation.AspNetCore;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Model;
using turbin.sikker.core.Validation;
using FluentValidation;
using turbin.sikker.core.Validation.UserValidations;
using turbin.sikker.core.Validation.UserRoleValidations;
using turbin.sikker.core.Common;
using turbin.sikker.core.Utilities;


namespace turbin.sikker.core

{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            IdentityModelEventSource.ShowPII = true;
            ConfigureAuthenticationAndAuthorization(services);

            services.AddIdentityServer()
                .AddSigningCredentials();
            // Add CORS services
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://turbinsikker-app.azurewebsites.net").WithHeaders("Content-Type", "Authorization", "Access-Control-Allow-Origin").AllowAnyMethod());
                // builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
               

            });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IChecklistService, ChecklistService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IChecklistTaskService, ChecklistTaskService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IPunchService, PunchService>();
            services.AddScoped<IChecklistWorkflowService, ChecklistWorkflowService>();

            services.AddScoped<IUserUtilities, UserUtilities>();
            services.AddScoped<IUserRoleUtilities, UserRoleUtilities>();
            services.AddScoped<IChecklistUtilities, ChecklistUtilities>();
            services.AddScoped<ICategoryUtilities, CategoryUtilities>();
            services.AddScoped<IChecklistTaskUtilities, ChecklistTaskUtilities>();
            services.AddScoped<IPunchUtilities, PunchUtilities>();

            services.AddScoped<ValidationHelper>();

            services.AddControllers();
            services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();


            // Add DbContext
            // var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);
            var connectionString = "Data Source=localhost;Initial Catalog=TurbinsikkerDb;User Id=sa; Password=Turbinsikker101;TrustServerCertificate=true;";


            services.AddDbContext<TurbinSikkerDbContext>(options =>
                options.UseSqlServer(connectionString
                ));




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private void ConfigureAuthenticationAndAuthorization(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            });



            services.AddAuthentication()
                .AddJwtBearer("Bearer", options =>
                {
                    options.Audience = "c1b6fd67-db9e-4a25-a4ad-9aec1fe3ec64";
                    options.Authority = "https://login.microsoftonline.com/2d89021a-6a8a-4063-a9fe-b4777c4088f1/";
                    options.RequireHttpsMetadata = false; //Bad? 
                });


            // TODO: Implement Authorization
            services.AddAuthorization(options =>
            {
               var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                   JwtBearerDefaults.AuthenticationScheme, "AzureAD"
                   );
               defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
               options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddAuthentication().AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TurbinSikkerDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable CORS
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string GetSecretValueFromKeyVault(string secretName)
        {
            var keyVaultUrl = Configuration["AzureKeyVault:VaultUrl"];
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);
            var secret = client.GetSecret(secretName);
            return secret.Value.Value;
        }

    }
}
