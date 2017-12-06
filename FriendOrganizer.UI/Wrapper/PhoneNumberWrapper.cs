using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
    public class PhoneNumberWrapper : ModelWrapper<PhoneNumber>
    {
        public PhoneNumberWrapper(PhoneNumber phoneNumber) : base(phoneNumber)
        {

        }


        public string Phone
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

    }

}
