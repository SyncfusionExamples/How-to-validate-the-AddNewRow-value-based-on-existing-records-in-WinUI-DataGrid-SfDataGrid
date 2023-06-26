using Microsoft.UI.Xaml;
using Syncfusion.UI.Xaml.Data;
using Syncfusion.UI.Xaml.DataGrid;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DataGridDemo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //Event subscription
            //this.dataGrid.CurrentCellValidating += OnCurrentCellValidating;
            this.dataGrid.RowValidating += OnRowValidating;
        }

        //Event customization
        private void OnRowValidating(object sender, RowValidatingEventArgs e)
        {
            // Check whether the row is in add new row or not
            bool isAddNewRow = dataGrid.IsAddNewIndex(dataGrid.SelectionController.CurrentCellManager.CurrentRowColumnIndex.RowIndex);
            if (!isAddNewRow)
                return;

            // Get the value of the column
            var data = e.RowData.GetType().GetProperty("CustomerName").GetValue(e.RowData);

            // Check whether the value is already existing or not
            var datacontext = (this.dataGrid.DataContext as ViewModel).Orders;
            var listOfNames = datacontext.Where(e => e.CustomerName.Equals(data)).ToList();
            if (listOfNames.Count > 0)
            {                
                e.IsValid = false;
                e.ErrorMessages.Add("CustomerName", "Entered Name is  already existing");
            }

        }

        //Event customization
        private void OnCurrentCellValidating(object sender, CurrentCellValidatingEventArgs args)
        {
            // Check whether the row is in add new row or not
            bool isAddNewRow = dataGrid.IsAddNewIndex(dataGrid.SelectionController.CurrentCellManager.CurrentRowColumnIndex.RowIndex);
            if (!isAddNewRow)
                return;

            // Check whether the column is CustomerName or not
            if (args.Column.MappingName == "CustomerName")
            {
                // Get the value of the cell
                var text = args.NewValue.ToString();
                // Check whether the value is already existing or not
                var datacontext = (this.dataGrid.DataContext as ViewModel).Orders;
                var listOfNames = datacontext.Where(e => e.CustomerName.Equals(text)).ToList();
                if (listOfNames.Count > 0)
                {
                    args.ErrorMessage = "Entered Name is  already existing";
                    args.IsValid = false;
                }
            }
        }        
    }
}