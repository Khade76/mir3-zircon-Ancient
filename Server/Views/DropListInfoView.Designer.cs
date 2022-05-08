namespace Server.Views
{
    partial class DropListInfoView
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.DropsView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ItemLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colChance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDropSet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPartOnly = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEasterEvent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DropInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.dropListInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Monsters = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMonster1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonsterLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colMultiplier1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DropInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDropTier = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SavingButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.DropListLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.DropsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DropInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropListInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Monsters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DropInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DropListLookUpEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // DropsView
            // 
            this.DropsView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItem,
            this.colChance,
            this.colAmount,
            this.colDropSet,
            this.colPartOnly,
            this.colEasterEvent});
            this.DropsView.GridControl = this.DropInfoGridControl;
            this.DropsView.Name = "DropsView";
            this.DropsView.OptionsView.EnableAppearanceEvenRow = true;
            this.DropsView.OptionsView.EnableAppearanceOddRow = true;
            this.DropsView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.DropsView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.DropsView.OptionsView.ShowGroupPanel = false;
            // 
            // colItem
            // 
            this.colItem.ColumnEdit = this.ItemLookUpEdit;
            this.colItem.FieldName = "Item";
            this.colItem.Name = "colItem";
            this.colItem.Visible = true;
            this.colItem.VisibleIndex = 0;
            // 
            // ItemLookUpEdit
            // 
            this.ItemLookUpEdit.AutoHeight = false;
            this.ItemLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.ItemLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ItemLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItemName", "Item Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItemType", "Item Type"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Price", "Price"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StackSize", "Stack Size")});
            this.ItemLookUpEdit.DisplayMember = "ItemName";
            this.ItemLookUpEdit.Name = "ItemLookUpEdit";
            this.ItemLookUpEdit.NullText = "[Item is null]";
            // 
            // colChance
            // 
            this.colChance.FieldName = "Chance";
            this.colChance.Name = "colChance";
            this.colChance.Visible = true;
            this.colChance.VisibleIndex = 1;
            // 
            // colAmount
            // 
            this.colAmount.FieldName = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.Visible = true;
            this.colAmount.VisibleIndex = 2;
            // 
            // colDropSet
            // 
            this.colDropSet.FieldName = "DropSet";
            this.colDropSet.Name = "colDropSet";
            this.colDropSet.Visible = true;
            this.colDropSet.VisibleIndex = 3;
            // 
            // colPartOnly
            // 
            this.colPartOnly.FieldName = "PartOnly";
            this.colPartOnly.Name = "colPartOnly";
            this.colPartOnly.Visible = true;
            this.colPartOnly.VisibleIndex = 4;
            // 
            // colEasterEvent
            // 
            this.colEasterEvent.FieldName = "EasterEvent";
            this.colEasterEvent.Name = "colEasterEvent";
            this.colEasterEvent.Visible = true;
            this.colEasterEvent.VisibleIndex = 5;
            // 
            // DropInfoGridControl
            // 
            this.DropInfoGridControl.DataSource = this.dropListInfoBindingSource;
            this.DropInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.LevelTemplate = this.DropsView;
            gridLevelNode1.RelationName = "Drops";
            gridLevelNode2.LevelTemplate = this.Monsters;
            gridLevelNode2.RelationName = "DropLists";
            this.DropInfoGridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.DropInfoGridControl.Location = new System.Drawing.Point(0, 147);
            this.DropInfoGridControl.MainView = this.DropInfoGridView;
            this.DropInfoGridControl.MenuManager = this.ribbon;
            this.DropInfoGridControl.Name = "DropInfoGridControl";
            this.DropInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.MonsterLookUpEdit,
            this.ItemLookUpEdit,
            this.DropListLookUpEdit});
            this.DropInfoGridControl.Size = new System.Drawing.Size(708, 436);
            this.DropInfoGridControl.TabIndex = 1;
            this.DropInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.Monsters,
            this.DropInfoGridView,
            this.DropsView});
            this.DropInfoGridControl.Click += new System.EventHandler(this.DropInfoGridControl_Click);
            // 
            // dropListInfoBindingSource
            // 
            this.dropListInfoBindingSource.DataSource = typeof(Library.SystemModels.DropListInfo);
            // 
            // Monsters
            // 
            this.Monsters.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMonster1,
            this.colMultiplier1});
            this.Monsters.GridControl = this.DropInfoGridControl;
            this.Monsters.Name = "Monsters";
            this.Monsters.OptionsView.EnableAppearanceEvenRow = true;
            this.Monsters.OptionsView.EnableAppearanceOddRow = true;
            this.Monsters.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.Monsters.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.Monsters.OptionsView.ShowGroupPanel = false;
            // 
            // colMonster1
            // 
            this.colMonster1.ColumnEdit = this.MonsterLookUpEdit;
            this.colMonster1.FieldName = "Monster";
            this.colMonster1.Name = "colMonster1";
            this.colMonster1.Visible = true;
            this.colMonster1.VisibleIndex = 0;
            // 
            // MonsterLookUpEdit
            // 
            this.MonsterLookUpEdit.AutoHeight = false;
            this.MonsterLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.MonsterLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MonsterLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonsterName", "Monster Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AI", "AI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Level", "Level"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Experience", "Experience"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "Is Boss")});
            this.MonsterLookUpEdit.DisplayMember = "MonsterName";
            this.MonsterLookUpEdit.Name = "MonsterLookUpEdit";
            this.MonsterLookUpEdit.NullText = "[Monster is null]";
            // 
            // colMultiplier1
            // 
            this.colMultiplier1.FieldName = "Multiplier";
            this.colMultiplier1.Name = "colMultiplier1";
            this.colMultiplier1.Visible = true;
            this.colMultiplier1.VisibleIndex = 1;
            // 
            // DropInfoGridView
            // 
            this.DropInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDropTier});
            this.DropInfoGridView.GridControl = this.DropInfoGridControl;
            this.DropInfoGridView.Name = "DropInfoGridView";
            this.DropInfoGridView.OptionsDetail.AllowExpandEmptyDetails = true;
            this.DropInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.DropInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.DropInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.DropInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.DropInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colDropTier
            // 
            this.colDropTier.FieldName = "DropTier";
            this.colDropTier.Name = "colDropTier";
            this.colDropTier.Visible = true;
            this.colDropTier.VisibleIndex = 0;
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.SavingButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 2;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.Size = new System.Drawing.Size(708, 147);
            // 
            // SavingButton
            // 
            this.SavingButton.Caption = "Save Database";
            this.SavingButton.Id = 1;
            this.SavingButton.LargeWidth = 60;
            this.SavingButton.Name = "SavingButton";
            this.SavingButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SavingButton_ItemClick);
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
            this.ribbonPageGroup1.ItemLinks.Add(this.SavingButton);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "Saving";
            // 
            // DropListLookUpEdit
            // 
            this.DropListLookUpEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.DropListLookUpEdit.AutoHeight = false;
            this.DropListLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.DropListLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DropListLookUpEdit.DisplayMember = "DropList";
            this.DropListLookUpEdit.Name = "DropListLookUpEdit";
            this.DropListLookUpEdit.NullText = "[DropList is null]";
            // 
            // DropListInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 583);
            this.Controls.Add(this.DropInfoGridControl);
            this.Controls.Add(this.ribbon);
            this.Name = "DropListInfoView";
            this.Ribbon = this.ribbon;
            this.Text = "Drop Info View";
            ((System.ComponentModel.ISupportInitialize)(this.DropsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DropInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropListInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Monsters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DropInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DropListLookUpEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SavingButton;
        private DevExpress.XtraGrid.GridControl DropInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DropInfoGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ItemLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit DropListLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView DropsView;
        private System.Windows.Forms.BindingSource dropListInfoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colItem;
        private DevExpress.XtraGrid.Columns.GridColumn colChance;
        private DevExpress.XtraGrid.Columns.GridColumn colAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colDropSet;
        private DevExpress.XtraGrid.Columns.GridColumn colPartOnly;
        private DevExpress.XtraGrid.Columns.GridColumn colEasterEvent;
        private DevExpress.XtraGrid.Columns.GridColumn colDropTier;
        private DevExpress.XtraGrid.Views.Grid.GridView Monsters;
        private DevExpress.XtraGrid.Columns.GridColumn colMonster1;
        private DevExpress.XtraGrid.Columns.GridColumn colMultiplier1;
    }
}