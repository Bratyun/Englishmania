using System;
using System.Collections.Generic;
using System.Text;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(User model)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string login, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
