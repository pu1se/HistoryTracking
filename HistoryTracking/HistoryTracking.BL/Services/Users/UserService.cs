using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services
{
    public class UserService : BaseService
    {
        public UserService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<GetUserModel>> GetList()
        {
            var users = await Storage.Users.Select(
                    e =>
                    new GetUserModel
                    {
                        Id = e.Id,
                        Email = e.Email,
                        Name = e.Name,
                        UserType = e.UserType
                    })
                .ToListAsync();

            return users;
        }

        public async Task<GetUserModel> GetItem(Guid userId)
        {
            var users = await GetList();

            return users.FirstOrDefault(x => x.Id == userId);
        }

        public List<UserType> GetUserTypes()
        {
            return EnumHelper.ToList<UserType>();
        }

        public void AddEditUser(AddEditUserModel model)
        {

        }
    }
}
