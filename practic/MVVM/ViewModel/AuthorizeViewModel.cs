using practic.Core;
using practic.Services;
using practic.MVVM.Model;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace practic.MVVM.ViewModel
{
   public class TextToPasswordCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new string('*', value?.ToString().Length ?? 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AuthorizeViewModel : Core.ViewModel
    {
        private List<User> users_list = new();
        public List<User> USERS
        {
            get { return users_list; }
            set
            {
                users_list = value;
            }
        }

        private DataBase db = new();
        private string login; 

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string password;

        public string Password
        {
            get => password; 
            set { password = value; OnPropertyChanged(); }
        }

        private INavigationService _navigationService;
        public INavigationService Navigation { get => _navigationService; set { _navigationService = value; OnPropertyChanged(); } }

        private RelayCommand logInCommand;
        private RelayCommand goToRegistration;

        public User? Authenticate(string login, string password, List<User> users)
        {
            foreach (User u in users)
            {
                if (u.login == login && u.password == password)
                {
                    return u;
                }
            }

            MessageBox.Show("Такого пользователя нет!");
            return null;
        }

        public RelayCommand LogInCommand
        {
            get
            {
                return logInCommand ??= new RelayCommand(obj =>
                {
                    User? user = Authenticate(Login, Password, USERS);
                    if (user != null)
                    {
                        if (user.isAdmin)
                        {
                            Navigation.NavigateTo<AdminPageViewModel>();
                        }
                        else
                        {
                            Navigation.NavigateTo<UserPageViewModel>();
                        }
                    }
                }, obj => true);
            }
        }
        public RelayCommand GoToRegistration
        {
            get
            {
                return goToRegistration ??= new RelayCommand(obj =>
                {
                    Navigation.NavigateTo<RegistrationViewModel>();
                }, obj => true);
            }
        }
        public AuthorizeViewModel(INavigationService navigation)
        {
            bool isSuccessCreation = false;
            isSuccessCreation = db.CreateDBUsers();
            isSuccessCreation = db.CreateDBTickets();
            if (isSuccessCreation)
            {
               USERS = db.GetDBUsers();
            }
            else
            {
                MessageBox.Show("Данные не смогли загрузится", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Navigation = navigation;
        }

        async void Creation()
        {
            User us = new User();
            us.id = 1;
            us.login = "root";
            us.password = "root";
            us.firstName = "";
            us.secondName = "";
            us.isAdmin = true;
           await db.AddDBUsersAsync(us);
           us.id = 2;
            us.login = "user";
            us.password = "user";
            us.firstName = "";
            us.secondName = "";
            us.isAdmin = false;
           await db.AddDBUsersAsync(us);
        }
    }
}
