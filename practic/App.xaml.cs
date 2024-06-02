﻿using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using practic.Core;
using practic.MVVM.Model;
using practic.MVVM.View;
using practic.MVVM.ViewModel;
using practic.Services;

namespace practic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private DataBase db = new DataBase();
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AuthorizeViewModel>();
            services.AddSingleton<UserPageViewModel>();
            services.AddSingleton<AdminPageViewModel>();
            services.AddSingleton<RegistrationViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var regViewModel = _serviceProvider.GetRequiredService<RegistrationViewModel>();
            var authorizeViewModel = _serviceProvider.GetRequiredService<AuthorizeViewModel>();
            regViewModel.UsersByRegistrationUpdated += () =>
            {
                authorizeViewModel.USERS = db.GetDBUsers();
            };
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}