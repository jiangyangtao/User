using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient.Authentication
{
    public interface IUserAuthenticationProvider
    {
        Task<UserResponse> LoginAsync(string userName, string password);

        Task<UserResponse> ValidationAsync(string userId);
    }
}
