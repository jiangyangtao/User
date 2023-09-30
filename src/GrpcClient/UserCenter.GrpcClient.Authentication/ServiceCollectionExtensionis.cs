using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient.Authentication
{
    public static class ServiceCollectionExtensionis
    {
        public static IServiceCollection AddUserCenterAuthenticationGrpcClient(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var userCenterConfig = configuration.GetSection("UserCenter") ?? throw new KeyNotFoundException("In configuration not found UserCenter");
            var endpoingConfig = userCenterConfig.GetSection("Endpoint") ?? throw new KeyNotFoundException("In configuration not found Endpoint of UserCenter");

            return services.AddUserCenterAuthenticationGrpcClient(a => a.Endpoint = endpoingConfig.Value);
        }


        public static IServiceCollection AddUserCenterAuthenticationGrpcClient(this IServiceCollection services, string endpoint)
            => services.AddUserCenterAuthenticationGrpcClient(a => a.Endpoint = endpoint);

        public static IServiceCollection AddUserCenterAuthenticationGrpcClient(this IServiceCollection services, Action<GrpcClientOptions> action)
        {
            var clientOptions = new GrpcClientOptions();
            action(clientOptions);

            services.AddGrpcClient<UserAuthenticationGrpcService.UserAuthenticationGrpcServiceClient>(options =>
            {
                options.Address = new Uri(clientOptions.Endpoint);
                options.ChannelOptionsActions.Add((channelOptions) =>
                {
                    // 允许自签名证书
                    channelOptions.HttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                    };

                    var serviceConfig = new ServiceConfig();
                    serviceConfig.MethodConfigs.Add(new MethodConfig
                    {
                        Names = { MethodName.Default },
                        RetryPolicy = new RetryPolicy       // 重试策略
                        {
                            MaxAttempts = 5,
                            InitialBackoff = TimeSpan.FromSeconds(1),
                            MaxBackoff = TimeSpan.FromSeconds(5),
                            BackoffMultiplier = 1.5,
                            RetryableStatusCodes = { StatusCode.Unavailable }
                        }
                    });
                    channelOptions.ServiceConfig = serviceConfig;
                });
            });

            services.AddSingleton<IUserAuthenticationProvider, UserAuthenticationProvider>();
            return services;
        }
    }
}