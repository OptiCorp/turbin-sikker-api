using Azure.Identity;
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
using turbin.sikker.core.Validation;
using FluentValidation;

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
                    builder => builder.WithOrigins("http://localhost:5173").WithHeaders("Content-Type", "Authorization").AllowAnyMethod());
                //builder.AllowAnyOrigin().AllowAnyHeader().WithExposedHeaders("Authorization").SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
                //WithOrigins("https://localhost:5173", "https://localhost:7190")
                //    .AllowAnyHeader()
                //    .AllowAnyMethod()
                //    .AllowCredentials()
                //    );

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



            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<UserCreateDto>, UserCreateValidator>();
            services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();

            // Add DbContext
            var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);

            //DEVELOPMENT CONNECTION STRING
            //BjørnOle
            //var connectionString = "Data Source=localhost;Initial Catalog=turbinsikkerdb;User Id=sa; Password=Turbinsikker101;TrustServerCertificate=true;";
            //Malin
            //var connectionString = "Server =.\\SQLEXPRESS; Database = turbinsikkerdb; Trusted_Connection = True; TrustServerCertificate = Yes;";
            //Katrine
            //var connectionString = "Data Source=localhost;Initial Catalog=Turbinsikkerdb;User Id=sa; Password=Turbinsikker101;TrustServerCertificate=true;";


            services.AddDbContext<TurbinSikkerDbContext>(options =>
                options.UseSqlServer(connectionString
                ));




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private void ConfigureAuthenticationAndAuthorization(IServiceCollection services)
        {
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //});



            services.AddAuthentication()
                .AddJwtBearer("Bearer", options =>
                {
                    options.Audience = "95763e09-e04c-48a8-99a6-a878ed99d774";
                    options.Authority = "https://login.microsoftonline.com/df4dc9e8-cc4f-4792-a55e-36f7e1d92c47/v2.0";
                    options.RequireHttpsMetadata = false; //Bad? 
                });


            // TODO: Implement Authorization
            //services.AddAuthorization(options =>
            //{
            //    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            //        JwtBearerDefaults.AuthenticationScheme, "AzureAD"
            //        );
            //    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            //    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            //});

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
