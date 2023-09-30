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

        public async Task<UserResponse> GetUserByIdAsync(string userId)
        {
            var request = new UserIdRequest() { UserId = userId };
            var response = new UserResponse() { UserId = userId, Username = string.Empty };

            var user = await _userGrpcClient.GetByIdAsync(request);
            if (user == null) return response;

            response.Username = user.Username;
            return response;
        }

        public async Task<UserResponse[]> GetUsersAsync(string[] userIds)
        {
            if (userIds == null || userIds.Length <= 0) return Array.Empty<UserResponse>();

            var request = new UserIdsRequest();
            request.UserIds.AddRange(userIds);

            var response = await _userGrpcClient.GetUsersAsync(request);
            if (response == null) return Array.Empty<UserResponse>();
            if (response.Users == null) return Array.Empty<UserResponse>();
            if (response.Users.Count <= 0) return Array.Empty<UserResponse>();

            var users = response.Users.Select(a => new UserResponse { UserId = a.UserId, Username = a.Username }).ToArray();
            return users;
        }
    }
}
