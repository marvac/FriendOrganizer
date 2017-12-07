﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }

        private IFriendLookupDataService _friendLookupService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(IFriendLookupDataService friendLookupService,
            IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
        }

        private void AfterFriendDeleted(int friendId)
        {
            var friend = Friends.SingleOrDefault(x => x.Id == friendId);
            if (friend != null)
            {
                Friends.Remove(friend);
            }

        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookup = Friends.SingleOrDefault(f => f.Id == obj.Id);

            if (lookup == null)
            {
                Friends.Add(new NavigationItemViewModel(
                    obj.Id, 
                    obj.DisplayMember, 
                    nameof(FriendDetailViewModel),
                    _eventAggregator));
            }
            else
            {
                lookup.DisplayMember = obj.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(
                    item.Id, 
                    item.DisplayMember,
                    nameof(FriendDetailViewModel),
                    _eventAggregator));
            }
        }
    }
}
