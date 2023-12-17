using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model;

namespace Api.Services
{
    public interface ITokenServices
    {
        public  string CreateToken(UserModel user);
}}