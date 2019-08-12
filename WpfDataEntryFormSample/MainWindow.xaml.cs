using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfDataEntryFormSample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow {
        readonly List<PayrollModel> payrollData = new List<PayrollModel>();

        public MainWindow() {
            InitializeComponent();
            InitializePayrollData();
            LoadDocumentTemplate();
        }

        private void LoadDocumentTemplate() {
            spreadsheetControl1.LoadDocument("PayrollCalculatorTemplate.xlsx");
            spreadsheetControl1.Document.History.IsEnabled = false;
        }

        private void InitializePayrollData() {
            payrollData.Add(new PayrollModel() {
                EmployeeName = "Linda Brown",
                HourlyWages = 10.0,
                RegularHoursWorked = 40,
                VacationHours = 5,
                SickHours = 1,
                OvertimeHours = 0,
                OvertimeRate = 15.0,
                OtherDeduction = 20.0,
                TaxStatus = 1,
                FederalAllowance = 4,
                StateTax = 0.023,
                FederalIncomeTax = 0.28,
                SocialSecurityTax = 0.063,
                MedicareTax = 0.0145,
                InsuranceDeduction = 20.0,
                OtherRegularDeduction = 40.0
            });

            payrollData.Add(new PayrollModel() {
                EmployeeName = "Kate Smith",
                HourlyWages = 11.0,
                RegularHoursWorked = 45,
                VacationHours = 5,
                SickHours = 0,
                OvertimeHours = 3,
                OvertimeRate = 20.0,
                OtherDeduction = 20.0,
                TaxStatus = 1,
                FederalAllowance = 4,
                StateTax = 0.0245,
                FederalIncomeTax = 0.276,
                SocialSecurityTax = 0.061,
                MedicareTax = 0.015,
                InsuranceDeduction = 20.0,
                OtherRegularDeduction = 42.0
            });

            payrollData.Add(new PayrollModel() {
                EmployeeName = "Nick Taylor",
                HourlyWages = 15.0,
                RegularHoursWorked = 40,
                VacationHours = 6,
                SickHours = 2,
                OvertimeHours = 6,
                OvertimeRate = 40.0,
                OtherDeduction = 21.0,
                TaxStatus = 2,
                FederalAllowance = 3,
                StateTax = 0.0301,
                FederalIncomeTax = 0.2702,
                SocialSecurityTax = 0.068,
                MedicareTax = 0.015,
                InsuranceDeduction = 22.0,
                OtherRegularDeduction = 39.0
            });

            payrollData.Add(new PayrollModel() {
                EmployeeName = "Tommy Dickson",
                HourlyWages = 20.0,
                RegularHoursWorked = 40,
                VacationHours = 0,
                SickHours = 0,
                OvertimeHours = 3,
                OvertimeRate = 45.0,
                OtherDeduction = 12.46,
                TaxStatus = 3,
                FederalAllowance = 4,
                StateTax = 0.045,
                FederalIncomeTax = 0.2904,
                SocialSecurityTax = 0.084,
                MedicareTax = 0.0143,
                InsuranceDeduction = 41.4,
                OtherRegularDeduction = 24.3
            });

            payrollData.Add(new PayrollModel() {
                EmployeeName = "Emmy Milton",
                HourlyWages = 32.0,
                RegularHoursWorked = 45,
                VacationHours = 0,
                SickHours = 0,
                OvertimeHours = 5,
                OvertimeRate = 40.0,
                OtherDeduction = 0.0,
                TaxStatus = 2,
                FederalAllowance = 3,
                StateTax = 0.025,
                FederalIncomeTax = 0.28,
                SocialSecurityTax = 0.064,
                MedicareTax = 0.0143,
                InsuranceDeduction = 19.34,
                OtherRegularDeduction = 25.0
            });
        }
    }
}
