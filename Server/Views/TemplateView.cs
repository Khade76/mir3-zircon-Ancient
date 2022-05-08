using System;
using DevExpress.XtraBars;

namespace Server.Views
{
    public partial class TemplateView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public TemplateView()
        {
            InitializeComponent();

            //TemplateGridControl.DataSource = SMain.Session.GetCollection<Template>().Binding;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SMain.SetUpView(TemplateGridView);
        }

        private void SaveButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SMain.Session.Save(true);
        }
    }
}