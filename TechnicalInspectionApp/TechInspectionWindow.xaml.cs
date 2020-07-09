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
    /// Логика взаимодействия для TechInspectionWindow.xaml
    /// </summary>
    public partial class TechInspectionWindow : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        DriverRepository driverRep;
        CarRepository carRep;
        TechInspectionRep techInspectionRep;
        public TechInspectionWindow()
        {
            InitializeComponent();
            DataContext = this;
            driverRep = new DriverRepository();
            carRep = new CarRepository();
            techInspectionRep = new TechInspectionRep();
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }
        private List<Driver> _drivers;
        public List<Driver> Drivers
        {
            get
            {
                _drivers = driverRep.GetDrivers().Where(x => x.TechInspections.Count == 0 || x.TechInspections?.LastOrDefault().Blocked == false).ToList();
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
                _cars = carRep.GetCars().Where(x => x.TechInspections.Count == 0 || x.TechInspections?.LastOrDefault().Blocked == false).ToList();
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

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      AddTechInspection();
                      MessageBox.Show("Успешно сформировано", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                      Close();
                  }, (obj) => errors.Count == 0));
            }
        }

        private void AddTechInspection()
        {
            DateTime dtNow = DateTime.Now;
            TechInspection techInspection = new TechInspection
            {
                Date = new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, dtNow.Hour, dtNow.Minute, 0),
                DriverId = SelectedDriver.DriverId,
                CarId = SelectedCar.CarId,
                Blocked = true
            };
            techInspectionRep.AddTechInspection(techInspection);
        }

        public void ResetFields()
        {
            Date = null;
            SelectedDriver = null;
            SelectedCar = null;
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

        #region Validation

        private Dictionary<String, List<String>> errors = new Dictionary<string, List<string>>();
        public void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                errors[propertyName].Insert(0, error);
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) &&
                errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0) errors.Remove(propertyName);
            }
        }

        public string Error
        {
            get
            {
                return this[string.Empty];
            }
        }

        public string this[string propertyName]
        {
            get
            {
                propertyName = propertyName ?? string.Empty;
                switch (propertyName)
                {
                    case "Date":
                        if (Date == null)
                        {
                            AddError("Date", "Необходимо выбрать дату");
                        }
                        else
                        {
                            RemoveError("Date", "Необходимо выбрать дату");
                        }
                        break;
                    case "SelectedDriver":
                        if (SelectedDriver == null)
                        {
                            AddError("SelectedDriver", "Необходимо выбрать водителя");
                        }
                        else
                        {
                            RemoveError("SelectedDriver", "Необходимо выбрать водителя");
                        }
                        break;
                    case "SelectedCar":
                        if (SelectedCar == null)
                        {
                            AddError("SelectedCar", "Необходимо выбрать автомобиль");
                        }
                        else
                        {
                            RemoveError("SelectedCar", "Необходимо выбрать автомобиль");
                        }
                        break;
                }
                return (!errors.ContainsKey(propertyName) ? null :
                        String.Join(Environment.NewLine, errors[propertyName]));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
