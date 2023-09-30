using Grpc.Core;
using Hosting.Grpc.Common;
using UserCenter.Core.Abstractions;
using UserCenter.GrpcServices;

namespace UserCenter.Application.GrpcProviders
{
    public class UserAuthenticationGrpcProvider : UserAuthenticationGrpcService.UserAuthenticationGrpcServiceBase
    {
        private readonly IUserProvider _userProvider;

        public UserAuthenticationGrpcProvider(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public override async Task<UserAuthenticationResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userProvider.LoginAsync(request.Username, request.Passwrod);
            if (user == null) return new UserAuthenticationResponse { Error = new ErrorResult { Code = 1, Message = "用户名或密码错误" } };

            var info = new UserInfo { UserId = user.UserId, Username = user.Username, };
            return new UserAuthenticationResponse { Result = info };
        }

        public override async Task<UserAuthenticationResponse> Validation(ValidationUserRequest request, ServerCallContext context)
        {
            var user = await _userProvider.GetByIdAsync(request.UserId);
            if (user == null) return new UserAuthenticationResponse() { Error = new ErrorResult { Code = -1, Message = "账号已停用" } };

            var info = new UserInfo { UserId = user.UserId, Username = user.Username, };
            return new UserAuthenticationResponse { Result = info };
        }
    }
}
