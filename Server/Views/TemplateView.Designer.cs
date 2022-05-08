namespace Server.Views
{
    partial class TemplateView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateView));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.SaveButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.miniGameInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TemplateGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TemplateGridControl = new DevExpress.XtraGrid.GridControl();
            this.colIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMiniGame = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMaxValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMapParameter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDuration = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniGameInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplateGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplateGridControl)).BeginInit();
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
            this.ribbon.Size = new System.Drawing.Size(631, 141);
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
            // miniGameInfoBindingSource
            // 
            this.miniGameInfoBindingSource.DataSource = typeof(Library.SystemModels.MiniGameInfo);
            // 
            // TemplateGridView
            // 
            this.TemplateGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIndex,
            this.colMiniGame,
            this.colMaxValue,
            this.colMapParameter,
            this.colDuration});
            this.TemplateGridView.GridControl = this.TemplateGridControl;
            this.TemplateGridView.Name = "TemplateGridView";
            this.TemplateGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.TemplateGridView.OptionsView.EnableAppearanceOddRow = true;
            this.TemplateGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.TemplateGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.TemplateGridView.OptionsView.ShowGroupPanel = false;
            // 
            // TemplateGridControl
            // 
            this.TemplateGridControl.DataSource = this.miniGameInfoBindingSource;
            this.TemplateGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TemplateGridControl.Location = new System.Drawing.Point(0, 141);
            this.TemplateGridControl.MainView = this.TemplateGridView;
            this.TemplateGridControl.MenuManager = this.ribbon;
            this.TemplateGridControl.Name = "TemplateGridControl";
            this.TemplateGridControl.Size = new System.Drawing.Size(631, 293);
            this.TemplateGridControl.TabIndex = 2;
            this.TemplateGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.TemplateGridView});
            // 
            // colIndex
            // 
            this.colIndex.FieldName = "Index";
            this.colIndex.Name = "colIndex";
            this.colIndex.Visible = true;
            this.colIndex.VisibleIndex = 0;
            // 
            // colMiniGame
            // 
            this.colMiniGame.FieldName = "MiniGame";
            this.colMiniGame.Name = "colMiniGame";
            this.colMiniGame.Visible = true;
            this.colMiniGame.VisibleIndex = 1;
            // 
            // colMaxValue
            // 
            this.colMaxValue.FieldName = "MaxValue";
            this.colMaxValue.Name = "colMaxValue";
            this.colMaxValue.Visible = true;
            this.colMaxValue.VisibleIndex = 2;
            // 
            // colMapParameter
            // 
            this.colMapParameter.FieldName = "MapParameter";
            this.colMapParameter.Name = "colMapParameter";
            this.colMapParameter.Visible = true;
            this.colMapParameter.VisibleIndex = 3;
            // 
            // colDuration
            // 
            this.colDuration.FieldName = "Duration";
            this.colDuration.Name = "colDuration";
            this.colDuration.Visible = true;
            this.colDuration.VisibleIndex = 4;
            // 
            // TemplateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 434);
            this.Controls.Add(this.TemplateGridControl);
            this.Controls.Add(this.ribbon);
            this.Name = "TemplateView";
            this.Ribbon = this.ribbon;
            this.Text = "TemplateView";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniGameInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplateGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplateGridControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private System.Windows.Forms.BindingSource miniGameInfoBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView TemplateGridView;
        private DevExpress.XtraGrid.GridControl TemplateGridControl;
        private DevExpress.XtraGrid.Columns.GridColumn colIndex;
        private DevExpress.XtraGrid.Columns.GridColumn colMiniGame;
        private DevExpress.XtraGrid.Columns.GridColumn colMaxValue;
        private DevExpress.XtraGrid.Columns.GridColumn colMapParameter;
        private DevExpress.XtraGrid.Columns.GridColumn colDuration;
    }
}