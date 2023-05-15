using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;

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
            // Add CORS services
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:7082")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Add DbContext
            var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);
            services.AddDbContext<TurbinSikkerDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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
