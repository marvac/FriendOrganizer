using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _dataService;
        private IEventAggregator _eventAggregator;
        private FriendWrapper _friend;

        public FriendWrapper Friend
        {
            get { return _friend; }
            set { _friend = value; OnPropertyChanged(); }
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; set; }

        public FriendDetailViewModel(IFriendRepository dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute); //No need for CanExecute since always true
        }

        private async void OnDeleteExecute()
        {
            _dataService.Delete(Friend.Model);
            await _dataService.SaveAsync();
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync();
            //Data saved - no more changes to save
            HasChanges = _dataService.HasChanges();

            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            //Disallow saving if there are no changes to be saved, there are errors, or friend is null
            return Friend != null &&
                !Friend.HasErrors &&
                HasChanges;
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await _dataService.GetByIdAsync(friendId.Value) : CreateFriend();

            //var friend2 = await _dataService.GetByIdAsync((int)friendId);
            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += Friend_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Friend.Id == 0)
            {
                //New friend being created, trigger validation
                Friend.FirstName = "";
            }
        }

        private Friend CreateFriend()
        {
            var friend = new Friend();

            _dataService.Add(friend);
            return friend;
        }

        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _dataService.HasChanges();
            }

            if (e.PropertyName == nameof(Friend.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
