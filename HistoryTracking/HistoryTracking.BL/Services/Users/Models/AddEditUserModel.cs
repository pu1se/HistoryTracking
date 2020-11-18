using System;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services.User
{
    public class AddEditUserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
    }
}
