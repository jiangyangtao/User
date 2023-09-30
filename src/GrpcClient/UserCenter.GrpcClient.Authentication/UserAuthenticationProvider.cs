using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient.Authentication
{
    internal class UserAuthenticationProvider : IUserAuthenticationProvider
    {
        private readonly UserAuthenticationGrpcService.UserAuthenticationGrpcServiceClient _userAuthenticationClient;

        public UserAuthenticationProvider(UserAuthenticationGrpcService.UserAuthenticationGrpcServiceClient userAuthenticationClient)
        {
            _userAuthenticationClient = userAuthenticationClient;
        }

        public async Task<UserResponse> LoginAsync(string username, string password)
        {
            var resuest = new LoginRequest { Username = username, Passwrod = password };
            return await _userAuthenticationClient.LoginAsync(resuest);
        }

        public async Task<UserResponse> ValidationAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new UserResponse
            {
                Error = new Hosting.Grpc.Common.ErrorResult()
                {
                    Code = -1,
                    Message = "UserId can not be empty or null."
                }
            };

            var request = new ValidationUserRequest() { UserId = userId };
            return await _userAuthenticationClient.ValidationAsync(request);
        }
    }
}
