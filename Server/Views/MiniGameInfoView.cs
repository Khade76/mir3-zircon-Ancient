using System;
using DevExpress.XtraBars;
using Library.SystemModels;

namespace Server.Views
{
    public partial class MiniGameInfoView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MiniGameInfoView()
        {
            InitializeComponent();

            MonsterInfoGridControl.DataSource = SMain.Session.GetCollection<MiniGameInfo>().Binding;

            MapLookUpEdit.DataSource = SMain.Session.GetCollection<MapInfo>().Binding;
            ItemLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
            RegionLookUpEdit.DataSource = SMain.Session.GetCollection<MapRegion>().Binding;
            MonsterLookUpEdit.DataSource = SMain.Session.GetCollection<MonsterInfo>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(MiniGameInfoGridView);
            SMain.SetUpView(RewardsGridView);
            SMain.SetUpView(CTFGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }

        private void MonsterInfoGridControl_Click(object sender, EventArgs e)
        {

        }
    }
}