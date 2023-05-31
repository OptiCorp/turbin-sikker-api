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
                    builder => builder.WithOrigins("https://localhost:7082")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<ICategoryService, CategoryService>();



            // Add DbContext
            var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);
            services.AddDbContext<TurbinSikkerDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddControllers();

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
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        //ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://login.microsoftonline.com/df4dc9e8-cc4f-4792-a55e-36f7e1d92c47",
                        ValidAudience = "a337567c-cda4-4847-89d9-16c4c67128cc"
                    };
                    options.Audience = "95763e09-e04c-48a8-99a6-a878ed99d774";
                    options.Authority = "https://login.microsoftonline.com/df4dc9e8-cc4f-4792-a55e-36f7e1d92c47";
                    options.RequireHttpsMetadata = false; //Bad? 
                });


            services.AddAuthentication()
                .AddJwtBearer("AzureAD", options =>
                {
                    options.Audience = "95763e09-e04c-48a8-99a6-a878ed99d774";
                    options.Authority = "https://login.microsoftonline.com/df4dc9e8-cc4f-4792-a55e-36f7e1d92c47";
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

        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
