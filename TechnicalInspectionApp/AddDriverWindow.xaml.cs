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
    public partial class AddDriverWindow : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        DriverRepository driverRep;
        public AddDriverWindow(Driver driver = null)
        {
            InitializeComponent();
            driverRep = new DriverRepository();
            DataContext = this;
            if(driver != null)
            {
                _driverId = driver.DriverId;
                Title = "Редактирование водителя";
                FIO = driver.FIO;
                DriverLicenseData = driver.DriverLicenseData;
                DriverLicenseEndDate = driver.DriverLicenseEndDate;
            }
        }

        private int _driverId { get; set; } = -1;

        private string _fio;
        public string FIO
        {
            get
            {
                return _fio;
            }
            set
            {
                _fio = value;
                OnPropertyChanged("FIO");
            }
        }
        private string _driverLicenseData;
        public string DriverLicenseData
        {
            get
            {
                return _driverLicenseData;
            }
            set
            {
                _driverLicenseData = value;
                OnPropertyChanged("DriverLicenseData");
            }
        }
        private DateTime? _driverLicenseEndDate;
        public DateTime? DriverLicenseEndDate
        {
            get
            {
                return _driverLicenseEndDate;
            }
            set
            {
                _driverLicenseEndDate = value;
                OnPropertyChanged("DriverLicenseEndDate");
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
                      AddDriver();
                      MessageBox.Show("Водитель успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                      ResetFields();
                  }, (obj) => errors.Count == 0));
            }
        }

        private void AddDriver()
        {
            Driver driver = new Driver
            {
                DriverId = _driverId,
                FIO = FIO,
                Enabled = true,
                DriverLicenseData = DriverLicenseData,
                DriverLicenseEndDate = DriverLicenseEndDate.Value,
            };
            driverRep.AddDriver(driver);
        }

        public void ResetFields()
        {
            FIO = string.Empty;
            DriverLicenseData = string.Empty;
            DriverLicenseEndDate = null;
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
                    case "FIO":
                        if (string.IsNullOrEmpty(FIO))
                        {
                            AddError("FIO", "Необходимо заполнить ФИО водителя");
                        }
                        else
                        {
                            RemoveError("FIO", "Необходимо заполнить ФИО водителя");
                        }
                        break;
                    case "DriverLicenseData":
                        if (string.IsNullOrEmpty(DriverLicenseData))
                        {
                            AddError("DriverLicenseData", "Необходимо заполнить поле серия и номер водит. удостоверения");
                        }
                        else
                        {
                            RemoveError("DriverLicenseData", "Необходимо заполнить поле серия и номер водит. удостоверения");
                        }
                        break;
                    case "DriverLicenseEndDate":
                        if (DriverLicenseEndDate == null)
                        {
                            AddError("DriverLicenseEndDate", "Необходимо выбрать дату окончания вод. удостоверения");
                        }
                        else
                        {
                            RemoveError("DriverLicenseEndDate", "Необходимо выбрать дату окончания вод. удостоверения");
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
