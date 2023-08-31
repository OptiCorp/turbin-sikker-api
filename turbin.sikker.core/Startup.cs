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
                    builder => builder.WithOrigins("https://turbinsikker-app-win-prod.azurewebsites.net").WithHeaders("Content-Type", "Authorization", "Access-Control-Allow-Origin").AllowAnyMethod());
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
            var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);



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
                    options.Audience = "cc0af56e-ee49-46ce-aad6-010dce5bcbb6";
                    options.Authority = "https://login.microsoftonline.com/1a3889b2-f76f-4dd8-831e-b2d5e716c986/";
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
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions {ManagedIdentityClientId = "7ba87be7-bb2b-4f09-b4d0-b47c27191947"});
            var client = new SecretClient(new Uri(keyVaultUrl), credential);
            var secret = client.GetSecret(secretName);
            return secret.Value.Value;
        }

    }
}
