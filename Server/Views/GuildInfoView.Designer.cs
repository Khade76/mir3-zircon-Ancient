namespace Server.Views
{
    partial class GuildInfoView
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
            this.AccountLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.GuildGridControl = new DevExpress.XtraGrid.GridControl();
            this.GuildGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.CastleLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.Temp = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.AccountLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GuildGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GuildGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CastleLookUpEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountLookUpEdit
            // 
            this.AccountLookUpEdit.AutoHeight = false;
            this.AccountLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.AccountLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AccountLookUpEdit.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F5);
            this.AccountLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EMailAddress", "E-Mail")});
            this.AccountLookUpEdit.DisplayMember = "Name";
            this.AccountLookUpEdit.Name = "AccountLookUpEdit";
            this.AccountLookUpEdit.NullText = "[Account is null]";
            // 
            // GuildGridControl
            // 
            this.GuildGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuildGridControl.Location = new System.Drawing.Point(0, 0);
            this.GuildGridControl.MainView = this.GuildGridView;
            this.GuildGridControl.Name = "GuildGridControl";
            this.GuildGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.AccountLookUpEdit,
            this.CastleLookUpEdit});
            this.GuildGridControl.Size = new System.Drawing.Size(858, 523);
            this.GuildGridControl.TabIndex = 0;
            this.GuildGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GuildGridView});
            this.GuildGridControl.Click += new System.EventHandler(this.GuildGridControl_Click);
            // 
            // GuildGridView
            // 
            this.GuildGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn9,
            this.gridColumn11,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn7,
            this.gridColumn4,
            this.gridColumn8,
            this.Temp});
            this.GuildGridView.GridControl = this.GuildGridControl;
            this.GuildGridView.Name = "GuildGridView";
            this.GuildGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "GuildName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Guild Tax";
            this.gridColumn2.FieldName = "GuildTax";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Guild Funds";
            this.gridColumn3.FieldName = "GuildFunds";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "GuildLevel";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Member Limit";
            this.gridColumn5.FieldName = "MemberLimit";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Storage Size";
            this.gridColumn7.FieldName = "StorageSize";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Guild Notice";
            this.gridColumn8.FieldName = "GuildNotice";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 6;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Default Rank";
            this.gridColumn9.FieldName = "DefaultRank";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Castle";
            this.gridColumn11.FieldName = "Castle";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 8;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Mask.EditMask = "#,##0.#";
            this.repositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = true;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // CastleLookUpEdit
            // 
            this.CastleLookUpEdit.AutoHeight = false;
            this.CastleLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CastleLookUpEdit.DisplayMember = "Name";
            this.CastleLookUpEdit.Name = "CastleLookUpEdit";
            // 
            // Temp
            // 
            this.Temp.Caption = "Is Temporary";
            this.Temp.FieldName = "IsTemporary";
            this.Temp.Name = "Temp";
            this.Temp.Visible = true;
            this.Temp.VisibleIndex = 9;
            // 
            // GuildInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 523);
            this.Controls.Add(this.GuildGridControl);
            this.Name = "GuildInfoView";
            this.Text = "Guild View";
            ((System.ComponentModel.ISupportInitialize)(this.AccountLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GuildGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GuildGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CastleLookUpEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GuildGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView GuildGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit AccountLookUpEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit CastleLookUpEdit;
        private DevExpress.XtraGrid.Columns.GridColumn Temp;
    }
}