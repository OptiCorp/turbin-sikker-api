using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using turbin.sikker.core.Services;
using FluentValidation;
using turbin.sikker.core.Common;
using turbin.sikker.core.Utilities;
using Microsoft.Identity.Web;
using Serilog;

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

            services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            IdentityModelEventSource.ShowPII = true;
            ConfigureAuthenticationAndAuthorization(services);

            services.AddIdentityServer()
                .AddSigningCredentials();
            // Add CORS services
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChecklistService, ChecklistService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IChecklistTaskService, ChecklistTaskService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IPunchService, PunchService>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISocketService, SocketService>();

            services.AddScoped<IUserUtilities, UserUtilities>();
            services.AddScoped<IChecklistUtilities, ChecklistUtilities>();
            services.AddScoped<ICategoryUtilities, CategoryUtilities>();
            services.AddScoped<IChecklistTaskUtilities, ChecklistTaskUtilities>();
            services.AddScoped<IPunchUtilities, PunchUtilities>();
            services.AddScoped<IWorkflowUtilities, WorkflowUtilities>();
            services.AddScoped<IUploadUtilities, UploadUtilities>();
            services.AddScoped<IInvoiceUtilities, InvoiceUtilities>();
            services.AddScoped<INotificationUtilities, NotificationUtilities>();

            services.AddScoped<ValidationHelper>();

            services.AddControllers();
            services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

            services.AddHostedService<CreateInvoiceHandler>();
            services.AddHostedService<NotificationHandler>();
            services.AddHostedService<UserCreateHandler>();
            services.AddHostedService<UserUpdateHandler>();
            services.AddHostedService<UserDeleteHandler>();



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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(options =>
                        {
                            Configuration.Bind("AzureAd", options);
                            options.TokenValidationParameters.NameClaimType = "name";
                        }, options => { Configuration.Bind("AzureAd", options); });


            services.AddAuthorization(config =>
            {
                config.AddPolicy("AuthZPolicy", policyBuilder =>
                policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement() { RequiredScopesConfigurationKey = $"AzureAd.Scopes" }));
            });

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

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable CORS
            app.UseCors("AllowAllHeaders");

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
