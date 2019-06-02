using GateKeeper.Models;

namespace GateKeeper.Repositories
{
    public interface IUserRepository<U> where U : IUser
    {
        /// <summary>
        /// Returns the user that has the given username or null if
        /// it doesn't exist. The password on the user returned in this
        /// will be encrypted.
        /// </summary>
        U GetByUsername(string username);
    }
}