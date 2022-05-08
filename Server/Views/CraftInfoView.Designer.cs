namespace Server.Views
{
    partial class CraftInfoView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CraftInfoView));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SaveButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.CraftInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.CraftInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CraftInfoItemType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ItemLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.CraftInfoItemLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient1Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient2Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient3Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemIngredient4Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemCost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemChance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftInfoItemExp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.CraftLevelInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.CraftLevelInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CraftLevelInfoType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftLevelInfoLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CraftLevelInfoExp = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CraftInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CraftInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).BeginInit();
            this.tabPane1.SuspendLayout();
            this.tabNavigationPage1.SuspendLayout();
            this.tabNavigationPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CraftLevelInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CraftLevelInfoGridView)).BeginInit();
            this.SuspendLayout();
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
            this.ribbon.Size = new System.Drawing.Size(972, 144);
            // 
            // SaveButton
            // 
            this.SaveButton.Caption = "Save Database";
            this.SaveButton.Glyph = ((System.Drawing.Image)(resources.GetObject("SaveButton.Glyph")));
            this.SaveButton.Id = 1;
            this.SaveButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("SaveButton.LargeGlyph")));
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
            // CraftInfoGridControl
            // 
            this.CraftInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CraftInfoGridControl.Location = new System.Drawing.Point(0, 0);
            this.CraftInfoGridControl.MainView = this.CraftInfoGridView;
            this.CraftInfoGridControl.MenuManager = this.ribbon;
            this.CraftInfoGridControl.Name = "CraftInfoGridControl";
            this.CraftInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ItemLookUpEdit});
            this.CraftInfoGridControl.Size = new System.Drawing.Size(954, 355);
            this.CraftInfoGridControl.TabIndex = 2;
            this.CraftInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CraftInfoGridView});
            // 
            // CraftInfoGridView
            // 
            this.CraftInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CraftInfoItemType,
            this.CraftInfoItemItem,
            this.CraftInfoItemLevel,
            this.CraftInfoItemIngredient1,
            this.CraftInfoItemIngredient1Amount,
            this.CraftInfoItemIngredient2,
            this.CraftInfoItemIngredient2Amount,
            this.CraftInfoItemIngredient3,
            this.CraftInfoItemIngredient3Amount,
            this.CraftInfoItemIngredient4,
            this.CraftInfoItemIngredient4Amount,
            this.CraftInfoItemCost,
            this.CraftInfoItemChance,
            this.CraftInfoItemAmount,
            this.CraftInfoItemExp});
            this.CraftInfoGridView.GridControl = this.CraftInfoGridControl;
            this.CraftInfoGridView.Name = "CraftInfoGridView";
            this.CraftInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.CraftInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.CraftInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.CraftInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.CraftInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // CraftInfoItemType
            // 
            this.CraftInfoItemType.FieldName = "Type";
            this.CraftInfoItemType.Name = "CraftInfoItemType";
            this.CraftInfoItemType.Visible = true;
            this.CraftInfoItemType.VisibleIndex = 0;
            // 
            // CraftInfoItemItem
            // 
            this.CraftInfoItemItem.ColumnEdit = this.ItemLookUpEdit;
            this.CraftInfoItemItem.FieldName = "Item";
            this.CraftInfoItemItem.Name = "CraftInfoItemItem";
            this.CraftInfoItemItem.Visible = true;
            this.CraftInfoItemItem.VisibleIndex = 1;
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
            // CraftInfoItemLevel
            // 
            this.CraftInfoItemLevel.FieldName = "Level";
            this.CraftInfoItemLevel.Name = "CraftInfoItemLevel";
            this.CraftInfoItemLevel.Visible = true;
            this.CraftInfoItemLevel.VisibleIndex = 2;
            // 
            // CraftInfoItemIngredient1
            // 
            this.CraftInfoItemIngredient1.ColumnEdit = this.ItemLookUpEdit;
            this.CraftInfoItemIngredient1.FieldName = "Ingredient1";
            this.CraftInfoItemIngredient1.Name = "CraftInfoItemIngredient1";
            this.CraftInfoItemIngredient1.Visible = true;
            this.CraftInfoItemIngredient1.VisibleIndex = 3;
            // 
            // CraftInfoItemIngredient1Amount
            // 
            this.CraftInfoItemIngredient1Amount.FieldName = "Ingredient1Amount";
            this.CraftInfoItemIngredient1Amount.Name = "CraftInfoItemIngredient1Amount";
            this.CraftInfoItemIngredient1Amount.Visible = true;
            this.CraftInfoItemIngredient1Amount.VisibleIndex = 4;
            // 
            // CraftInfoItemIngredient2
            // 
            this.CraftInfoItemIngredient2.ColumnEdit = this.ItemLookUpEdit;
            this.CraftInfoItemIngredient2.FieldName = "Ingredient2";
            this.CraftInfoItemIngredient2.Name = "CraftInfoItemIngredient2";
            this.CraftInfoItemIngredient2.Visible = true;
            this.CraftInfoItemIngredient2.VisibleIndex = 5;
            // 
            // CraftInfoItemIngredient2Amount
            // 
            this.CraftInfoItemIngredient2Amount.FieldName = "Ingredient2Amount";
            this.CraftInfoItemIngredient2Amount.Name = "CraftInfoItemIngredient2Amount";
            this.CraftInfoItemIngredient2Amount.Visible = true;
            this.CraftInfoItemIngredient2Amount.VisibleIndex = 6;
            // 
            // CraftInfoItemIngredient3
            // 
            this.CraftInfoItemIngredient3.ColumnEdit = this.ItemLookUpEdit;
            this.CraftInfoItemIngredient3.FieldName = "Ingredient3";
            this.CraftInfoItemIngredient3.Name = "CraftInfoItemIngredient3";
            this.CraftInfoItemIngredient3.Visible = true;
            this.CraftInfoItemIngredient3.VisibleIndex = 7;
            // 
            // CraftInfoItemIngredient3Amount
            // 
            this.CraftInfoItemIngredient3Amount.FieldName = "Ingredient3Amount";
            this.CraftInfoItemIngredient3Amount.Name = "CraftInfoItemIngredient3Amount";
            this.CraftInfoItemIngredient3Amount.Visible = true;
            this.CraftInfoItemIngredient3Amount.VisibleIndex = 8;
            // 
            // CraftInfoItemIngredient4
            // 
            this.CraftInfoItemIngredient4.ColumnEdit = this.ItemLookUpEdit;
            this.CraftInfoItemIngredient4.FieldName = "Ingredient4";
            this.CraftInfoItemIngredient4.Name = "CraftInfoItemIngredient4";
            this.CraftInfoItemIngredient4.Visible = true;
            this.CraftInfoItemIngredient4.VisibleIndex = 9;
            // 
            // CraftInfoItemIngredient4Amount
            // 
            this.CraftInfoItemIngredient4Amount.FieldName = "Ingredient4Amount";
            this.CraftInfoItemIngredient4Amount.Name = "CraftInfoItemIngredient4Amount";
            this.CraftInfoItemIngredient4Amount.Visible = true;
            this.CraftInfoItemIngredient4Amount.VisibleIndex = 10;
            // 
            // CraftInfoItemCost
            // 
            this.CraftInfoItemCost.FieldName = "Cost";
            this.CraftInfoItemCost.Name = "CraftInfoItemCost";
            this.CraftInfoItemCost.Visible = true;
            this.CraftInfoItemCost.VisibleIndex = 11;
            // 
            // CraftInfoItemAmount
            // 
            this.CraftInfoItemAmount.FieldName = "Amount";
            this.CraftInfoItemAmount.Name = "CraftInfoItemAmount";
            this.CraftInfoItemAmount.Visible = true;
            this.CraftInfoItemAmount.VisibleIndex = 12;
            // 
            // CraftInfoItemChance
            // 
            this.CraftInfoItemChance.FieldName = "Chance";
            this.CraftInfoItemChance.Name = "CraftInfoItemChance";
            this.CraftInfoItemChance.Visible = true;
            this.CraftInfoItemChance.VisibleIndex = 13;
            // 
            // CraftInfoItemExp
            // 
            this.CraftInfoItemExp.FieldName = "Exp";
            this.CraftInfoItemExp.Name = "CraftInfoItemExp";
            this.CraftInfoItemExp.Visible = true;
            this.CraftInfoItemExp.VisibleIndex = 14;
            // 
            // tabPane1
            // 
            this.tabPane1.Controls.Add(this.tabNavigationPage1);
            this.tabPane1.Controls.Add(this.tabNavigationPage2);
            ;
            this.tabPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane1.Location = new System.Drawing.Point(0, 144);
            this.tabPane1.Name = "tabPane1";
            this.tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1,
            this.tabNavigationPage2});
            this.tabPane1.RegularSize = new System.Drawing.Size(972, 400);
            this.tabPane1.SelectedPage = this.tabNavigationPage1;
            this.tabPane1.Size = new System.Drawing.Size(972, 400);
            this.tabPane1.TabIndex = 3;
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "Craft Info";
            this.tabNavigationPage1.Controls.Add(this.CraftInfoGridControl);
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Size = new System.Drawing.Size(954, 355);
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.Caption = "Craft Level Info";
            this.tabNavigationPage2.Controls.Add(this.CraftLevelInfoGridControl);
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            this.tabNavigationPage2.Size = new System.Drawing.Size(960, 358);
            // 
            // CraftLevelInfoGridControl
            // 
            this.CraftLevelInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CraftLevelInfoGridControl.Location = new System.Drawing.Point(0, 0);
            this.CraftLevelInfoGridControl.MainView = this.CraftLevelInfoGridView;
            this.CraftLevelInfoGridControl.MenuManager = this.ribbon;
            this.CraftLevelInfoGridControl.Name = "CraftLevelInfoGridControl";
            this.CraftLevelInfoGridControl.Size = new System.Drawing.Size(960, 358);
            this.CraftLevelInfoGridControl.TabIndex = 2;
            this.CraftLevelInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CraftLevelInfoGridView});
            // 
            // CraftLevelInfoGridView
            // 
            this.CraftLevelInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CraftLevelInfoType,
            this.CraftLevelInfoLevel,
            this.CraftLevelInfoExp});
            this.CraftLevelInfoGridView.GridControl = this.CraftLevelInfoGridControl;
            this.CraftLevelInfoGridView.Name = "CraftLevelInfoGridView";
            this.CraftLevelInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.CraftLevelInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.CraftLevelInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.CraftLevelInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.CraftLevelInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // CraftLevelInfoType
            // 
            this.CraftLevelInfoType.FieldName = "Type";
            this.CraftLevelInfoType.Name = "CraftLevelInfoType";
            this.CraftLevelInfoType.Visible = true;
            this.CraftLevelInfoType.VisibleIndex = 0;
            // 
            // CraftLevelInfoLevel
            // 
            this.CraftLevelInfoLevel.FieldName = "Level";
            this.CraftLevelInfoLevel.Name = "CraftLevelInfoLevel";
            this.CraftLevelInfoLevel.Visible = true;
            this.CraftLevelInfoLevel.VisibleIndex = 1;
            // 
            // CraftLevelInfoExp
            // 
            this.CraftLevelInfoExp.FieldName = "Exp";
            this.CraftLevelInfoExp.Name = "CraftLevelInfoExp";
            this.CraftLevelInfoExp.Visible = true;
            this.CraftLevelInfoExp.VisibleIndex = 2;
            // 
            // 
            // CraftInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 544);
            this.Controls.Add(this.tabPane1);
            this.Controls.Add(this.ribbon);
            this.Name = "CraftInfoView";
            this.Ribbon = this.ribbon;
            this.Text = "CraftInfoView";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CraftInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CraftInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).EndInit();
            this.tabPane1.ResumeLayout(false);
            this.tabNavigationPage1.ResumeLayout(false);
            this.tabNavigationPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CraftLevelInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CraftLevelInfoGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private DevExpress.XtraGrid.GridControl CraftInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CraftInfoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemType;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemItem;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemLevel;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient1;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient1Amount;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient2;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient2Amount;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient3;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient3Amount;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient4;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemIngredient4Amount;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemCost;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemAmount;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemChance;
        private DevExpress.XtraGrid.Columns.GridColumn CraftInfoItemExp;
        private DevExpress.XtraBars.Navigation.TabPane tabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ItemLookUpEdit;
        private DevExpress.XtraGrid.GridControl CraftLevelInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CraftLevelInfoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn CraftLevelInfoType;
        private DevExpress.XtraGrid.Columns.GridColumn CraftLevelInfoLevel;
        private DevExpress.XtraGrid.Columns.GridColumn CraftLevelInfoExp;
    }
}