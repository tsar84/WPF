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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TechnicalInspectionApp.Model.Entities;
using TechnicalInspectionApp.Model.Repositories;

namespace TechnicalInspectionApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        DispatcherTimer timer;
        TechInspectionRep techInspectionRep;
        ReportRepository reportRep;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            techInspectionRep = new TechInspectionRep();
            reportRep = new ReportRepository();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ChangeStatus();
            if (Status.Length > 0)
            {
                MessageBox.Show($"{Status.ToString()}", "Статус", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateData();
            }
            Status.Clear();
        }

        #region Check
        private StringBuilder Status = new StringBuilder();
        private void ChangeStatus()
        {
            var techInspections = techInspectionRep.GetTechInspections();
            DateTime dtNow = DateTime.Now;
            foreach (var techInspection in techInspections)
            {
                if ((int)(dtNow - techInspection.Driver.DriverLicenseEndDate).TotalDays > 0)
                {
                    Status.Append(StatusType.DriverLicense + Environment.NewLine);
                    if (!SaveDataReport(techInspection, StatusType.DriverLicense))
                    {
                        Status.Clear();
                    }
                }
                if ((int)(dtNow - techInspection.Car.TechnicalInspectionEndDate).TotalDays > 0)
                {
                    Status.Append(StatusType.TechnicalInspection + Environment.NewLine);
                    if (!SaveDataReport(techInspection, StatusType.TechnicalInspection))
                    {
                        Status.Clear();
                    }
                }
                if ((int)(dtNow - techInspection.Car.InsuranseEndDate).TotalDays > 0)
                {
                    Status.Append(StatusType.Insuranse + Environment.NewLine);
                    if (!SaveDataReport(techInspection, StatusType.Insuranse))
                    {
                        Status.Clear();
                    }
                }
                ChangeBlocked(techInspection);
            }
        }
        private void ChangeBlocked(TechInspection techInspection)
        {
            if(techInspection.Blocked == true & (int)(DateTime.Now - techInspection.Date.AddMinutes(5)).TotalMinutes >= 0)
            {
                techInspection.Blocked = false;
                techInspectionRep.Blocked(techInspection);
            }
        }
        private bool SaveDataReport(TechInspection techInspection, string status)
        {
            var report = reportRep.GetReports().
                Where(x => x.TechInspectionId == techInspection.TechInspectionId & x.Status == status).FirstOrDefault();
            if (report == null)
            {
                techInspection.IsExpired = true;
                techInspectionRep.ChangeStatus(techInspection);

               
                reportRep.AddReport(new Report
                {
                    TechInspectionId = techInspection.TechInspectionId,
                    Status = status
                });
                return true;
            }
            return false;
        }
        #endregion

        private List<TechInspection> _techInspections;
        public List<TechInspection> TechInspections
        {
            get
            {
                _techInspections = techInspectionRep.GetTechInspections();
                return _techInspections;
            }
            set
            {
                _techInspections = value;
                OnPropertyChanged("TechInspections");
            }
        }

        private RelayCommand showReportCommand;
        public RelayCommand ShowReportCommand
        {
            get
            {
                return showReportCommand ??
                  (showReportCommand = new RelayCommand(obj =>
                  {
                      ReportWindow reportWindow = new ReportWindow();
                      reportWindow.ShowDialog();
                  }, (obj) => true));
            }
        }
        private RelayCommand showCarsCommand;
        public RelayCommand ShowCarsCommand
        {
            get
            {
                return showCarsCommand ??
                  (showCarsCommand = new RelayCommand(obj =>
                  {
                      CarsWindow carsWindow = new CarsWindow();
                      carsWindow.ShowDialog();
                  }, (obj) => true));
            }
        }
        private RelayCommand showDriversCommand;
        public RelayCommand ShowDriversCommand
        {
            get
            {
                return showDriversCommand ??
                  (showDriversCommand = new RelayCommand(obj =>
                  {
                      DriversWindow driversWindow = new DriversWindow();
                      driversWindow.ShowDialog();
                  }, (obj) => true));
            }
        }
        private RelayCommand techInspectionDriversCommand;
        public RelayCommand TechInspectionDriversCommand
        {
            get
            {
                return techInspectionDriversCommand ??
                  (techInspectionDriversCommand = new RelayCommand(obj =>
                  {
                      TechInspectionWindow techInspectionWindow = new TechInspectionWindow();
                      techInspectionWindow.ShowDialog();
                      UpdateData();
                  }, (obj) => true));
            }
        }

        private void UpdateData()
        {
            _techInspections.Clear();
            TechInspections = techInspectionRep.GetTechInspections();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    #region Converters
    class BgColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool status = (bool)value;
            if (status)
            {
                return new SolidColorBrush(Colors.Red);
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
