namespace Server.Views
{
    partial class MiniGameInfoView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniGameInfoView));
            this.RewardsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ItemLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colChance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTop1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTop2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTop3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonsterInfoGridControl = new DevExpress.XtraGrid.GridControl();
            this.miniGameInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CTFGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTeamAFlagSpawn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RegionLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colTeamBFlagSpawn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTeamAFlagReturn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTeamBFlagReturn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFlagMonster = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonsterLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.MiniGameInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMiniGame = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMapParameter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MapLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colMapLobby = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTeamARegion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTeamBRegion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMinLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMaxLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDuration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEntryFee = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTeamGame = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCanRevive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReviveDelay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SaveButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.cTFInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.colMinPlayers = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMaxPlayers = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.RewardsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniGameInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CTFGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegionLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiniGameInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cTFInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // RewardsGridView
            // 
            this.RewardsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItem,
            this.colChance,
            this.colAmount,
            this.colTop1,
            this.colTop2,
            this.colTop3});
            this.RewardsGridView.GridControl = this.MonsterInfoGridControl;
            this.RewardsGridView.Name = "RewardsGridView";
            this.RewardsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.RewardsGridView.OptionsView.EnableAppearanceOddRow = true;
            this.RewardsGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.RewardsGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.RewardsGridView.OptionsView.ShowGroupPanel = false;
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
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItemType", "Item Type")});
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
            // colTop1
            // 
            this.colTop1.FieldName = "Top1";
            this.colTop1.Name = "colTop1";
            this.colTop1.Visible = true;
            this.colTop1.VisibleIndex = 3;
            // 
            // colTop2
            // 
            this.colTop2.FieldName = "Top2";
            this.colTop2.Name = "colTop2";
            this.colTop2.Visible = true;
            this.colTop2.VisibleIndex = 4;
            // 
            // colTop3
            // 
            this.colTop3.FieldName = "Top3";
            this.colTop3.Name = "colTop3";
            this.colTop3.Visible = true;
            this.colTop3.VisibleIndex = 5;
            // 
            // MonsterInfoGridControl
            // 
            this.MonsterInfoGridControl.DataSource = this.miniGameInfoBindingSource;
            this.MonsterInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.LevelTemplate = this.RewardsGridView;
            gridLevelNode1.RelationName = "Rewards";
            gridLevelNode2.LevelTemplate = this.CTFGridView;
            gridLevelNode2.RelationName = "CTFInfo";
            this.MonsterInfoGridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.MonsterInfoGridControl.Location = new System.Drawing.Point(0, 143);
            this.MonsterInfoGridControl.MainView = this.MiniGameInfoGridView;
            this.MonsterInfoGridControl.MenuManager = this.ribbon;
            this.MonsterInfoGridControl.Name = "MonsterInfoGridControl";
            this.MonsterInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.MapLookUpEdit,
            this.RegionLookUpEdit,
            this.MonsterLookUpEdit,
            this.ItemLookUpEdit});
            this.MonsterInfoGridControl.ShowOnlyPredefinedDetails = true;
            this.MonsterInfoGridControl.Size = new System.Drawing.Size(631, 291);
            this.MonsterInfoGridControl.TabIndex = 2;
            this.MonsterInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CTFGridView,
            this.MiniGameInfoGridView,
            this.RewardsGridView});
            this.MonsterInfoGridControl.Click += new System.EventHandler(this.MonsterInfoGridControl_Click);
            // 
            // miniGameInfoBindingSource
            // 
            this.miniGameInfoBindingSource.DataSource = typeof(Library.SystemModels.MiniGameInfo);
            // 
            // CTFGridView
            // 
            this.CTFGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTeamAFlagSpawn,
            this.colTeamBFlagSpawn,
            this.colTeamAFlagReturn,
            this.colTeamBFlagReturn,
            this.colFlagMonster});
            this.CTFGridView.GridControl = this.MonsterInfoGridControl;
            this.CTFGridView.Name = "CTFGridView";
            this.CTFGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.CTFGridView.OptionsView.EnableAppearanceOddRow = true;
            this.CTFGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.CTFGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.CTFGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colTeamAFlagSpawn
            // 
            this.colTeamAFlagSpawn.ColumnEdit = this.RegionLookUpEdit;
            this.colTeamAFlagSpawn.FieldName = "TeamAFlagSpawn";
            this.colTeamAFlagSpawn.Name = "colTeamAFlagSpawn";
            this.colTeamAFlagSpawn.Visible = true;
            this.colTeamAFlagSpawn.VisibleIndex = 0;
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
            // colTeamBFlagSpawn
            // 
            this.colTeamBFlagSpawn.ColumnEdit = this.RegionLookUpEdit;
            this.colTeamBFlagSpawn.FieldName = "TeamBFlagSpawn";
            this.colTeamBFlagSpawn.Name = "colTeamBFlagSpawn";
            this.colTeamBFlagSpawn.Visible = true;
            this.colTeamBFlagSpawn.VisibleIndex = 1;
            // 
            // colTeamAFlagReturn
            // 
            this.colTeamAFlagReturn.ColumnEdit = this.RegionLookUpEdit;
            this.colTeamAFlagReturn.FieldName = "TeamAFlagReturn";
            this.colTeamAFlagReturn.Name = "colTeamAFlagReturn";
            this.colTeamAFlagReturn.Visible = true;
            this.colTeamAFlagReturn.VisibleIndex = 2;
            // 
            // colTeamBFlagReturn
            // 
            this.colTeamBFlagReturn.ColumnEdit = this.RegionLookUpEdit;
            this.colTeamBFlagReturn.FieldName = "TeamBFlagReturn";
            this.colTeamBFlagReturn.Name = "colTeamBFlagReturn";
            this.colTeamBFlagReturn.Visible = true;
            this.colTeamBFlagReturn.VisibleIndex = 3;
            // 
            // colFlagMonster
            // 
            this.colFlagMonster.Caption = "colFlagMonster";
            this.colFlagMonster.ColumnEdit = this.MonsterLookUpEdit;
            this.colFlagMonster.FieldName = "FlagMonster";
            this.colFlagMonster.Name = "colFlagMonster";
            this.colFlagMonster.Visible = true;
            this.colFlagMonster.VisibleIndex = 4;
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
            // MiniGameInfoGridView
            // 
            this.MiniGameInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMiniGame,
            this.colMapParameter,
            this.colMapLobby,
            this.colTeamARegion,
            this.colTeamBRegion,
            this.colMinLevel,
            this.colMaxLevel,
            this.colDuration,
            this.colEntryFee,
            this.colTeamGame,
            this.colCanRevive,
            this.colReviveDelay,
            this.colMinPlayers,
            this.colMaxPlayers});
            this.MiniGameInfoGridView.GridControl = this.MonsterInfoGridControl;
            this.MiniGameInfoGridView.Name = "MiniGameInfoGridView";
            this.MiniGameInfoGridView.OptionsDetail.AllowExpandEmptyDetails = true;
            this.MiniGameInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.MiniGameInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.MiniGameInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.MiniGameInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.MiniGameInfoGridView.OptionsView.ShowGroupPanel = false;
            this.MiniGameInfoGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colMiniGame, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colMiniGame
            // 
            this.colMiniGame.FieldName = "MiniGame";
            this.colMiniGame.Name = "colMiniGame";
            this.colMiniGame.Visible = true;
            this.colMiniGame.VisibleIndex = 0;
            // 
            // colMapParameter
            // 
            this.colMapParameter.ColumnEdit = this.MapLookUpEdit;
            this.colMapParameter.FieldName = "MapParameter";
            this.colMapParameter.Name = "colMapParameter";
            this.colMapParameter.Visible = true;
            this.colMapParameter.VisibleIndex = 1;
            // 
            // MapLookUpEdit
            // 
            this.MapLookUpEdit.AutoHeight = false;
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
            // colMapLobby
            // 
            this.colMapLobby.ColumnEdit = this.MapLookUpEdit;
            this.colMapLobby.FieldName = "MapLobby";
            this.colMapLobby.Name = "colMapLobby";
            this.colMapLobby.Visible = true;
            this.colMapLobby.VisibleIndex = 2;
            // 
            // colTeamARegion
            // 
            this.colTeamARegion.ColumnEdit = this.RegionLookUpEdit;
            this.colTeamARegion.FieldName = "TeamASpawn";
            this.colTeamARegion.Name = "colTeamARegion";
            this.colTeamARegion.Visible = true;
            this.colTeamARegion.VisibleIndex = 3;
            // 
            // colTeamBRegion
            // 
            this.colTeamBRegion.ColumnEdit = this.RegionLookUpEdit;
            this.colTeamBRegion.FieldName = "TeamBSpawn";
            this.colTeamBRegion.Name = "colTeamBRegion";
            this.colTeamBRegion.Visible = true;
            this.colTeamBRegion.VisibleIndex = 4;
            // 
            // colMinLevel
            // 
            this.colMinLevel.FieldName = "MinLevel";
            this.colMinLevel.Name = "colMinLevel";
            this.colMinLevel.Visible = true;
            this.colMinLevel.VisibleIndex = 6;
            // 
            // colMaxLevel
            // 
            this.colMaxLevel.FieldName = "MaxLevel";
            this.colMaxLevel.Name = "colMaxLevel";
            this.colMaxLevel.Visible = true;
            this.colMaxLevel.VisibleIndex = 7;
            // 
            // colDuration
            // 
            this.colDuration.FieldName = "Duration";
            this.colDuration.Name = "colDuration";
            this.colDuration.Visible = true;
            this.colDuration.VisibleIndex = 5;
            // 
            // colEntryFee
            // 
            this.colEntryFee.FieldName = "EntryFee";
            this.colEntryFee.Name = "colEntryFee";
            this.colEntryFee.Visible = true;
            this.colEntryFee.VisibleIndex = 8;
            // 
            // colTeamGame
            // 
            this.colTeamGame.Caption = "Team Game";
            this.colTeamGame.FieldName = "TeamGame";
            this.colTeamGame.Name = "colTeamGame";
            this.colTeamGame.Visible = true;
            this.colTeamGame.VisibleIndex = 9;
            // 
            // colCanRevive
            // 
            this.colCanRevive.FieldName = "CanRevive";
            this.colCanRevive.Name = "colCanRevive";
            this.colCanRevive.Visible = true;
            this.colCanRevive.VisibleIndex = 10;
            // 
            // colReviveDelay
            // 
            this.colReviveDelay.FieldName = "ReviveDelay";
            this.colReviveDelay.Name = "colReviveDelay";
            this.colReviveDelay.Visible = true;
            this.colReviveDelay.VisibleIndex = 11;
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
            this.ribbon.Size = new System.Drawing.Size(631, 143);
            // 
            // SaveButton
            // 
            this.SaveButton.Caption = "Save Databasse";
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
            // cTFInfoBindingSource
            // 
            this.cTFInfoBindingSource.DataMember = "CTFInfo";
            this.cTFInfoBindingSource.DataSource = this.miniGameInfoBindingSource;
            // 
            // colMinPlayers
            // 
            this.colMinPlayers.FieldName = "MinPlayers";
            this.colMinPlayers.Name = "colMinPlayers";
            this.colMinPlayers.Visible = true;
            this.colMinPlayers.VisibleIndex = 12;
            // 
            // colMaxPlayers
            // 
            this.colMaxPlayers.FieldName = "MaxPlayers";
            this.colMaxPlayers.Name = "colMaxPlayers";
            this.colMaxPlayers.Visible = true;
            this.colMaxPlayers.VisibleIndex = 13;
            // 
            // MiniGameInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 434);
            this.Controls.Add(this.MonsterInfoGridControl);
            this.Controls.Add(this.ribbon);
            this.Name = "MiniGameInfoView";
            this.Ribbon = this.ribbon;
            this.Text = "Mini Games";
            ((System.ComponentModel.ISupportInitialize)(this.RewardsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniGameInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CTFGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegionLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonsterLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiniGameInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cTFInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private System.Windows.Forms.BindingSource miniGameInfoBindingSource;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MapLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit RegionLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ItemLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterLookUpEdit;
        public DevExpress.XtraGrid.Views.Grid.GridView RewardsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colItem;
        private DevExpress.XtraGrid.Columns.GridColumn colChance;
        private DevExpress.XtraGrid.Columns.GridColumn colAmount;
        private DevExpress.XtraGrid.GridControl MonsterInfoGridControl;
        private System.Windows.Forms.BindingSource cTFInfoBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView CTFGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamAFlagSpawn;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamBFlagSpawn;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamAFlagReturn;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamBFlagReturn;
        private DevExpress.XtraGrid.Views.Grid.GridView MiniGameInfoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colMiniGame;
        private DevExpress.XtraGrid.Columns.GridColumn colMapParameter;
        private DevExpress.XtraGrid.Columns.GridColumn colMapLobby;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamARegion;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamBRegion;
        private DevExpress.XtraGrid.Columns.GridColumn colMinLevel;
        private DevExpress.XtraGrid.Columns.GridColumn colMaxLevel;
        private DevExpress.XtraGrid.Columns.GridColumn colDuration;
        private DevExpress.XtraGrid.Columns.GridColumn colEntryFee;
        private DevExpress.XtraGrid.Columns.GridColumn colTeamGame;
        private DevExpress.XtraGrid.Columns.GridColumn colFlagMonster;
        private DevExpress.XtraGrid.Columns.GridColumn colCanRevive;
        private DevExpress.XtraGrid.Columns.GridColumn colReviveDelay;
        private DevExpress.XtraGrid.Columns.GridColumn colTop1;
        private DevExpress.XtraGrid.Columns.GridColumn colTop2;
        private DevExpress.XtraGrid.Columns.GridColumn colTop3;
        private DevExpress.XtraGrid.Columns.GridColumn colMinPlayers;
        private DevExpress.XtraGrid.Columns.GridColumn colMaxPlayers;
    }
}