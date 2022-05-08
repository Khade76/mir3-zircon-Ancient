﻿using System;
using DevExpress.XtraBars;
using Library.SystemModels;

namespace Server.Views
{
    public partial class DropInfoView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public DropInfoView()
        {
            InitializeComponent();

            DropInfoGridControl.DataSource = SMain.Session.GetCollection<DropInfo>().Binding;
            DropListLookUpEdit.DataSource = SMain.Session.GetCollection<DropListInfo>().Binding;

            MonsterLookUpEdit.DataSource = SMain.Session.GetCollection<MonsterInfo>().Binding;
            ItemLookUpEdit.DataSource = SMain.Session.GetCollection<ItemInfo>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(DropInfoGridView);
        }


        private void SavingButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}