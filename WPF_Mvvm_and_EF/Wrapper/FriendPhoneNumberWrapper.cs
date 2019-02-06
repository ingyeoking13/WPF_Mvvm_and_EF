﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Wrapper
{
    public class FriendPhoneNumberWrapper : ModelWrapper<FriendPhoneNumber>
    {
        public FriendPhoneNumberWrapper(FriendPhoneNumber model) : base(model)
        {
        }

        public string Number {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
