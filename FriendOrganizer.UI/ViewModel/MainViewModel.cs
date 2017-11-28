using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendDataService _dataService;

        public ObservableCollection<Friend> Friends { get; set; } = new ObservableCollection<Friend>();
        private Friend _selectedFriend;

        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set { _selectedFriend = value; OnPropertyChanged(); }
        }


        public MainViewModel(IFriendDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadAsync()
        {
            var friends = await _dataService.GetAllAsync();

            Friends.Clear();

            foreach (var friend in friends)
            {
                Friends.Add(friend);
            }
        }

    }
}
