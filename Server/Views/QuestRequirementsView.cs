using System;
using DevExpress.XtraBars;
using Library;
using Library.SystemModels;

namespace Server.Views
{
    public partial class QuestRequirementsView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public QuestRequirementsView()
        {
            InitializeComponent();

            RequirementImageComboBox.Items.AddEnum<QuestRequirementType>();
            TaskImageComboBox.Items.AddEnum<QuestTaskType>();
            RequiredClassImageComboBox.Items.AddEnum<RequiredClass>();

            QuestInfoGridControl.DataSource = SMain.Session.GetCollection<QuestRequirement>().Binding;

            QuestInfoLookUpEdit.DataSource = SMain.Session.GetCollection<QuestInfo>().Binding;
            ItemInfoLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
            MonsterInfoLookUpEdit.DataSource = SMain.Session.GetCollection<MonsterInfo>().Binding;
            MapInfoLookUpEdit.DataSource = SMain.Session.GetCollection<MapInfo>().Binding;
            NPCLookUpEdit.DataSource = SMain.Session.GetCollection<NPCInfo>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(QuestRequirementsGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}