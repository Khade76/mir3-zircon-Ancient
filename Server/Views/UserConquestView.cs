using DevExpress.XtraGrid.Views.Grid;
using Server.Envir;

namespace Server.Views
{
    public partial class UserConquestView : DevExpress.XtraEditors.XtraForm
    {
        public UserConquestView()
        {
            InitializeComponent();

            UserDropGridControl.DataSource = SEnvir.UserConquestList?.Binding;


            UserDropGridView.OptionsSelection.MultiSelect = true;
            UserDropGridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
        }
    }
}