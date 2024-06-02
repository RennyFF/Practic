using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using practic.Core;
using practic.MVVM.Model;
using practic.Services;

namespace practic.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation= value;
                OnPropertyChanged();
            }
        }
        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            Navigation.NavigateTo<AuthorizeViewModel>();
        }
    }
}
