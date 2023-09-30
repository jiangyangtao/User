using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient
{
    internal class UserGrpcClientProvider : IUserGrpcClientProvider
    {
        private readonly UserGrpcService.UserGrpcServiceClient _userGrpcClient;

        public UserGrpcClientProvider(UserGrpcService.UserGrpcServiceClient userGrpcClient)
        {
            _userGrpcClient = userGrpcClient;
        }

        public async Task<UserResponse> LoginAsync(string username, string password)
        {
            var resuest = new LoginRequest { Username = username, Passwrod = password };
            return await _userGrpcClient.LoginAsync(resuest);
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
            return await _userGrpcClient.ValidationAsync(request);
        }
    }
}
