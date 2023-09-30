using IdentityAuthentication.TokenValidation;
using UserCenter.Application.GrpcProviders;
using Yangtao.Hosting.Endpoint;
using Yangtao.Hosting.Mvc;
using Yangtao.Hosting.NLog;

namespace UserCenter.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add NLog
            builder.Logging.ConfigNLog();

            // Add services to the container.
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddAllowAnyCors();
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddApiVersion();
            services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseIdentityAuthentication();

            app.MapControllers();
            app.UseEnumConfigurationEndpoint();

            app.MapGrpcService<UserGrpcProvider>();
            app.MapGrpcService<UserAuthenticationGrpcProvider>();

            app.Map("/", () => "Hello User Service"); // ื๎ะก API
            app.Run();
        }
    }
}