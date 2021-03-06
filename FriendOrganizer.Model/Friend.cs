﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? LanguageId { get; set; }

        public Language Language { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public Friend()
        {
            PhoneNumbers = new Collection<PhoneNumber>();
        }
    }
}
