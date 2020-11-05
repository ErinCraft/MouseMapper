using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseMapper
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.Location = Hub.getLocation();
            this.Location = new Point(this.Location.X + Hub.getWidth() + 5, this. Location.Y);
            

            pictureBox1.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
            if (!Properties.Settings.Default.isTransparent)
            {
                pictureBox2.BackColor = Color.FromArgb(Properties.Settings.Default.Rb, Properties.Settings.Default.Gb, Properties.Settings.Default.Bb);
                button1.BackColor = Color.FromArgb(Properties.Settings.Default.Rb, Properties.Settings.Default.Gb, Properties.Settings.Default.Bb);
                checkBox3.Checked = false;
            }
            else
            {
                pictureBox2.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
                button1.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
                checkBox3.Checked = true;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Properties.Settings.Default.R = colorDialog1.Color.R;
            Properties.Settings.Default.G = colorDialog1.Color.G;
            Properties.Settings.Default.B = colorDialog1.Color.B;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Properties.Settings.Default.Rb = colorDialog1.Color.R;
            Properties.Settings.Default.Gb = colorDialog1.Color.G;
            Properties.Settings.Default.Bb = colorDialog1.Color.B;
            pictureBox1.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
            if (!Properties.Settings.Default.isTransparent)
            {
                pictureBox2.BackColor = Color.FromArgb(Properties.Settings.Default.Rb, Properties.Settings.Default.Gb, Properties.Settings.Default.Bb);
                checkBox3.Checked = false;
            }
            else
            {
                pictureBox2.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
                checkBox3.Checked = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                Properties.Settings.Default.isTransparent = true;
            else
                Properties.Settings.Default.isTransparent = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            Properties.Settings.Default.DefaultlblFont = fontDialog1.Font;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            Properties.Settings.Default.DefaulttxtFont = fontDialog1.Font;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Properties.Settings.Default.Rf = colorDialog1.Color.R;
            Properties.Settings.Default.Gf = colorDialog1.Color.G;
            Properties.Settings.Default.Bf = colorDialog1.Color.B;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.R = Properties.Settings.Default.Rb = 18;
            Properties.Settings.Default.G = Properties.Settings.Default.Gb = 137;
            Properties.Settings.Default.B = Properties.Settings.Default.Bb = 167;
            Properties.Settings.Default.Rf = Properties.Settings.Default.Gf = Properties.Settings.Default.Bf = 240;
            Properties.Settings.Default.DefaultlblFont = new Font("Calibri Light" , 14.25f);
            Properties.Settings.Default.DefaulttxtFont = new Font("Calibri Light", 9f);
            Properties.Settings.Default.Save();
        }
    }
}
