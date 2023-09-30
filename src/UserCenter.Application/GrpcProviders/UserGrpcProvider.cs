using Grpc.Core;
using UserCenter.Core.Abstractions;
using UserCenter.GrpcServices;
using Yangtao.Hosting.Extensions;

namespace UserCenter.Application.GrpcProviders
{
    public class UserGrpcProvider : UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserProvider _userProvider;

        public UserGrpcProvider(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public override async Task<UserResponse> GetById(UserIdRequest request, ServerCallContext context)
        {
            var user = await _userProvider.GetByIdAsync(request.UserId);
            if (user == null) return new UserResponse { UserId = string.Empty, Username = string.Empty };

            return new UserResponse { UserId = user.UserId, Username = user.Username };
        }

        public override async Task<UsersResponse> GetUsers(UserIdsRequest request, ServerCallContext context)
        {
            var userIds = request.UserIds.ToArray();
            var users = await _userProvider.GetUsersAsync(userIds);
            if (users.IsNullOrEmpty()) return new UsersResponse();

            var userResponses = users.Select(a => new UserResponse { UserId = a.UserId, Username = a.Username }).ToArray();
            var usersResponse = new UsersResponse();
            usersResponse.Users.AddRange(userResponses);

            return usersResponse;
        }
    }
}
