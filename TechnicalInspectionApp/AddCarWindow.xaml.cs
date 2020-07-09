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
    public partial class AddCarWindow : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        CarRepository carRep;
        public AddCarWindow(Car car = null)
        {
            InitializeComponent();
            carRep = new CarRepository();
            DataContext = this;
            if (car != null)
            {
                _carId = car.CarId;
                Title = "Редактирование автомобиля";
                StateNumber = car.StateNumber;
                Mark = car.Mark;
                Model = car.Model;
                TechnicalInspectionEndDate = car.TechnicalInspectionEndDate;
                InsuranseEndDate = car.InsuranseEndDate;
            }
        }

        private int _carId { get; set; } = -1;

        private string _stateNumber;
        public string StateNumber
        {
            get
            {
                return _stateNumber;
            }
            set
            {
                _stateNumber = value;
                OnPropertyChanged("StateNumber");
            }
        }

        private string _mark;
        public string Mark
        {
            get
            {
                return _mark;
            }
            set
            {
                _mark = value;
                OnPropertyChanged("Mark");
            }
        }

        private string _model;
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
        }

        private DateTime? _technicalInspectionEndDate;
        public DateTime? TechnicalInspectionEndDate
        {
            get
            {
                return _technicalInspectionEndDate;
            }
            set
            {
                _technicalInspectionEndDate = value;
                OnPropertyChanged("TechnicalInspectionEndDate");
            }
        }

        private DateTime? _insuranseEndDate;
        public DateTime? InsuranseEndDate
        {
            get
            {
                return _insuranseEndDate;
            }
            set
            {
                _insuranseEndDate = value;
                OnPropertyChanged("InsuranseEndDate");
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
                      AddCar();
                      MessageBox.Show("Автомобиль успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                      ResetFields();
                  }, (obj) => errors.Count == 0));
            }
        }

        private void AddCar()
        {
            Car car = new Car
            {
                CarId = _carId,
                StateNumber = StateNumber,
                Enabled = true,
                Mark = Mark,
                Model = Model,
                TechnicalInspectionEndDate = TechnicalInspectionEndDate.Value,
                InsuranseEndDate = InsuranseEndDate.Value
            };
            carRep.AddCar(car);
        }

        public void ResetFields()
        {
            StateNumber = string.Empty;
            Mark = string.Empty;
            Model = string.Empty;
            TechnicalInspectionEndDate = null;
            InsuranseEndDate = null;
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
                    case "StateNumber":
                        if (string.IsNullOrEmpty(StateNumber))
                        {
                            AddError("StateNumber", "Необходимо заполнить гос. номер");
                        }
                        else
                        {
                            RemoveError("StateNumber", "Необходимо заполнить гос. номер");
                        }
                        break;
                    case "Mark":
                        if (string.IsNullOrEmpty(Mark))
                        {
                            AddError("Mark", "Необходимо заполнить поле марка");
                        }
                        else
                        {
                            RemoveError("Mark", "Необходимо заполнить поле марка");
                        }
                        break;
                    case "Model":
                        if (string.IsNullOrEmpty(Model))
                        {
                            AddError("Model", "Необходимо заполнить поле модель");
                        }
                        else
                        {
                            RemoveError("Model", "Необходимо заполнить поле модель");
                        }
                        break;
                    case "TechnicalInspectionEndDate":
                        if (TechnicalInspectionEndDate == null)
                        {
                            AddError("TechnicalInspectionEndDate", "Необходимо выбрать дату окончания тех. осмотра");
                        }
                        else
                        {
                            RemoveError("TechnicalInspectionEndDate", "Необходимо выбрать дату окончания тех. осмотра");
                        }
                        break;
                    case "InsuranseEndDate":
                        if (InsuranseEndDate == null)
                        {
                            AddError("InsuranseEndDate", "Необходимо выбрать дату окончания страховки");
                        }
                        else
                        {
                            RemoveError("InsuranseEndDate", "Необходимо выбрать дату окончания страховки");
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
