using System;
using System.Collections.Generic;
using System.Text;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface IUserService
    {
        void Create(User model);
        bool IsExist(string login, string passwordHash);
        User Get(string login, string passwordHash);
    }
}
