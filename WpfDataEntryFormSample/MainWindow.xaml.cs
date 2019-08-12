using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Settings;
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
            BindCustomEditors();
        }

        private void LoadDocumentTemplate() {
            spreadsheetControl1.LoadDocument("PayrollCalculatorTemplate.xlsx");
            spreadsheetControl1.Document.History.IsEnabled = false;
        }
        private void BindCustomEditors() {
            var sheet = spreadsheetControl1.ActiveWorksheet;
            sheet.CustomCellInplaceEditors.Add(sheet["D8"], CustomCellInplaceEditorType.Custom, "RegularHoursWorked");
            sheet.CustomCellInplaceEditors.Add(sheet["D10"], CustomCellInplaceEditorType.Custom, "VacationHours");
            sheet.CustomCellInplaceEditors.Add(sheet["D12"], CustomCellInplaceEditorType.Custom, "SickHours");
            sheet.CustomCellInplaceEditors.Add(sheet["D14"], CustomCellInplaceEditorType.Custom, "OvertimeHours");
            sheet.CustomCellInplaceEditors.Add(sheet["D16"], CustomCellInplaceEditorType.Custom, "OvertimeRate");
            sheet.CustomCellInplaceEditors.Add(sheet["D22"], CustomCellInplaceEditorType.Custom, "OtherDeduction");
        }

        private BaseEditSettings CreateCustomEditorSettings(string tag) {
            switch (tag) {
                case "RegularHoursWorked": return CreateSpinEditSettings(0, 184, 1);
                case "VacationHours": return CreateSpinEditSettings(0, 184, 1);
                case "SickHours": return CreateSpinEditSettings(0, 184, 1);
                case "OvertimeHours": return CreateSpinEditSettings(0, 100, 1);
                case "OvertimeRate": return CreateSpinEditSettings(0, 50, 1);
                case "OtherDeduction": return CreateSpinEditSettings(0, 100, 1);
                default: return null;
            }
        }

        private SpinEditSettings CreateSpinEditSettings(int minValue, int maxValue, int increment) => new SpinEditSettings {
            HorizontalContentAlignment = EditSettingsHorizontalAlignment.Right,
            MinValue = minValue,
            MaxValue = maxValue,
            Increment = increment,
            IsFloatValue = false
        };

        private void SpreadsheetControl1_CustomCellEdit(object sender, DevExpress.Xpf.Spreadsheet.SpreadsheetCustomCellEditEventArgs e) {
            if (e.ValueObject.IsText)
                e.EditSettings = CreateCustomEditorSettings(e.ValueObject.TextValue);

        }

        private void SpreadsheetControl1_ProtectionWarning(object sender, System.ComponentModel.HandledEventArgs e) => e.Handled = true;

        private void SpreadsheetControl1_SelectionChanged(object sender, EventArgs e) {
            var sheet = spreadsheetControl1.ActiveWorksheet;
            if (sheet != null) {
                var editors = sheet.CustomCellInplaceEditors.GetCustomCellInplaceEditors(sheet.Selection);
                if (editors.Count == 1)
                    spreadsheetControl1.OpenCellEditor(DevExpress.XtraSpreadsheet.CellEditorMode.Edit);
            }
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
