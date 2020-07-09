using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    public partial class ReportWindow : Window, INotifyPropertyChanged
    {
        ReportRepository reportRep;
        DriverRepository driverRep;
        CarRepository carRep;
        public ReportWindow()
        {
            InitializeComponent();
            reportRep = new ReportRepository();
            driverRep = new DriverRepository();
            carRep = new CarRepository();
            DataContext = this;
        }

        private List<Report> _report;
        public List<Report> Reports
        {
            get
            {
                if (_report == null)
                {
                    _report = reportRep.GetReports();
                }
                return _report;
            }
            set
            {
                _report = value;
                OnPropertyChanged("Reports");
            }
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

        private List<Car> _cars;
        public List<Car> Cars
        {
            get
            {
                _cars = carRep.GetCars();
                return _cars;
            }
            set
            {
                _cars = value;
                OnPropertyChanged("Cars");
            }
        }

        private Car _selectedCar;
        public Car SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                _selectedCar = value;
                OnPropertyChanged("SelectedCar");
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        private RelayCommand filterCommand;
        public RelayCommand FilterCommand
        {
            get
            {
                return filterCommand ??
                  (filterCommand = new RelayCommand(obj =>
                  {
                      var reports = reportRep.GetReports();
                      if (SelectedDriver != null)
                      {
                          reports = reports.Where(x => x.TechInspection.DriverId == SelectedDriver.DriverId).ToList();
                      }
                      if (SelectedCar != null)
                      {
                          reports = reports.Where(x => x.TechInspection.CarId == SelectedCar.CarId).ToList();
                      }
                      if(StartDate != null & EndDate != null)
                      {
                          reports = reports.Where(x => x.TechInspection.Date >= StartDate & x.TechInspection.Date <= EndDate).ToList();
                      }
                      Reports = reports;
                  }, (obj) => true));
            }
        }

        private void OnComboboxDriverTextChanged(object sender, RoutedEventArgs e)
        {
            cmbbxDriver.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(cmbbxDriver.ItemsSource);
            cv.Filter = s =>
                ((Driver)s).FIO.IndexOf(cmbbxDriver.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void OnComboboxCarTextChanged(object sender, RoutedEventArgs e)
        {
            cmbbxCar.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(cmbbxCar.ItemsSource);
            cv.Filter = s =>
                ((Car)s).StateNumber.IndexOf(cmbbxCar.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    #region Converters
    class BgReportColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            if (status == StatusType.DriverLicense)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (status == StatusType.Insuranse)
            {
                return new SolidColorBrush(Colors.LightBlue);
            }
            else if (status == StatusType.TechnicalInspection)
            {
                return new SolidColorBrush(Colors.LightYellow);
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }
        public object ConvertBack(object values, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
