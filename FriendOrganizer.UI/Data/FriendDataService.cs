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
    class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDbContext> _context;

        public FriendDataService(Func<FriendOrganizerDbContext> context)
        {
            _context = context;
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            using (var ctx = _context())
            {
                return await ctx.Friends.AsNoTracking().SingleAsync(x => x.Id == friendId);
            }
        }

        public async Task SaveAsync(Friend friend)
        {
            using (var ctx = _context())
            {
                ctx.Friends.Attach(friend);
                ctx.Entry(friend).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
