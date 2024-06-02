using practic.Core;
using practic.MVVM.Model;
using practic.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace practic.MVVM.ViewModel
{
    public class RegistrationViewModel : Core.ViewModel, IDataErrorInfo
    {
        public delegate void UsersUpdatedByRegistrationEventHandler();
        public event UsersUpdatedByRegistrationEventHandler UsersByRegistrationUpdated;
        private INavigationService _navigationService;
        public INavigationService Navigation { get => _navigationService; set { _navigationService = value; OnPropertyChanged(); } }
        private readonly RegValidation _userValidator;
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
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string firstname;
        public string Firstname
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged(nameof(Firstname));
            }
        }

        private string secondname;
        public string Secondname
        {
            get => secondname;
            set
            {
                secondname = value;
                OnPropertyChanged(nameof(Secondname));
            }
        }

        private RelayCommand submitRegistration;
        public RelayCommand SubmitRegistration
        {
            get
            {
                return submitRegistration ??= new RelayCommand(async obj =>
                {
                    var validationResult = _userValidator.Validate(this);
                    if (validationResult.IsValid)
                    {
                        User new_user = new User();
                        new_user.firstName = Firstname;
                        new_user.secondName = Secondname;
                        new_user.password = Password;
                        new_user.login = Login;
                        new_user.isAdmin = false;
                        DataBase db = new DataBase();
                        await db.AddDBUsersAsync(new_user);
                        UsersByRegistrationUpdated?.Invoke();
                        Navigation.NavigateTo<AuthorizeViewModel>();
                    }
                    else
                    {
                        MessageBox.Show($"Ошибки в заполнении полей!");
                    }
                }, obj => true);
            }
        }

        public string this[string columnName]
        {
            get
            {
                var firstOrDefault = _userValidator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                if (firstOrDefault != null)
                    return _userValidator != null ? firstOrDefault.ErrorMessage : "";
                return "";
            }
        }

        public string Error
        {
            get
            {
                if (_userValidator != null)
                {
                    var results = _userValidator.Validate(this);
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }
                }
                return string.Empty;
            }
        }


        public RegistrationViewModel(INavigationService navigation)
        {
            _userValidator = new RegValidation();
            Navigation = navigation;
        }


    }
}
