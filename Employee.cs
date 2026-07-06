using DevExpress.Mvvm;
using System;
using System.Collections.Generic;

namespace MoreTokensApp
{
    public class Employee : BindableBase
    {
        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Position
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public List<object> RoleIds
        {
            get => GetValue<List<Object>>();
            set => SetValue(value);
        }

        public Employee()
        {
            if (RoleIds == null)
                RoleIds = new List<object>();
        }
    }
}
