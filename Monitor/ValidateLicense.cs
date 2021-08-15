using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitorTrackerForm
{
    public partial class ValidateLicense : Form
    {
        public ValidateLicense()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Guid validate;
            if (!string.IsNullOrEmpty(txtllave.Text))
            {
                bool isValid = Guid.TryParse(txtllave.Text, out validate);
                if (isValid)
                {
                    var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings.Remove("ApiKey");
                    config.AppSettings.Settings.Add("ApiKey", txtllave.Text);
                    config.Save(ConfigurationSaveMode.Minimal);
                    ConfigurationManager.RefreshSection("appSettings");
                    this.Close();
                    StartUp su = new StartUp();
                    su.RunOnStartup();
                }
                else
                {
                    MessageBox.Show("Debe Colocar una llave válida");
                }
            }
            else
            {
                MessageBox.Show("Debe Colocar una llave válida");
            }
        }

        private void ValidateLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Track trk = new Track();
            //trk.Activate();
        }

        private void ValidateLicense_Load(object sender, EventArgs e)
        {

        }
    }
}
