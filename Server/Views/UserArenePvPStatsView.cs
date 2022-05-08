using DevExpress.XtraGrid.Views.Grid;
using Server.Envir;

namespace Server.Views
{
    public partial class UserArenPvpStatsView : DevExpress.XtraEditors.XtraForm
    {
        public UserArenPvpStatsView()
        {
            InitializeComponent();

            UserArenaPvPStat.DataSource = SEnvir.UserArenaPvPStatsList?.Binding;


            UserArenaPvPStats.OptionsSelection.MultiSelect = true;
            UserArenaPvPStats.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
        }
    }
}