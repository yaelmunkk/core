using System.Collections.Generic;

namespace Interfaces
{
    using Models;

    public interface IUserService
    {
        List<User>? GetAll();
        User Get(int id);
        void Post(User t);
        void Delete(int id);
    }
}