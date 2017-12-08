using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _dataService;
        private IMessageDialogService _messageDialogService;
        private ILanguageLookupDataService _languageLookupDataService;
        private PhoneNumberWrapper _selectedPhoneNumber;
        private FriendWrapper _friend;
        private bool _hasChanges;


        public PhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set { _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public FriendWrapper Friend
        {
            get { return _friend; }
            set { _friend = value; OnPropertyChanged(); }
        }

        public ICommand AddPhoneNumberCommand { get; set; }
        public ICommand RemovePhoneNumberCommand { get; set; }
        public ObservableCollection<LookupItem> Languages { get; }
        public ObservableCollection<PhoneNumberWrapper> PhoneNumbers { get; set; }

        #region Constructor
        public FriendDetailViewModel(
            IFriendRepository dataService, 
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ILanguageLookupDataService languageLookupDataService) : base(eventAggregator)
        {
            _dataService = dataService;
            _messageDialogService = messageDialogService;
            _languageLookupDataService = languageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute); //No need for CanExecute since always true

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            Languages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<PhoneNumberWrapper>();
        }
        #endregion

        #region Public Methods

        public override async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await _dataService.GetByIdAsync(friendId.Value) : CreateFriend();

            InitializeFriend(friend);

            InitializePhoneNumbers(friend.PhoneNumbers);

            await LoadLanguagesAsync();
        }

        #endregion

        #region Private Methods

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += Friend_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Friend.Id == 0)
            {
                //New friend being created, trigger validation
                Friend.FirstName = "";
            }
        }

        private void InitializePhoneNumbers(ICollection<PhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= PhoneNumberWrapper_PropertyChanged;
            }

            PhoneNumbers.Clear();

            foreach (var phoneNumber in phoneNumbers)
            {
                var wrapper = new PhoneNumberWrapper(phoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += PhoneNumberWrapper_PropertyChanged;
            }
        }

        private void PhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _dataService.HasChanges();
            }

            if (e.PropertyName == nameof(PhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog(
                $"Are you sure you want to delete {Friend.FirstName} {Friend.LastName}?", "Delete?");

            if (result == MessageDialogResult.OK)
            {
                _dataService.Delete(Friend.Model);
                await _dataService.SaveAsync();

                RaiseDetailDeletedEvent(Friend.Id);
            }
        }

        protected override async void OnSaveExecute()
        {
            await _dataService.SaveAsync();
            //Data saved - no more changes to save
            HasChanges = _dataService.HasChanges();
            RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
        }

        protected override bool OnSaveCanExecute()
        {
            //Disallow saving if there are no changes to be saved, there are errors, or friend is null
            return Friend != null &&
                PhoneNumbers.All(x => !x.HasErrors) &&
                !Friend.HasErrors &&
                HasChanges;
        }

        private Friend CreateFriend()
        {
            var friend = new Friend();

            _dataService.Add(friend);
            return friend;
        }

        private async Task LoadLanguagesAsync()
        {
            Languages.Clear();
            var lookup = await _languageLookupDataService.GetLanguageLookupAsync();

            foreach (var item in lookup)
            {
                Languages.Add(item);
            }
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

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= PhoneNumberWrapper_PropertyChanged;

            //Friend.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model); //Causes null ref in database
            _dataService.RemovePhoneNumber(SelectedPhoneNumber.Model);

            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _dataService.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new PhoneNumberWrapper(new PhoneNumber());
            newNumber.PropertyChanged += PhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Phone = ""; //Validation triggered here
        }
        #endregion

    }
}
