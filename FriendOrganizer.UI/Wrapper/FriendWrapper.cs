using FriendOrganizer.Model;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        
        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue<string>(value);
                OnPropertyChanged();
                ValidateProperty();
            }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue<string>(value);
                OnPropertyChanged();
                ValidateProperty();
            }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue<string>(value);
                OnPropertyChanged();
                ValidateProperty();
            }
        }

        public FriendWrapper(Friend friend) : base(friend)
        {

        }

        private void ValidateProperty([CallerMemberName]string propertyName = null)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case nameof(FirstName):
                    if(string.Equals(FirstName, string.Empty, StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "First name cannot be blank.");
                    }
                    break;
                case nameof(LastName):
                    if (string.Equals(LastName, string.Empty, StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "Last name cannot be blank.");
                    }
                    break;
                case nameof(Email):
                    if (Email.ToLower().Contains("@mailinator"))
                    {
                        AddError(propertyName, "Enter a valid email address.");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
