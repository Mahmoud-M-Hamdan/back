using Api.Model;

namespace Api.Services
{
    public interface IUserRepository
    {

        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByIdAsync(int id);
        Task<UserModel> GetUserByUserNameAsync(string username);
        void AddUser(UserModel user);
        public  Task<bool> SaveChangesAsync();

    }
}