using System;
using DevExpress.XtraBars;
using Library.SystemModels;

namespace Server.Views
{
    public partial class FamePointInfoView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FamePointInfoView()
        {
            InitializeComponent();

            StoreInfoGridControl.DataSource = SMain.Session.GetCollection<FamePointInfo>().Binding;
            ItemLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(StoreInfoGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}