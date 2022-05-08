namespace Server.Views
{
    partial class CastleInfoView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CastleInfoView));
            this.CastleInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.CastleInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MapLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RegionLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.outpost1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outpost2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outpost3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outpost4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outpost5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ItemLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonsterLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.OutPostMon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.OutPostGuard1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OutPostGuard2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SaveButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.CastleInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CastleInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegionLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            this.SuspendLayout();
            // 
            // CastleInfoGridControl
            // 
            this.CastleInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CastleInfoGridControl.Location = new System.Drawing.Point(0, 143);
            this.CastleInfoGridControl.MainView = this.CastleInfoGridView;
            this.CastleInfoGridControl.MenuManager = this.ribbon;
            this.CastleInfoGridControl.Name = "CastleInfoGridControl";
            this.CastleInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.MonsterLookUpEdit,
            this.RegionLookUpEdit,
            this.MapLookUpEdit,
            this.repositoryItemTextEdit1,
            this.ItemLookUpEdit});
            this.CastleInfoGridControl.ShowOnlyPredefinedDetails = true;
            this.CastleInfoGridControl.Size = new System.Drawing.Size(744, 382);
            this.CastleInfoGridControl.TabIndex = 2;
            this.CastleInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CastleInfoGridView});
            this.CastleInfoGridControl.Click += new System.EventHandler(this.CastleInfoGridControl_Click);
            // 
            // CastleInfoGridView
            // 
            this.CastleInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.outpost1,
            this.outpost2,
            this.outpost3,
            this.outpost4,
            this.outpost5,
            this.gridColumn6,
            this.gridColumn9,
            this.gridColumn7,
            this.OutPostMon,
            this.gridColumn8,
            this.OutPostGuard1,
            this.OutPostGuard2});
            this.CastleInfoGridView.GridControl = this.CastleInfoGridControl;
            this.CastleInfoGridView.Name = "CastleInfoGridView";
            this.CastleInfoGridView.OptionsDetail.AllowExpandEmptyDetails = true;
            this.CastleInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.CastleInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.CastleInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.CastleInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.CastleInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.ColumnEdit = this.MapLookUpEdit;
            this.gridColumn2.FieldName = "Map";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // MapLookUpEdit
            // 
            this.MapLookUpEdit.AutoHeight = false;
            this.MapLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.MapLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MapLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FileName", "File Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description")});
            this.MapLookUpEdit.DisplayMember = "Description";
            this.MapLookUpEdit.Name = "MapLookUpEdit";
            this.MapLookUpEdit.NullText = "[Map is null]";
            // 
            // gridColumn3
            // 
            this.gridColumn3.FieldName = "StartTime";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "Duration";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.ColumnEdit = this.RegionLookUpEdit;
            this.gridColumn5.FieldName = "CastleRegion";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // RegionLookUpEdit
            // 
            this.RegionLookUpEdit.AutoHeight = false;
            this.RegionLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.RegionLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RegionLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ServerDescription", "Description")});
            this.RegionLookUpEdit.DisplayMember = "ServerDescription";
            this.RegionLookUpEdit.Name = "RegionLookUpEdit";
            this.RegionLookUpEdit.NullText = "[Region is null]";
            // 
            // outpost1
            // 
            this.outpost1.ColumnEdit = this.RegionLookUpEdit;
            this.outpost1.FieldName = "OutPost1";
            this.outpost1.Name = "outpost1";
            this.outpost1.Visible = true;
            this.outpost1.VisibleIndex = 5;
            // 
            // outpost2
            // 
            this.outpost2.ColumnEdit = this.RegionLookUpEdit;
            this.outpost2.FieldName = "OutPost2";
            this.outpost2.Name = "outpost2";
            this.outpost2.Visible = true;
            this.outpost2.VisibleIndex = 6;
            // 
            // outpost3
            // 
            this.outpost3.ColumnEdit = this.RegionLookUpEdit;
            this.outpost3.FieldName = "OutPost3";
            this.outpost3.Name = "outpost3";
            this.outpost3.Visible = true;
            this.outpost3.VisibleIndex = 7;
            // 
            // outpost4
            // 
            this.outpost4.ColumnEdit = this.RegionLookUpEdit;
            this.outpost4.FieldName = "OutPost4";
            this.outpost4.Name = "outpost4";
            this.outpost4.Visible = true;
            this.outpost4.VisibleIndex = 8;
            // 
            // outpost5
            // 
            this.outpost5.ColumnEdit = this.RegionLookUpEdit;
            this.outpost5.FieldName = "OutPost5";
            this.outpost5.Name = "outpost5";
            this.outpost5.Visible = true;
            this.outpost5.VisibleIndex = 9;
            // 
            // gridColumn6
            // 
            this.gridColumn6.ColumnEdit = this.RegionLookUpEdit;
            this.gridColumn6.FieldName = "AttackSpawnRegion";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 10;
            // 
            // gridColumn9
            // 
            this.gridColumn9.ColumnEdit = this.ItemLookUpEdit;
            this.gridColumn9.FieldName = "Item";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 11;
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
            // gridColumn7
            // 
            this.gridColumn7.ColumnEdit = this.MonsterLookUpEdit;
            this.gridColumn7.FieldName = "Monster";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 12;
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
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "IsBoss")});
            this.MonsterLookUpEdit.DisplayMember = "MonsterName";
            this.MonsterLookUpEdit.Name = "MonsterLookUpEdit";
            this.MonsterLookUpEdit.NullText = "[Monster is Null]";
            // 
            // OutPostMon
            // 
            this.OutPostMon.Caption = "OutPostMonster";
            this.OutPostMon.ColumnEdit = this.MonsterLookUpEdit;
            this.OutPostMon.FieldName = "OutPostMon";
            this.OutPostMon.Name = "OutPostMon";
            this.OutPostMon.Visible = true;
            this.OutPostMon.VisibleIndex = 13;
            // 
            // gridColumn8
            // 
            this.gridColumn8.ColumnEdit = this.repositoryItemTextEdit1;
            this.gridColumn8.FieldName = "Discount";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 16;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // OutPostGuard1
            // 
            this.OutPostGuard1.Caption = "Guard Melee";
            this.OutPostGuard1.ColumnEdit = this.MonsterLookUpEdit;
            this.OutPostGuard1.FieldName = "OutPostGuard1";
            this.OutPostGuard1.Name = "OutPostGuard1";
            this.OutPostGuard1.Visible = true;
            this.OutPostGuard1.VisibleIndex = 15;
            // 
            // OutPostGuard2
            // 
            this.OutPostGuard2.Caption = "Guard Range";
            this.OutPostGuard2.ColumnEdit = this.MonsterLookUpEdit;
            this.OutPostGuard2.FieldName = "OutPostGuard2";
            this.OutPostGuard2.Name = "OutPostGuard2";
            this.OutPostGuard2.Visible = true;
            this.OutPostGuard2.VisibleIndex = 14;
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
            this.ribbon.Size = new System.Drawing.Size(744, 143);
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
            // CastleInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 525);
            this.Controls.Add(this.CastleInfoGridControl);
            this.Controls.Add(this.ribbon);
            this.Name = "CastleInfoView";
            this.Ribbon = this.ribbon;
            this.Text = "Castle Info";
            ((System.ComponentModel.ISupportInitialize)(this.CastleInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CastleInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegionLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private DevExpress.XtraGrid.GridControl CastleInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CastleInfoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit RegionLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MapLookUpEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ItemLookUpEdit;
        private DevExpress.XtraGrid.Columns.GridColumn outpost1;
        private DevExpress.XtraGrid.Columns.GridColumn outpost2;
        private DevExpress.XtraGrid.Columns.GridColumn outpost3;
        private DevExpress.XtraGrid.Columns.GridColumn outpost4;
        private DevExpress.XtraGrid.Columns.GridColumn outpost5;
        private DevExpress.XtraGrid.Columns.GridColumn outpostMon;
        private DevExpress.XtraGrid.Columns.GridColumn OutPostMon;
        private DevExpress.XtraGrid.Columns.GridColumn OutPostGuard1;
        private DevExpress.XtraGrid.Columns.GridColumn OutPostGuard2;
    }
}