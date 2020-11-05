using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MouseMapper
{
    public partial class SplashScreen : Form
    {
        Hub f;
        public SplashScreen()
        {
            f = new Hub();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(Properties.Settings.Default.Rf, Properties.Settings.Default.Gf, Properties.Settings.Default.Bf);
            notifyIcon1.Icon = MouseMapper.Properties.Resources.ChocolateBar;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while(f.ready == false)
            {

            }
            f.Opacity = 0;
            f.Show();
            f.Hide();
            this.Hide();
            timer1.Stop();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            f.Show();
        }
    }
}
