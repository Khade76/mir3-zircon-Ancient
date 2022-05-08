using System;
using DevExpress.XtraBars;
using Library;
using Library.SystemModels;

namespace Server.Views
{
    public partial class QuestTaskView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public QuestTaskView()
        {
            InitializeComponent();

            RequirementImageComboBox.Items.AddEnum<QuestRequirementType>();
            TaskImageComboBox.Items.AddEnum<QuestTaskType>();
            RequiredClassImageComboBox.Items.AddEnum<RequiredClass>();

            QuestInfoGridControl.DataSource = SMain.Session.GetCollection<QuestTask>().Binding;

            QuestInfoLookUpEdit.DataSource = SMain.Session.GetCollection<QuestInfo>().Binding;
            ItemInfoLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
            MonsterInfoLookUpEdit.DataSource = SMain.Session.GetCollection<MonsterInfo>().Binding;
            MapInfoLookUpEdit.DataSource = SMain.Session.GetCollection<MapInfo>().Binding;
            NPCLookUpEdit.DataSource = SMain.Session.GetCollection<NPCInfo>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(QuestInfoGridView);
            SMain.SetUpView(MonsterDetailsGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}