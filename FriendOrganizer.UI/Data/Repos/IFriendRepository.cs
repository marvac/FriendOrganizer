using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        void RemovePhoneNumber(PhoneNumber phoneNumber);
    }
}