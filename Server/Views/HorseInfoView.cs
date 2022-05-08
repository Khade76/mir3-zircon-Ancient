using System;
using DevExpress.XtraBars;
using Library;
using Library.SystemModels;


namespace Server.Views
{
    public partial class HorseInfoView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public HorseInfoView()
        {
            InitializeComponent();


            HorseInfoGridControl.DataSource = SMain.Session.GetCollection<HorseInfo>().Binding;

            MonsterLookUpEdit.DataSource = SMain.Session.GetCollection<MonsterInfo>().Binding;
            RegionLookUpEdit.DataSource = SMain.Session.GetCollection<MapRegion>().Binding;
            MapLookUpEdit.DataSource = SMain.Session.GetCollection<MapInfo>().Binding;
            ItemLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
            HorseTypeImageComboBox.Items.AddEnum<HorseType>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(HorseInfoGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}