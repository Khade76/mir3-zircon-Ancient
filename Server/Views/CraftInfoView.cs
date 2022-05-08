using System;
using DevExpress.XtraBars;
using Library.SystemModels;

namespace Server.Views
{
    public partial class CraftInfoView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public CraftInfoView()
        {
            InitializeComponent();

            CraftInfoGridControl.DataSource = SMain.Session.GetCollection<CraftItemInfo>().Binding;
            CraftLevelInfoGridControl.DataSource = SMain.Session.GetCollection<CraftLevelInfo>().Binding;

            ItemLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(CraftInfoGridView);
            SMain.SetUpView(CraftLevelInfoGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}