using System;
using System.ComponentModel;
using DevExpress.XtraBars;
using Server.Envir;

namespace Server.Views
{
    public partial class SystemGMLogView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static BindingList<string> GMLogs = new BindingList<string>();

        public SystemGMLogView()
        {
            InitializeComponent();

            LogListBoxControl.DataSource = GMLogs;
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
            String log;

            while (SEnvir.DisplayGMLogs.TryDequeue(out log))
                GMLogs.Add(log);

            if (GMLogs.Count > 0)
                ClearLogsButton.Enabled = true;
        }

        private void ClearLogsButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GMLogs.Clear();
            ClearLogsButton.Enabled = false;
        }
    }
}