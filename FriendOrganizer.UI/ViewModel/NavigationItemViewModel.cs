using FriendOrganizer.UI.Event;
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
    public class NavigationItemViewModel : ViewModelBase
    {
        public ICommand OpenDetailViewCommand { get; }

        private string _displayMember;

        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value; OnPropertyChanged(); }
        }

        private string _detailViewModelName;
        private int _id;
        private IEventAggregator _eventAggregator;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public NavigationItemViewModel(
            int id, 
            string displayMember, 
            string detailViewModelName,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Id = id;
            DisplayMember = displayMember;
            _detailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                        .Publish(new OpenDetailViewEventArgs
                        {
                            Id = this.Id,
                            ViewModelName = _detailViewModelName
                        });
        }
    }
}
