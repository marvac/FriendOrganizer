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

        public async Task<List<Friend>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Friends.AsNoTracking().ToListAsync();
            }
        }
    }
}
