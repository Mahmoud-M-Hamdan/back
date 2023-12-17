using Api.Context;
using Api.DTO;
using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {

            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserModel> GetUserByUserNameAsync(string username)
        {
            // return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

          return  await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(u => u.UserName == username);
    }
        


        public void AddUser(UserModel user)
        {
            _context.Users.Add(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }



    }
}