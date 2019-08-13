using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly PayrollViewModel payrollViewModel;

        public MainWindow() {
            InitializeComponent();
            LoadDocumentTemplate();
            BindCustomEditors();
            CollectionViewSource payrollViewSource = (CollectionViewSource)FindResource("PayrollViewSource");
            payrollViewModel = new PayrollViewModel(payrollViewSource.View);
            DataContext = payrollViewModel;
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
    }
}
