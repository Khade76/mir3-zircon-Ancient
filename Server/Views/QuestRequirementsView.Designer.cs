namespace Server.Views
{
    partial class QuestRequirementsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestRequirementsView));
            this.RequirementImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.QuestInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.RequiredClassImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.QuestInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.TaskImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.ItemInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.MonsterInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.MapInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.QuestRequirementsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.NPCLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SaveButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.questRequirementBindingSource = new System.Windows.Forms.BindingSource();
            this.colRequirement = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuest = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuestParameter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIntParameter1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClass = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.RequirementImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequiredClassImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestRequirementsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NPCLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.questRequirementBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // RequirementImageComboBox
            // 
            this.RequirementImageComboBox.AutoHeight = false;
            this.RequirementImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RequirementImageComboBox.Name = "RequirementImageComboBox";
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
            // RequiredClassImageComboBox
            // 
            this.RequiredClassImageComboBox.AutoHeight = false;
            this.RequiredClassImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RequiredClassImageComboBox.Name = "RequiredClassImageComboBox";
            // 
            // QuestInfoGridControl
            // 
            this.QuestInfoGridControl.DataSource = this.questRequirementBindingSource;
            this.QuestInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestInfoGridControl.Location = new System.Drawing.Point(0, 147);
            this.QuestInfoGridControl.MainView = this.QuestRequirementsGridView;
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
            this.QuestRequirementsGridView});
            // 
            // TaskImageComboBox
            // 
            this.TaskImageComboBox.AutoHeight = false;
            this.TaskImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TaskImageComboBox.Name = "TaskImageComboBox";
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
            // QuestRequirementsGridView
            // 
            this.QuestRequirementsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colQuest,
            this.colRequirement,
            this.colQuestParameter,
            this.colIntParameter1,
            this.colClass});
            this.QuestRequirementsGridView.GridControl = this.QuestInfoGridControl;
            this.QuestRequirementsGridView.Name = "QuestRequirementsGridView";
            this.QuestRequirementsGridView.OptionsDetail.AllowExpandEmptyDetails = true;
            this.QuestRequirementsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.QuestRequirementsGridView.OptionsView.EnableAppearanceOddRow = true;
            this.QuestRequirementsGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.QuestRequirementsGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.QuestRequirementsGridView.OptionsView.ShowGroupPanel = false;
            this.QuestRequirementsGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colRequirement, DevExpress.Data.ColumnSortOrder.Ascending)});
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
            // questRequirementBindingSource
            // 
            this.questRequirementBindingSource.DataSource = typeof(Library.SystemModels.QuestRequirement);
            // 
            // colRequirement
            // 
            this.colRequirement.ColumnEdit = this.RequirementImageComboBox;
            this.colRequirement.FieldName = "Requirement";
            this.colRequirement.Name = "colRequirement";
            this.colRequirement.Visible = true;
            this.colRequirement.VisibleIndex = 1;
            // 
            // colQuest
            // 
            this.colQuest.ColumnEdit = this.QuestInfoLookUpEdit;
            this.colQuest.FieldName = "Quest";
            this.colQuest.Name = "colQuest";
            this.colQuest.Visible = true;
            this.colQuest.VisibleIndex = 0;
            // 
            // colQuestParameter
            // 
            this.colQuestParameter.ColumnEdit = this.QuestInfoLookUpEdit;
            this.colQuestParameter.FieldName = "QuestParameter";
            this.colQuestParameter.Name = "colQuestParameter";
            this.colQuestParameter.Visible = true;
            this.colQuestParameter.VisibleIndex = 2;
            // 
            // colIntParameter1
            // 
            this.colIntParameter1.FieldName = "IntParameter1";
            this.colIntParameter1.Name = "colIntParameter1";
            this.colIntParameter1.Visible = true;
            this.colIntParameter1.VisibleIndex = 3;
            // 
            // colClass
            // 
            this.colClass.ColumnEdit = this.RequiredClassImageComboBox;
            this.colClass.FieldName = "Class";
            this.colClass.Name = "colClass";
            this.colClass.Visible = true;
            this.colClass.VisibleIndex = 4;
            // 
            // QuestRequirementsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 500);
            this.Controls.Add(this.QuestInfoGridControl);
            this.Controls.Add(this.ribbon);
            this.Name = "QuestRequirementsView";
            this.Ribbon = this.ribbon;
            this.Text = "QuestRequirementsView";
            ((System.ComponentModel.ISupportInitialize)(this.RequirementImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequiredClassImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuestRequirementsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NPCLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.questRequirementBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private DevExpress.XtraGrid.GridControl QuestInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView QuestRequirementsGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox RequirementImageComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit QuestInfoLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox TaskImageComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ItemInfoLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterInfoLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MapInfoLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit NPCLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox RequiredClassImageComboBox;
        private System.Windows.Forms.BindingSource questRequirementBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colQuest;
        private DevExpress.XtraGrid.Columns.GridColumn colRequirement;
        private DevExpress.XtraGrid.Columns.GridColumn colQuestParameter;
        private DevExpress.XtraGrid.Columns.GridColumn colIntParameter1;
        private DevExpress.XtraGrid.Columns.GridColumn colClass;
    }
}