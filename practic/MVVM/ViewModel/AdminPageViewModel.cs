using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using practic.Core;
using practic.Services;

namespace practic.MVVM.ViewModel
{
    public class AdminPageViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToHomeCommand { get; set; }
        public AdminPageViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<UserPageViewModel>(); }, o => true);
        }
    }
}
