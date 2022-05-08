namespace Server.Views
{
    partial class QuestTaskView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestTaskView));
            this.MonsterDetailsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonsterInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MapInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QuestInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.questTaskBindingSource = new System.Windows.Forms.BindingSource();
            this.QuestInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colQuest = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QuestInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colTask = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemParameter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ItemInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMobDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SaveButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.RequirementImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.TaskImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.NPCLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.RequiredClassImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterDetailsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.questTaskBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequirementImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NPCLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequiredClassImageComboBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MonsterDetailsGridView
            // 
            this.MonsterDetailsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15});
            this.MonsterDetailsGridView.GridControl = this.QuestInfoGridControl;
            this.MonsterDetailsGridView.Name = "MonsterDetailsGridView";
            this.MonsterDetailsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.MonsterDetailsGridView.OptionsView.EnableAppearanceOddRow = true;
            this.MonsterDetailsGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.MonsterDetailsGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.MonsterDetailsGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn11
            // 
            this.gridColumn11.ColumnEdit = this.MonsterInfoLookUpEdit;
            this.gridColumn11.FieldName = "Monster";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 0;
            // 
            // MonsterInfoLookUpEdit
            // 
            this.MonsterInfoLookUpEdit.AutoHeight = false;
            this.MonsterInfoLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.MonsterInfoLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MonsterInfoLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonsterName", "Monster Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AI", "AI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Level", "Level"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Experience", "Experience"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "IsBoss")});
            this.MonsterInfoLookUpEdit.DisplayMember = "MonsterName";
            this.MonsterInfoLookUpEdit.Name = "MonsterInfoLookUpEdit";
            this.MonsterInfoLookUpEdit.NullText = "[Monster is null]";
            // 
            // gridColumn12
            // 
            this.gridColumn12.ColumnEdit = this.MapInfoLookUpEdit;
            this.gridColumn12.FieldName = "Map";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 1;
            // 
            // MapInfoLookUpEdit
            // 
            this.MapInfoLookUpEdit.AutoHeight = false;
            this.MapInfoLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.MapInfoLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MapInfoLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FileName", "File Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description")});
            this.MapInfoLookUpEdit.DisplayMember = "Description";
            this.MapInfoLookUpEdit.Name = "MapInfoLookUpEdit";
            this.MapInfoLookUpEdit.NullText = "[Map is null]";
            // 
            // gridColumn13
            // 
            this.gridColumn13.FieldName = "Chance";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 2;
            // 
            // gridColumn14
            // 
            this.gridColumn14.FieldName = "Amount";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 3;
            // 
            // gridColumn15
            // 
            this.gridColumn15.FieldName = "DropSet";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 4;
            // 
            // QuestInfoGridControl
            // 
            this.QuestInfoGridControl.DataSource = this.questTaskBindingSource;
            this.QuestInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.LevelTemplate = this.MonsterDetailsGridView;
            gridLevelNode1.RelationName = "MonsterDetails";
            this.QuestInfoGridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.QuestInfoGridControl.Location = new System.Drawing.Point(0, 147);
            this.QuestInfoGridControl.MainView = this.QuestInfoGridView;
            this.QuestInfoGridControl.MenuManager = this.ribbon;
            this.QuestInfoGridControl.Name = "QuestInfoGridControl";
            this.QuestInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.QuestInfoLookUpEdit,
            this.RequirementImageComboBox,
            this.TaskImageComboBox,
            this.ItemInfoLookUpEdit,
            this.MonsterInfoLookUpEdit,
            this.MapInfoLookUpEdit,
            this.NPCLookUpEdit,
            this.RequiredClassImageComboBox});
            this.QuestInfoGridControl.Size = new System.Drawing.Size(865, 353);
            this.QuestInfoGridControl.TabIndex = 2;
            this.QuestInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.QuestInfoGridView,
            this.MonsterDetailsGridView});
            // 
            // questTaskBindingSource
            // 
            this.questTaskBindingSource.DataSource = typeof(Library.SystemModels.QuestTask);
            // 
            // QuestInfoGridView
            // 
            this.QuestInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colQuest,
            this.colTask,
            this.colItemParameter,
            this.colAmount,
            this.colMobDescription});
            this.QuestInfoGridView.GridControl = this.QuestInfoGridControl;
            this.QuestInfoGridView.Name = "QuestInfoGridView";
            this.QuestInfoGridView.OptionsDetail.AllowExpandEmptyDetails = true;
            this.QuestInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.QuestInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.QuestInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.QuestInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.QuestInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colQuest
            // 
            this.colQuest.ColumnEdit = this.QuestInfoLookUpEdit;
            this.colQuest.FieldName = "Quest";
            this.colQuest.Name = "colQuest";
            this.colQuest.Visible = true;
            this.colQuest.VisibleIndex = 0;
            // 
            // QuestInfoLookUpEdit
            // 
            this.QuestInfoLookUpEdit.AutoHeight = false;
            this.QuestInfoLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.QuestInfoLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.QuestInfoLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("QuestName", "Quest Name")});
            this.QuestInfoLookUpEdit.DisplayMember = "QuestName";
            this.QuestInfoLookUpEdit.Name = "QuestInfoLookUpEdit";
            this.QuestInfoLookUpEdit.NullText = "[Quest is null]";
            // 
            // colTask
            // 
            this.colTask.FieldName = "Task";
            this.colTask.Name = "colTask";
            this.colTask.Visible = true;
            this.colTask.VisibleIndex = 1;
            // 
            // colItemParameter
            // 
            this.colItemParameter.ColumnEdit = this.ItemInfoLookUpEdit;
            this.colItemParameter.FieldName = "ItemParameter";
            this.colItemParameter.Name = "colItemParameter";
            this.colItemParameter.Visible = true;
            this.colItemParameter.VisibleIndex = 2;
            // 
            // ItemInfoLookUpEdit
            // 
            this.ItemInfoLookUpEdit.AutoHeight = false;
            this.ItemInfoLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.ItemInfoLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ItemInfoLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItemName", "Item Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItemType", "Item Type"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Price", "Price"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StackSize", "Stack Size")});
            this.ItemInfoLookUpEdit.DisplayMember = "ItemName";
            this.ItemInfoLookUpEdit.Name = "ItemInfoLookUpEdit";
            this.ItemInfoLookUpEdit.NullText = "[Item is null]";
            // 
            // colAmount
            // 
            this.colAmount.FieldName = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.Visible = true;
            this.colAmount.VisibleIndex = 3;
            // 
            // colMobDescription
            // 
            this.colMobDescription.FieldName = "MobDescription";
            this.colMobDescription.Name = "colMobDescription";
            this.colMobDescription.Visible = true;
            this.colMobDescription.VisibleIndex = 4;
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.SaveButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 2;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.Size = new System.Drawing.Size(865, 147);
            // 
            // SaveButton
            // 
            this.SaveButton.Caption = "Save Database";
            this.SaveButton.Id = 1;
            this.SaveButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.ImageOptions.Image")));
            this.SaveButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("SaveButton.ImageOptions.LargeImage")));
            this.SaveButton.LargeWidth = 60;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveButton_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.ItemLinks.Add(this.SaveButton);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "Saving";
            // 
            // RequirementImageComboBox
            // 
            this.RequirementImageComboBox.AutoHeight = false;
            this.RequirementImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RequirementImageComboBox.Name = "RequirementImageComboBox";
            // 
            // TaskImageComboBox
            // 
            this.TaskImageComboBox.AutoHeight = false;
            this.TaskImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TaskImageComboBox.Name = "TaskImageComboBox";
            // 
            // NPCLookUpEdit
            // 
            this.NPCLookUpEdit.AutoHeight = false;
            this.NPCLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.NPCLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NPCLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCName", "NPC Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("RegionName", "Region Name")});
            this.NPCLookUpEdit.DisplayMember = "RegionName";
            this.NPCLookUpEdit.Name = "NPCLookUpEdit";
            this.NPCLookUpEdit.NullText = "[NPC is null]";
            // 
            // RequiredClassImageComboBox
            // 
            this.RequiredClassImageComboBox.AutoHeight = false;
            this.RequiredClassImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RequiredClassImageComboBox.Name = "RequiredClassImageComboBox";
            // 
            // QuestTaskView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 500);
            this.Controls.Add(this.QuestInfoGridControl);
            this.Controls.Add(this.ribbon);
            this.Name = "QuestTaskView";
            this.Ribbon = this.ribbon;
            this.Text = "QuestTaskView";
            ((System.ComponentModel.ISupportInitialize)(this.MonsterDetailsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.questTaskBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequirementImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NPCLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequiredClassImageComboBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private DevExpress.XtraGrid.GridControl QuestInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView QuestInfoGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox RequirementImageComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit QuestInfoLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox TaskImageComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ItemInfoLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView MonsterDetailsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterInfoLookUpEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MapInfoLookUpEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit NPCLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox RequiredClassImageComboBox;
        private System.Windows.Forms.BindingSource questTaskBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colQuest;
        private DevExpress.XtraGrid.Columns.GridColumn colTask;
        private DevExpress.XtraGrid.Columns.GridColumn colItemParameter;
        private DevExpress.XtraGrid.Columns.GridColumn colAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colMobDescription;
    }
}