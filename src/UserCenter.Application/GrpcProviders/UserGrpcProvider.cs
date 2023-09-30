using Grpc.Core;
using Hosting.Grpc.Common;
using UserCenter.Core.Abstractions;
using UserCenter.GrpcServices;

namespace UserCenter.Application.GrpcProviders
{
    public class UserGrpcProvider : UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserProvider _userProvider;

        public UserGrpcProvider(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public override async Task<UserResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userProvider.LoginAsync(request.Username, request.Passwrod);
            if (user == null) return new UserResponse { Error = new ErrorResult { Code = 1, Message = "用户名或密码错误" } };

            var info = new UserInfo { UserId = user.UserId, Username = user.Username, };
            return new UserResponse { Result = info };
        }

        public override async Task<UserResponse> Validation(ValidationUserRequest request, ServerCallContext context)
        {
            var user = await _userProvider.GetByIdAsync(request.UserId);
            if (user == null) return new UserResponse() { Error = new ErrorResult { Code = -1, Message = "账号已停用" } };

            var info = new UserInfo { UserId = user.UserId, Username = user.Username, };
            return new UserResponse { Result = info };
        }
    }
}
