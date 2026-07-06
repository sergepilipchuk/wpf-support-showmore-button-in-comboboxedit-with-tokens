using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MoreTokensApp {
    public class MainViewModel : ViewModelBase {
        public Employee CurrentEmployee {
            get { return GetValue<Employee>(); }
            set { SetValue(value); }
        }
        public ObservableCollection<EmployeeRole> AvailableRoles { get; set; }

        public MainViewModel() {
            AvailableRoles = new ObservableCollection<EmployeeRole> {
                new EmployeeRole { Id = 0, Name = "Manager" },
                new EmployeeRole { Id = 1, Name = "Developer" },
                new EmployeeRole { Id = 2, Name = "Designer" },
                new EmployeeRole { Id = 3, Name = "QA" },
                new EmployeeRole { Id = 4, Name = "HR" },
                new EmployeeRole { Id = 5, Name = "Team Lead" },
                new EmployeeRole { Id = 6, Name = "Product Owner" },
                new EmployeeRole { Id = 7, Name = "Scrum Master" },
                new EmployeeRole { Id = 8, Name = "Business Analyst" },
                new EmployeeRole { Id = 9, Name = "DevOps Engineer" },
                new EmployeeRole { Id = 10, Name = "Support Engineer" },
                new EmployeeRole { Id = 11, Name = "Sales" },
                new EmployeeRole { Id = 12, Name = "Marketing" },
                new EmployeeRole { Id = 13, Name = "Finance" },
                new EmployeeRole { Id = 14, Name = "IT Administrator" }
            };

            CurrentEmployee = new Employee {
                FirstName = "John",
                LastName = "Doe",
                RoleIds = new List<object> { 0, 1, 4, 6, 10, 11, 13, 14 }
            };
        }
    }
}
