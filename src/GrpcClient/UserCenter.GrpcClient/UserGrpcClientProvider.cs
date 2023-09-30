using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient
{
    internal class UserGrpcClientProvider : IUserGrpcClientProvider
    {
        public UserGrpcClientProvider()
        {
        }

        public Task<UserResponse> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> ValidationAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
