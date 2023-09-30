using Grpc.Core;
using UserCenter.GrpcServices;

namespace UserCenter.Application.GrpcProviders
{
    public class UserGrpcProvider : UserGrpcService.UserGrpcServiceBase
    {
        public UserGrpcProvider()
        {
        }

        public override Task<UserResponse> Login(LoginRequest request, ServerCallContext context)
        {
            return base.Login(request, context);
        }

        public override Task<UserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            return base.GetUser(request, context);
        }
    }
}
