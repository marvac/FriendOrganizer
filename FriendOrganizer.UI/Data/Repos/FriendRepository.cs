using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context) : base(context)
        {

        }

        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends.Include(x => x.PhoneNumbers)
                .SingleAsync(x => x.Id == friendId);
        }

        public void RemovePhoneNumber(PhoneNumber phoneNumber)
        {
            Context.PhoneNumbers.Remove(phoneNumber);
        }
    }
}
