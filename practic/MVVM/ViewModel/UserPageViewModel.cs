using practic.Core;
using practic.MVVM.Model;
using practic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace practic.MVVM.ViewModel
{
    public class ColumnOption
    {
        public string DisplayName { get; set; }
        public string ColumnTag { get; set; }
    }

    public class UserPageViewModel : Core.ViewModel
    {
        private ObservableCollection<Ticket> tickets = new();

        public ObservableCollection<Ticket> Tickets
        {
            get => tickets;
            set
            {
                tickets = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }

        public ICollectionView FilteredTickets { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilteredTickets.Refresh();
                }
            }
        }

        private ColumnOption _selectedColumn;
        public ColumnOption SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                if (_selectedColumn != value)
                {
                    _selectedColumn = value;
                    OnPropertyChanged();
                    FilteredTickets.Refresh();
                }
            }
        }

        private User activeUser;

        public User ActiveUser
        {
            get { return activeUser; }
            set
            {
                activeUser = value;
                OnPropertyChanged(nameof(ActiveUser));
            }
        }

        public ObservableCollection<ColumnOption> ColumnOptions { get; set; }


        public UserPageViewModel()
        {
            ColumnOptions = new ObservableCollection<ColumnOption>
            {
                new ColumnOption { DisplayName = "Номер заявки", ColumnTag = "number_ticket" },
                new ColumnOption { DisplayName = "Дата создания", ColumnTag = "date" },
                new ColumnOption { DisplayName = "Поломка", ColumnTag = "causeby" },
                new ColumnOption { DisplayName = "Тип поломки", ColumnTag = "typeofcause" },
                new ColumnOption { DisplayName = "Ответственный", ColumnTag = "responsible" },
                new ColumnOption { DisplayName = "Статус заявки", ColumnTag = "status" }
            };
        }

        public bool FilterTickets(object item)
        {
            if (item is Ticket ticket)
            {
                if (string.IsNullOrEmpty(SearchText) || SelectedColumn == null)
                    return true;

                var property = typeof(Ticket).GetProperty(SelectedColumn.ColumnTag);
                if (property != null)
                {
                    var value = property.GetValue(ticket)?.ToString();
                    return value != null && value.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }
    }
}
