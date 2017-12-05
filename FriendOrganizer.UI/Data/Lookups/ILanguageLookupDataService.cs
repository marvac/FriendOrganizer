using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public interface ILanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetLanguageLookupAsync();
    }
}