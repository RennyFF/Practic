using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using practic.Core;
using practic.MVVM.Model;
using practic.Services;

namespace practic.MVVM.ViewModel
{

    public class AdminPageViewModel : Core.ViewModel
    {
        private DataBase db = new();
        private RelayCommand addTicketCommand { get; set; }
        public RelayCommand AddTicketCommand
        {
            get
            {
                return addTicketCommand ??= new RelayCommand(async obj =>
                {
                    Ticket newTicket = new Ticket
                    {
                        id = -1,
                        number_ticket = -1,
                        date = DateTime.Now.ToString(),
                        causeby = "Новая проблема",
                        typeofcause = "Тип проблемы",
                        client_id = -1,
                        responsible = "Новый ответственный",
                        status = "В обработке"
                    };

                    Tickets.Add(newTicket);
                }, obj => true);
            }
        }
        private RelayCommand deleteTicketCommand { get; set; }
        public RelayCommand DeleteTicketCommand
        {
            get
            {
                return deleteTicketCommand ??= new RelayCommand(async obj =>
                {
                       Tickets.Remove(SelectedTicket);
                       SelectedTicket = null;
                }, obj => SelectedTicket!=null);
            }
        }
        private RelayCommand saveTicketCommand { get; set; }
        public RelayCommand SaveTicketCommand
        {
            get
            {
                return saveTicketCommand ??= new RelayCommand(async obj =>
                {
                    await Task.WhenAll(
                        db.DeleteAllRowsDb("Tickets")
                    );

                    foreach (Ticket ticket in Tickets)
                    {
                        await db.AddDBTicketsAsync(ticket);
                    }

                    MessageBox.Show("Данные сохранены!");
                }, obj => Tickets != null);
            }
        }
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

        private Ticket selectedTicket;
        public Ticket SelectedTicket
        {
            get => selectedTicket;
            set
            {
                selectedTicket = value;
                OnPropertyChanged(nameof(SelectedTicket));
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

        public AdminPageViewModel()
        {
            ColumnOptions = new ObservableCollection<ColumnOption>
            {
                new ColumnOption { DisplayName = "Номер заявки", ColumnTag = "number_ticket" },
                new ColumnOption { DisplayName = "Дата создания", ColumnTag = "date" },
                new ColumnOption { DisplayName = "Поломка", ColumnTag = "causeby" },
                new ColumnOption { DisplayName = "Тип поломки", ColumnTag = "typeofcause" },
                new ColumnOption { DisplayName = "Ответственный", ColumnTag = "responsible" },
                new ColumnOption { DisplayName = "Клиент", ColumnTag = "client_id" },
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
