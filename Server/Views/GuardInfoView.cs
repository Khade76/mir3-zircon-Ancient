using DevExpress.XtraBars;
using System;
using DevExpress.XtraGrid.Views.Grid;
using Library;
using Library.SystemModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Views
{
    public partial class RibbonForm1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonForm1()
        {
            InitializeComponent();
            MonsterLookUpEdit.DataSource = SMain.Session.GetCollection<MonsterInfo>().Binding;
            MapLookUpEdit.DataSource = SMain.Session.GetCollection<MapInfo>().Binding;
            gridControl1.DataSource = SMain.Session.GetCollection<GuardInfo>().Binding;

            DirectionImageComboBox.Items.AddEnum<MirDirection>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(GuardInfoGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}