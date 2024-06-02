using practic.Core;
using practic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic.MVVM.ViewModel
{
    public class UserPageViewModel : Core.ViewModel
    {
        private INavigationService _navigationService;
        public INavigationService Navigation { get => _navigationService; set { _navigationService = value; OnPropertyChanged(); } }

        public RelayCommand NavigateToSettingsView { get; set; }
        public UserPageViewModel(INavigationService navigation) {
            Navigation = navigation;
            NavigateToSettingsView = new RelayCommand(o => { Navigation.NavigateTo<AdminPageViewModel>(); }, o => true);
        }
    }
}
