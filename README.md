# Spreadsheet Data Entry Form example

Platform: WPF  
User story: payroll calculator

This example demonstrate ability to use Spreadsheet Control as data entry form.

Step by step:  
* Create document template (PayrollCalculatorTemplate.xlsx), apply worksheet protection (password is 123), protect workbook structure
* Create PayrollModel.cs - entity class, implement INotifyPropertyChanged, add properties (EmployeeName, RegularHoursWorked, etc.)
* Add spreadsheet control to main form
* Add code which load document template, bind custom cell inplace editors, create custom editors at the beginning of editing cell
* Implement data navigator
* Create PayrollData.cs - payroll with sample data
* Use SpreadsheetBindingManager component to bind data source properties to cells
* Assign binding and data context
 
