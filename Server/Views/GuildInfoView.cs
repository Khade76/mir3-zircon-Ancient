using DevExpress.XtraEditors;
using Server.Envir;

namespace Server.Views
{
    public partial class GuildInfoView : XtraForm
    {
        public GuildInfoView()
        {
            InitializeComponent();

            GuildGridControl.DataSource = SEnvir.GuildInfoList?.Binding;
            AccountLookUpEdit.DataSource = SEnvir.AccountInfoList?.Binding;

            GuildGridView.OptionsSelection.MultiSelect = true;
            GuildGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
        }

        private void GuildGridControl_Click(object sender, System.EventArgs e)
        {

        }
    }
}