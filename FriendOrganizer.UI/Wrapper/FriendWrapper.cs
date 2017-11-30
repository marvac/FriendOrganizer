using FriendOrganizer.Model;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public FriendWrapper(Friend friend) : base(friend)
        {

        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "test", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "First name cannot be test.";
                    }
                    break;
                case nameof(LastName):
                    if (string.Equals(LastName, "test", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Last name cannot be test.";
                    }
                    break;
                case nameof(Email):
                    if (Email.ToLower().Contains("@mailinator"))
                    {
                        yield return "Enter a valid email address.";
                    }
                    break;
            }
        }
    }
}
