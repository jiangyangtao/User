using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient.Authentication
{
    public interface IUserAuthenticationProvider
    {
        Task<UserAuthenticationResponse> LoginAsync(string userName, string password);

        Task<UserAuthenticationResponse> ValidationAsync(string userId);
    }
}
