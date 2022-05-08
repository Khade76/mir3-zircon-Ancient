﻿using System;
using DevExpress.XtraBars;
using Library.SystemModels;

namespace Server.Views
{
    public partial class BaseStatView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public BaseStatView()
        {
            InitializeComponent();

            BaseStatGridControl.DataSource = SMain.Session.GetCollection<BaseStat>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(BaseStatGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }

        private void BaseStatGridControl_Click(object sender, EventArgs e)
        {

        }
    }
}