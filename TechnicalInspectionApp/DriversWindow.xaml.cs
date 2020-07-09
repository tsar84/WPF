using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TechnicalInspectionApp.Model.Entities;
using TechnicalInspectionApp.Model.Repositories;

namespace TechnicalInspectionApp
{
    /// <summary>
    /// Логика взаимодействия для DriversWindow.xaml
    /// </summary>
    public partial class DriversWindow : Window, INotifyPropertyChanged
    {
        DriverRepository driverRep;
        public DriversWindow()
        {
            InitializeComponent();
            driverRep = new DriverRepository();
            DataContext = this;
        }

        private List<Driver> _drivers;
        public List<Driver> Drivers
        {
            get
            {
                _drivers = driverRep.GetDrivers();
                return _drivers;
            }
            set
            {
                _drivers = value;
                OnPropertyChanged("Drivers");
            }
        }
        private Driver _selectedDriver;
        public Driver SelectedDriver
        {
            get
            {
                return _selectedDriver;
            }
            set
            {
                _selectedDriver = value;
                OnPropertyChanged("SelectedDriver");
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      AddDriverWindow addDriverWindow = new AddDriverWindow();
                      addDriverWindow.ShowDialog();
                      UpdateData();
                  }, (obj) => true));
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand(obj =>
                  {
                      AddDriverWindow addDriverWindow = new AddDriverWindow(SelectedDriver);
                      addDriverWindow.ShowDialog();
                      UpdateData();

                  }, (obj) => SelectedDriver != null));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      if (MessageBox.Show($"Вы действительно хотите удалить {SelectedDriver.FIO}?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                      {
                          driverRep.DeleteDriver(SelectedDriver);
                          UpdateData();
                      }
                  }, (obj) => SelectedDriver != null));
            }
        }

        private void UpdateData()
        {
            _drivers.Clear();
            Drivers = driverRep.GetDrivers();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
