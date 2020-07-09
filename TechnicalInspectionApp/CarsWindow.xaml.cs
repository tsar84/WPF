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
    public partial class CarsWindow : Window, INotifyPropertyChanged
    {
        CarRepository carRep = new CarRepository();
        public CarsWindow()
        {
            InitializeComponent();
            carRep = new CarRepository();
            DataContext = this;
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

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      AddCarWindow addCarWindow = new AddCarWindow();
                      addCarWindow.ShowDialog();
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
                      AddCarWindow addCarWindow = new AddCarWindow(SelectedCar);
                      addCarWindow.ShowDialog();
                      UpdateData();
                  }, (obj) => SelectedCar != null));
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
                      if(MessageBox.Show($"Вы действительно хотите удалить {SelectedCar.Model} {SelectedCar.Mark} {SelectedCar.StateNumber}?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                      {
                        carRep.DeleteCar(SelectedCar);
                        UpdateData();
                      }
                  }, (obj) => SelectedCar != null));
            }
        }

        private void UpdateData()
        {
            _cars.Clear();
            Cars = carRep.GetCars();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
