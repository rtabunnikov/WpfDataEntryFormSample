using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;

namespace WpfDataEntryFormSample {
    public class SpreadsheetBindingManager {
        private SpreadsheetControl control;
        private object dataSource;
        private object currentItem;
        private ICollectionView collectionView;
        private readonly Dictionary<string, string> cellBindings = new Dictionary<string, string>();
        private readonly PropertyDescriptorCollection propertyDescriptors = new PropertyDescriptorCollection(null);
        
        public SpreadsheetBindingManager() {
        }

        public SpreadsheetControl Control {
            get => control;
            set {
                if (!ReferenceEquals(control, value)) {
                    if (control != null)
                        control.CellValueChanged -= SpreadsheetControl_CellValueChanged;
                    control = value;
                    if (control != null)
                        control.CellValueChanged += SpreadsheetControl_CellValueChanged;
                }
            }
        }

        public string SheetName { get; set; }

        public object DataSource {
            get => dataSource;
            set {
                if (!ReferenceEquals(dataSource, value)) {
                    Detach();
                    dataSource = value;
                    Attach();
                }
            }
        }

        public void AddBinding(string propertyName, string cellReference) {
            if (cellBindings.ContainsKey(propertyName))
                throw new ArgumentException($"Already has binding to {propertyName} property");
            if (dataSource is IItemProperties provider) {
                var itemProperties = provider.ItemProperties;
                PropertyDescriptor propertyDescriptor = itemProperties.SingleOrDefault(p => p.Name == propertyName)?.Descriptor as PropertyDescriptor;
                if (propertyDescriptor == null)
                    throw new InvalidOperationException($"Unknown { propertyName } property");
                if (currentItem != null)
                    propertyDescriptor.AddValueChanged(currentItem, OnPropertyChanged);
                propertyDescriptors.Add(propertyDescriptor);
            }
            cellBindings.Add(propertyName, cellReference);
        }

        public void RemoveBinding(string propertyName) {
            if (cellBindings.ContainsKey(propertyName)) {
                PropertyDescriptor propertyDescriptor = propertyDescriptors[propertyName];
                if (currentItem != null)
                    propertyDescriptor.RemoveValueChanged(currentItem, OnPropertyChanged);
                propertyDescriptors.Remove(propertyDescriptor);
                cellBindings.Remove(propertyName);
            }
        }

        public void ClearBindings() {
            UnsubscribePropertyChanged();
            propertyDescriptors.Clear();
            cellBindings.Clear();
        }

        private void Attach() {
            collectionView = dataSource as ICollectionView;
            if (collectionView != null) {
                collectionView.CurrentChanged += CollectionView_CurrentChanged;
                currentItem = collectionView.CurrentItem;
            }
            if (dataSource is IItemProperties provider) {
                var itemProperties = provider.ItemProperties;
                foreach (string propertyName in cellBindings.Keys) {
                    PropertyDescriptor propertyDescriptor = itemProperties.SingleOrDefault(p=> p.Name == propertyName)?.Descriptor as PropertyDescriptor;
                    if (propertyDescriptor == null)
                        throw new InvalidOperationException($"Unable to get property descriptor for { propertyName } property");
                    propertyDescriptors.Add(propertyDescriptor);
                }
            }
            PullData();
            SubscribePropertyChanged();
        }

        private void Detach() {
            if (dataSource != null) {
                UnsubscribePropertyChanged();
                if (collectionView != null) {
                    collectionView.CurrentChanged -= CollectionView_CurrentChanged;
                    collectionView = null;
                    currentItem = null;
                }
                propertyDescriptors.Clear();
            }
        }

        private void CollectionView_CurrentChanged(object sender, EventArgs e) {
            control?.BeginUpdate();
            try {
                DeactivateCellEditor(DevExpress.XtraSpreadsheet.CellEditorEnterValueMode.ActiveCell);
                UnsubscribePropertyChanged();
                currentItem = collectionView.CurrentItem;
                PullData();
                SubscribePropertyChanged();
                ActivateCellEditor();
            }
            finally {
                control?.EndUpdate();
            }
        }

        private void UnsubscribePropertyChanged() {
            if (currentItem != null) {
                foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
                    propertyDescriptor.RemoveValueChanged(currentItem, OnPropertyChanged);
            }
        }

        private void SubscribePropertyChanged() {
            if (currentItem != null) {
                foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
                    propertyDescriptor.AddValueChanged(currentItem, OnPropertyChanged);
            }
        }

        private void OnPropertyChanged(object sender, EventArgs eventArgs) {
            PropertyDescriptor propertyDescriptor = sender as PropertyDescriptor;
            if (propertyDescriptor != null && currentItem != null) {
                string reference;
                if (cellBindings.TryGetValue(propertyDescriptor.Name, out reference))
                    SetCellValue(reference, CellValue.FromObject(propertyDescriptor.GetValue(currentItem)));
            }
        }

        private void PullData() {
            if (currentItem != null) {
                foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors) {
                    string reference = cellBindings[propertyDescriptor.Name];
                    SetCellValue(reference, CellValue.FromObject(propertyDescriptor.GetValue(currentItem)));
                }
            }
        }

        private void SpreadsheetControl_CellValueChanged(object sender, DevExpress.XtraSpreadsheet.SpreadsheetCellEventArgs e) {
            if (e.SheetName == SheetName) {
                string reference = e.Cell.GetReferenceA1();
                string propertyName = cellBindings.SingleOrDefault(p => p.Value == reference).Key;
                if (!string.IsNullOrEmpty(propertyName)) {
                    PropertyDescriptor propertyDescriptor = propertyDescriptors[propertyName];
                    if (propertyDescriptor != null && currentItem != null)
                        propertyDescriptor.SetValue(currentItem, e.Value.ToObject());
                }
            }
        }

        private Worksheet Sheet =>
            control != null && control.Document.Worksheets.Contains(SheetName) ? control.Document.Worksheets[SheetName] : null;

        private void SetCellValue(string reference, CellValue value) {
            if (Sheet != null) {
                bool reactivateEditor = IsCellEditorActive && reference == Sheet.Selection.GetReferenceA1();
                if (reactivateEditor)
                    DeactivateCellEditor();
                Sheet[reference].Value = value;
                if (reactivateEditor)
                    ActivateCellEditor();
            }
        }

        private bool IsCellEditorActive => control != null && control.IsCellEditorActive;

        private void ActivateCellEditor() {
            var sheet = Sheet;
            if (sheet != null) {
                var editors = sheet.CustomCellInplaceEditors.GetCustomCellInplaceEditors(sheet.Selection);
                if (editors.Count == 1)
                    control.OpenCellEditor(DevExpress.XtraSpreadsheet.CellEditorMode.Edit);
            }
        }

        private void DeactivateCellEditor(DevExpress.XtraSpreadsheet.CellEditorEnterValueMode mode = DevExpress.XtraSpreadsheet.CellEditorEnterValueMode.Cancel) {
            if (IsCellEditorActive)
                control.CloseCellEditor(mode);
        }
    }
}

