﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;

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
                        Name = e.Name
                    })
                .ToListAsync();

            return users;
        }
    }
}