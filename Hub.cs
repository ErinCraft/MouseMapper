using MouseMapper.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace MouseMapper
{
    public partial class Hub : Form
    {
        ClassContainer container = new ClassContainer();
        static Point p = new Point();
        static int wHub;
        static int toRemove = 0;
        public bool ready = false;
        public bool is_customizating = false;

        public Hub()
        {
            InitializeComponent();
            notifyIcon1.Visible = true;
            ready = true;
            backgroundWorker1.RunWorkerAsync();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = 1;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.BackColor = Color.FromArgb(Properties.Settings.Default.Rf, Properties.Settings.Default.Gf, Properties.Settings.Default.Bf);
            this.Controls.Add(container.p1);
            this.Controls.Add(container.p2);
            button10.Enabled = false;
            button10.Height = button10.Height - 10;
            button10.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
            addFromFile();

            container.addHandlers(subAddbtn_Click, subCancelbtn_Click, subChoosebtn_Click);

            label1.Text = Application.ProductName;
            button1.BackColor = button2.BackColor = panel1.BackColor = panel2.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
            panel1.Hide();
            panel2.Hide();

            p = this.Location;
            wHub = this.Width;

        }

        //METODI PER LETTURA, SOVRASCRITTURA, MODIFICA DELLE CONFIGURAZIONI
        #region Configurations
        //IMPORTAZIONE AUTOMATICA ALL'AVVIO
        private void addFromFile()
        {
            //CREA OGGETTI E ASSEGNA LE LORO PROPRIETÀ
            string file = "";
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (System.IO.File.Exists(System.IO.Path.Combine(path, "MouseMapper/application.config") ) || System.IO.File.ReadAllText(System.IO.Path.Combine(path, "MouseMapper")).Equals(""))
                file = System.IO.File.ReadAllText(System.IO.Path.Combine(path, "MouseMapper/application.config"));
            else
            {
                System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + "MouseMapper");
                System.IO.File.Create(System.IO.Path.Combine(path,  "MouseMapper/application.config"));
            }

            if (!file.Equals(""))
            {
                string[] allprop;
                allprop = file.Split('#');
                container.conta = allprop.Length - 2;

                for (int i = 0; i < container.conta; i++)
                {

                    ProgramConfiguration pg1 = new ProgramConfiguration(allprop[i + 1]);

                    if (pg1.getGameMode() == 0)
                    {
                        PanelEx dpex = new PanelEx(container.contasoftware);


                        //CREAZIONE DI UN LABEL AGGIUNTIVO
                        dpex.addLabel(pg1.getAlias());
                        //CREAZIONE DI UNA PICTUREBOX
                        dpex.addPBox(Resources.notSet);
                        //CREAZIONE DELLE TEXTBOX
                        dpex.addTbox(pg1.getLeft(), pg1.getRight(), pg1.getXb2(), pg1.getXb1(), pg1.getMiddle());
                        //CREAZIONE DEI LABEL
                        dpex.addTLbl();
                        //AGGIUNGO LISTENER
                        dpex.addButtons(cancelConfig, modifyConfig, ("CancS" + container.contasoftware.ToString()), ("ModS" + container.contasoftware.ToString()));
                        //AGGIUNGO DIR PER CHANGEALIAS (E DIR)
                        dpex.setDir(pg1.getDir());

                        container.p1.Controls.Add(dpex.dynPanel);
                        container.allPanel.Add(dpex);
                        container.ListConfig.Add(pg1);
                        container.AllConfig.Add(pg1);
                        container.addtoList(dpex.Cancelbtn);
                        container.contasoftware++;

                        if (container.contasoftware * (dpex.dynPanel.Height) > container.p1.Height)
                        {
                            container.b1.Maximum = ((container.contasoftware * (dpex.dynPanel.Height + 5)) - container.p1.Height + 58 + 29);
                        }
                    }
                    else
                    {
                        PanelEx dpex = new PanelEx(container.contagames);

                        //CREAZIONE DI UN LABEL AGGIUNTIVO
                        dpex.addLabel(pg1.getAlias());
                        //CREAZIONE DI UNA PICTUREBOX
                        dpex.addPBox(Resources.notSet);
                        //CREAZIONE DELLE TEXTBOX
                        dpex.addTbox(pg1.getLeft(), pg1.getRight(), pg1.getXb2(), pg1.getXb1(), pg1.getMiddle());
                        //CREAZIONE DEI LABEL
                        dpex.addTLbl();                        
                        //AGGIUNGO LISTENER
                        dpex.addButtons(cancelConfig, modifyConfig, ("CancG" + container.contagames.ToString()), ("ModG" + container.contagames.ToString()));
                        //AGGIUNGO DIR PER CHANGEALIAS (E DIR)
                        dpex.setDir(pg1.getDir());

                        container.p2.Controls.Add(dpex.dynPanel);
                        container.allPanelGames.Add(dpex);
                        container.ListConfigGames.Add(pg1);
                        container.AllConfig.Add(pg1);
                        container.addtoListGames(dpex.Cancelbtn);
                        container.contagames++;

                        if (container.contagames * (dpex.dynPanel.Height) > container.p2.Height)
                        {
                            container.b2.Maximum = ((container.contagames * (dpex.dynPanel.Height + 5)) - container.p2.Height + 58 + 29);
                        }
                    }

                }
                container.setPBox();
            }
            container.checkIfEmpty();
        }
        //AGGIUNTA DI UNA SINGOLA CONFIGURAZIONE A RUNTIME
        private void aggiungiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            is_customizating = false;
            this.Controls.Remove(container.p1);
            this.Controls.Remove(container.p2);
            this.Controls.Add(container.adder.panel);
            container.adder.setAcDischeck(true);
            timer7.Start();
            this.Controls.Add(container.p1);
            this.Controls.Add(container.p2);
        }
        //AGGIORNAMENTO DELLE CONFIGURAZIONI. SOVRASCRITTURA DEL FILE DI CONFIGURAZIONE
        private void aggiornaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AGGIORNA IMMEDIATAMENTE LA LISTA DELLE CONFIGURAZIONI, PREPARANDOLA A SOVRASCRIVERE IL FILE .config

            foreach (ProgramConfiguration pg in container.ListConfig)
            {
                foreach (PanelEx panel in container.allPanel)
                {

                    if (pg.getAlias().Equals(panel.getlblAlias()))
                    {
                        string justName;

                        if (pg.getNome().Equals(""))
                        {
                            justName = panel.getFilename();
                            justName = justName.Replace(".exe", "");
                            justName = justName.Replace(".EXE", "");
                            pg.setNome(justName);
                            pg.setDir(panel.getDir());
                        }
                        pg.setNome(pg.getNome());
                        pg.setDir(pg.getDir());
                        pg.setLeft(panel.getLeft());
                        pg.setRight(panel.getRight());
                        pg.setXb1(panel.getXb1());
                        pg.setXb2(panel.getXb2());
                        pg.setMiddle(panel.getMiddle());
                        pg.setGameMode("", 0);
                    }
                }

            }
            foreach (ProgramConfiguration pg in container.ListConfigGames)
            {
                foreach (PanelEx panel in container.allPanelGames)
                {

                    if (pg.getAlias().Equals(panel.getlblAlias()))
                    {
                        string justName;

                        if (pg.getNome().Equals(""))
                        {
                            justName = panel.getFilename();
                            justName = justName.Replace(".exe", "");
                            justName = justName.Replace(".EXE", "");
                            pg.setNome(justName);
                            pg.setDir(panel.getDir());
                        }
                        pg.setNome(pg.getNome());
                        pg.setDir(pg.getDir());
                        pg.setLeft(panel.getLeft());
                        pg.setRight(panel.getRight());
                        pg.setXb1(panel.getXb1());
                        pg.setXb2(panel.getXb2());
                        pg.setMiddle(panel.getMiddle());
                        pg.setGameMode("", 1);
                    }
                }
            }
            //SOVRASCRIVE
            string file = "";
            foreach (ProgramConfiguration pg in container.ListConfig)
            {
                file += pg.getTxtCode();
            }
            foreach (ProgramConfiguration pg in container.ListConfigGames)
            {
                file += pg.getTxtCode();
            }
            file += "#" + '\n';
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.File.WriteAllText(System.IO.Path.Combine(path, "MouseMapper/application.config"), file);
        }
        #endregion

        //METODI PER IL MAPPING DEL MOUSE
        #region mousemapping
        //METODI DI LIBRERIE NECESSARIE
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        //METODO PER DETERMINARE SE SI STA GIOCANDO O NO
        public void checkWhatIts()
        {
            bool modeActive = false;
            Thread t = new Thread(getCodes);

            while (true)
            {
                int processID;
                GetWindowThreadProcessId(GetForegroundWindow(), out processID);
                Process p = Process.GetProcessById(processID);

                foreach (ProgramConfiguration pg in container.AllConfig)
                {
                    if(pg.getNome().Equals(p.ProcessName) && pg.getGameMode() == 1)
                    {
                        if (!modeActive)
                        {
                            t.Start();
                            modeActive = true;
                        }
                    }else if (pg.getNome().Equals(p.ProcessName) && pg.getGameMode() == 0)
                    {
                        switch (MouseButtons)
                        {
                            case MouseButtons.Left:
                                SendKeys.SendWait(pg.getLCode());
                                break;
                            case MouseButtons.Right:
                                SendKeys.SendWait(pg.getRCode());
                                break;
                            case MouseButtons.XButton1:
                                SendKeys.SendWait(pg.getXb2Code());
                                break;
                            case MouseButtons.XButton2:
                                SendKeys.SendWait(pg.getXb1Code());
                                break;
                            case MouseButtons.Middle:
                                SendKeys.SendWait(pg.getMCode());
                                break;
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
        //DECODIFICA DEI COMANDI PER GIOCARE
        public void getCodes()
        {
            int processID;
            int i = 0;

            while (true)
            {
                GetWindowThreadProcessId(GetForegroundWindow(), out processID);
                Process p = Process.GetProcessById(processID);
                foreach (ProgramConfiguration pg in container.ListConfigGames)
                {
                    if (p.ProcessName.Equals(pg.getNome()))
                    {
                        switch (MouseButtons)
                        {
                            #region left
                            case MouseButtons.Left:

                                while (MouseButtons == MouseButtons.Left && pg.getLdecode(pg.getLeft()) != 0x00)
                                {
                                    if (i == 0)
                                    {
                                        InputSender.clickdown(
                                            new InputSender.KeyboardInput
                                            {
                                                wScan = pg.getLdecode(pg.getLeft()),
                                                dwFlags = (uint)(InputSender.KeyEventF.KeyDown | InputSender.KeyEventF.Scancode)
                                            });
                                        i++;
                                    }

                                }
                                if (MouseButtons == MouseButtons.None)
                                {
                                    InputSender.ClickKey(pg.getLdecode(pg.getLeft()));
                                    i--;
                                }
                                break;
                            #endregion
                            #region right
                            case MouseButtons.Right:

                                while (MouseButtons == MouseButtons.Left && pg.getRdecode(pg.getRight()) != 0x00)
                                {
                                    if (i == 0)
                                    {
                                        InputSender.clickdown(
                                            new InputSender.KeyboardInput
                                            {
                                                wScan = pg.getRdecode(pg.getRight()),
                                                dwFlags = (uint)(InputSender.KeyEventF.KeyDown | InputSender.KeyEventF.Scancode)
                                            });
                                        i++;
                                    }

                                }
                                if (MouseButtons == MouseButtons.None)
                                {
                                    InputSender.ClickKey(pg.getRdecode(pg.getRight()));
                                    i--;
                                }
                                break;
                            #endregion
                            #region middle
                            case MouseButtons.Middle:

                                while (MouseButtons == MouseButtons.Middle && pg.getMdecode(pg.getMiddle()) != 0x00)
                                {
                                    if (i == 0)
                                    {
                                        InputSender.clickdown(
                                            new InputSender.KeyboardInput
                                            {
                                                wScan = pg.getMdecode(pg.getMiddle()),
                                                dwFlags = (uint)(InputSender.KeyEventF.KeyDown | InputSender.KeyEventF.Scancode)
                                            });
                                        i++;
                                    }

                                }
                                if (MouseButtons == MouseButtons.None)
                                {
                                    InputSender.ClickKey(pg.getMdecode(pg.getMiddle()));
                                    i--;
                                }
                                break;
                            #endregion
                            #region xbutton1
                            case MouseButtons.XButton1:

                                while (MouseButtons == MouseButtons.XButton1 && pg.getXb1decode(pg.getXb1()) != 0x00)
                                {
                                    if (i == 0)
                                    {
                                        InputSender.clickdown(
                                            new InputSender.KeyboardInput
                                            {
                                                wScan = pg.getXb1decode(pg.getXb1()),
                                                dwFlags = (uint)(InputSender.KeyEventF.KeyDown | InputSender.KeyEventF.Scancode)
                                            });
                                        i++;
                                    }

                                }
                                if (MouseButtons == MouseButtons.None)
                                {
                                    InputSender.ClickKey(pg.getXb1decode(pg.getXb1()));
                                    i--;
                                }
                                break;
                            #endregion
                            #region xbutton2
                            case MouseButtons.XButton2:

                                while (MouseButtons == MouseButtons.XButton2 && pg.getXb2decode(pg.getXb2()) != 0x00)
                                {
                                    if (i == 0)
                                    {
                                        InputSender.clickdown(
                                            new InputSender.KeyboardInput
                                            {
                                                wScan = pg.getXb2decode(pg.getXb2()),
                                                dwFlags = (uint)(InputSender.KeyEventF.KeyDown | InputSender.KeyEventF.Scancode)
                                            }); //4
                                        i++;
                                    }

                                }
                                if (MouseButtons == MouseButtons.None)
                                {
                                    InputSender.ClickKey(pg.getXb2decode(pg.getXb2()));
                                    i--;
                                }
                                break;
                                #endregion
                        }
                    }
                }
            }
        }
        #endregion

        //ANIMAZIONI
        #region animazionitimer
        //APERTURA / CHIUSURA PANEL 1 
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (panel1.Width < 192)
            {
                panel1.Width += 8;
            }
            else
                timer3.Stop();
        }
        private void timer5_Tick(object sender, EventArgs e)
        {
            if (panel1.Width > 0)
            {
                panel1.Width -= 8;
            }

            else
            {
                if (panel1.Width == 0)
                {
                    panel1.Hide();
                }
                timer5.Stop();
            }
        }

        //APERTURA / CHIUSURA PANEL 2
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (panel2.Width < 192)
            {
                panel2.Width += 8;
            }
            else
                timer4.Stop();
        }
        private void timer6_Tick(object sender, EventArgs e)
        {
            if (panel2.Width > 0)
            {
                panel2.Width -= 8;
            }
            else
            {
                if (panel2.Width == 0)
                {
                    panel2.Hide();
                }
                timer6.Stop();
            }
        }

        //CHIUSURA CONTEMPORANEA
        private void timer7_Tick(object sender, EventArgs e)
        {
            if (panel2.Width > 0 && panel2.Width > 0)
            {
                panel2.Width -= 8;
               
            }
            else if(panel2.Width == 0 && panel1.Width > 0)
            {
                panel1.Width -= 8;
            }
            else
            {
                if (panel2.Width == 0 && panel1.Width == 0)
                {
                    panel2.Hide();
                    panel1.Hide();
                }
                timer7.Stop();
            }
        }

        //ANIMAZIONI CAMBIO GIOCHI SOFTWARE
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (container.p1.Width < 716)
                container.p1.Width += 12;
            else
            {
                container.p1.Width = 716;
                timer1.Stop();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (container.p1.Width > 0)
                container.p1.Width -= 12;
            else
            {
                timer2.Stop();
            }
        }
        #endregion
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                backgroundWorker1.RunWorkerAsync();
                Hide();
                notifyIcon1.Visible = true;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            backgroundWorker1.CancelAsync();
        }
        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
        }
        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Environment.Exit(0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread t = new Thread(checkWhatIts);
            t.Start();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                panel1.Show();
                timer3.Start();
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(panel2.Visible == true)
            {
                timer7.Start();
                timer3.Stop();
                timer4.Stop();
            }
            else
            {
                timer3.Stop();
                timer5.Start();

            }
        }
        private void button7_Click(object sender, EventArgs e)
        { 
            if (panel2.Visible == false)
            {
                panel2.Show();
                timer4.Start();
            }
            else if(panel2.Visible == true)
            {
                timer6.Start();
                
            }
            
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Settings form = new Settings();
            form.Show(this);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public static Point getLocation()
        {
            return p;
        }
        public static int getWidth()
        {
            return wHub;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Stop();
            button10.Enabled = false;
            button11.Enabled = true;
            button10.Height = button10.Height - 10;
            button11.Height = button11.Height + 10;
            button10.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
            button11.BackColor = this.BackColor;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            timer2.Start();
            timer1.Stop();
            button10.Enabled = true;
            button11.Enabled = false;
            button11.Height = button11.Height - 10;
            button10.Height = button10.Height + 10;
            button10.BackColor = this.BackColor;
            button11.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);
        }
        //EVENTI DEDICATI A SUBFORM adder
        private void subAddbtn_Click(object sender, EventArgs e)
        {
            if (is_customizating == false)
            {
                if (!container.adder.getDirfromTxt().Equals(""))
                {
                    ProgramConfiguration pg = new ProgramConfiguration();
                    container.adder.setAlias(container.adder.getAliasfromTxt());
                    container.adder.setDir(container.adder.getDirfromTxt());
                    container.adder.setGamemode();

                    if (container.adder.getSGmod() == 0)
                    {
                        pg.setNome(container.adder.getName());
                        pg.setAlias(container.adder.getAlias());
                        pg.setDir(container.adder.getDir());
                        pg.setGameMode("", container.adder.getSGmod());

                        PanelEx panel = new PanelEx(container.contasoftware);
                        panel.addPBox(Resources.notSet);
                        panel.addTbox(pg.getLeft(), pg.getRight(), pg.getMiddle(), pg.getXb1(), pg.getXb2());
                        panel.addLabel(container.adder.getAlias());
                        panel.addTLbl();
                        panel.addButtons(cancelConfig, modifyConfig, ("CancS" + container.contasoftware.ToString()), ("ModS" + container.contasoftware.ToString()));
                        panel.setDir(pg.getDir());

                        container.allPanel.Add(panel);
                        container.ListConfig.Add(pg);
                        container.AllConfig.Add(pg);
                        container.p1.Controls.Add(panel.dynPanel);
                        container.addtoList(panel.Cancelbtn);

                        container.conta++;
                        container.contasoftware++;

                        if (container.contasoftware * (panel.dynPanel.Height) > container.p1.Height)
                        {
                            container.b1.Maximum = ((container.contasoftware * (panel.dynPanel.Height + 5)) - container.p1.Height + 58 + 29);
                        }
                    }
                    else
                    {
                        PanelEx panel = new PanelEx(container.contagames);

                        pg.setNome(container.adder.getName());
                        pg.setAlias(container.adder.getAlias());
                        pg.setDir(container.adder.getDir());
                        pg.setGameMode("", container.adder.getSGmod());

                        panel.addPBox(Resources.notSet);
                        panel.addTbox(pg.getLeft(), pg.getRight(), pg.getMiddle(), pg.getXb1(), pg.getXb2());
                        panel.addLabel(container.adder.getAlias());
                        panel.addTLbl();
                        panel.addButtons(cancelConfig, modifyConfig, ("CancG" + container.contagames.ToString()), ("ModG" + container.contagames.ToString()));
                        panel.setDir(pg.getDir());

                        container.allPanelGames.Add(panel);
                        container.ListConfigGames.Add(pg);
                        container.AllConfig.Add(pg);
                        container.p2.Controls.Add(panel.dynPanel);
                        container.addtoListGames(panel.Cancelbtn);

                        container.conta++;
                        container.contagames++;

                        if (container.contagames * (panel.dynPanel.Height) > container.p2.Height)
                        {
                            container.b2.Maximum = ((container.contagames * (panel.dynPanel.Height + 5)) - container.p2.Height + 58 + 29);
                        }
                    }
                    container.checkIfEmpty();
                    container.setPBox();
                    container.adder.clear();

                    this.Controls.Remove(container.adder.panel);
                }
                else
                {
                    MessageBox.Show("Devi prima inserire la posizione del programma!");
                }
            }
            else
            {
                container.adder.setAlias(container.adder.getAliasfromTxt());
                container.adder.setDir(container.adder.getDirfromTxt());
                container.adder.setGamemode();

                if (container.getWhat() == 1)
                {
                    //CERCO NELLA LISTA GIOCHI CHI HA toMod = 1
                    foreach(PanelEx panel in container.allPanelGames)
                    {
                        if(panel.gettoModify() == true)
                        {
                            foreach(ProgramConfiguration pg in container.ListConfigGames)
                            {
                                if (panel.getlblAlias().Equals(pg.getAlias()))
                                {
                                    panel.setAlias(container.adder.getAlias());
                                    panel.setDir(container.adder.getDir());
                                    pg.setAlias(panel.getlblAlias());
                                    pg.setDir(panel.getDir());
                                    pg.setNome(container.adder.getName());

                                    panel.setMod(0);
                                    container.setPBox();
                                }
                            }
                        }
                    }
                }
                else
                {
                    //CERCO NELLA LISTA SOFTWARE CHE HA toMod = 1
                    foreach (PanelEx panel in container.allPanel)
                    {
                        if (panel.gettoModify() == true)
                        {
                            foreach (ProgramConfiguration pg in container.ListConfig)
                            {
                                if (panel.getlblAlias().Equals(pg.getAlias()))
                                {
                                    panel.setAlias(container.adder.getAlias());
                                    panel.setDir(container.adder.getDir());
                                    pg.setAlias(panel.getlblAlias());
                                    pg.setDir(panel.getDir());
                                    pg.setNome(container.adder.getName());

                                    panel.setMod(0);
                                    container.setPBox();
                                }
                            }
                        }
                    }
                }
                container.adder.clear();
                this.Controls.Remove(container.adder.panel);
                this.is_customizating = false;
            }
        }
        private void subCancelbtn_Click(object sender, EventArgs e) 
        {
            this.Controls.Remove(container.adder.panel);
        }
        private void subChoosebtn_Click(object sender, EventArgs e)
        {
            container.adder.SubOFD.ShowDialog();

            container.adder.setName(container.adder.SubOFD.SafeFileName.Replace(".exe", "").Replace(".EXE", ""));
            container.adder.setDirtxt(container.adder.SubOFD.FileName);
        }

        public void modifyConfig(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            is_customizating = true;

            if (button.Name.Substring(0, 4).Equals("ModG"))
            {
                int i = int.Parse(button.Name.Replace("ModG", ""));
                PanelEx p = container.allPanelGames.ElementAt(i);

                this.Controls.Remove(container.p1);
                this.Controls.Remove(container.p2);
                this.Controls.Add(container.adder.panel);
                container.adder.setGMod(1);
                container.adder.setAcDischeck(false);
                this.Controls.Add(container.p1);
                this.Controls.Add(container.p2);

                p.setMod(1);
                container.setWhat(1);

                //INIZIALIZZA ADDER PER LA CONFIGURAZIONE PREINIZIALIZZATA
                container.adder.setAcDischeck(false);
                container.adder.setAliastxt(p.getlblAlias());
                container.adder.setDirtxt(p.getDir());

                string[] foldertoName = p.getDir().Split('\\');
                container.adder.setName(foldertoName[foldertoName.Length - 1]);

            }
            if (button.Name.Substring(0, 4).Equals("ModS"))
            {
                int i = int.Parse(button.Name.Replace("ModS", ""));
                PanelEx p = container.allPanel.ElementAt(i);

                this.Controls.Remove(container.p1);
                this.Controls.Remove(container.p2);
                this.Controls.Add(container.adder.panel);
                container.adder.setGMod(0);
                container.adder.setAcDischeck(false);
                this.Controls.Add(container.p1);
                this.Controls.Add(container.p2);

                p.setMod(1);
                container.setWhat(0);

                //INIZIALIZZA ADDER PER LA CONFIGURAZIONE PREINIZIALIZZATA
                container.adder.setAcDischeck(false);
                container.adder.setAliastxt(p.getlblAlias());
                container.adder.setDirtxt(p.getDir());

                string[] foldertoName = p.getDir().Split('\\');
                container.adder.setName(foldertoName[foldertoName.Length - 1]);
            }

        }
        public void cancelConfig(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name.Substring(0, 5).Equals("CancG"))
            {
                int i = int.Parse(button.Name.Replace("CancG", ""));
                PanelEx p = container.allPanelGames.ElementAt(i);
                ProgramConfiguration pg = container.ListConfigGames.ElementAt(i);

                if (p.getlblAlias().Equals(pg.getAlias())){
                    container.ListConfigGames.Remove(pg);
                }

                container.p2.Controls.Remove(p.dynPanel);
                container.allPanelGames.Remove(p);
                
                container.updateNames(1, button);
            }
            if (button.Name.Substring(0, 5).Equals("CancS"))
            {
                int i = int.Parse(button.Name.Replace("CancS", ""));
                PanelEx p = container.allPanel.ElementAt(i);
                ProgramConfiguration pg = container.ListConfig.ElementAt(i);

                if (p.getlblAlias().Equals(pg.getAlias()))
                {
                    container.ListConfig.Remove(pg);
                }

                container.p1.Controls.Remove(p.dynPanel);
                container.allPanel.Remove(p);
                container.updateNames(0, button);
            }
        }
        //DA SISTEMARE removePaC e adjustContainer
        public void removePaC(string alias)
        {

            foreach (PanelEx panel in container.allPanel)
            {

                if (panel.getlblAlias().Equals(alias))
                {
                    toRemove = container.allPanel.IndexOf(panel);
                    container.allPanel.Remove(panel);
                    container.p1.Controls.Remove(panel.dynPanel);
                    adjustContainer();
                    break;
                }
            }
        }
        public void adjustContainer()
        {
            int counter = 0;
            foreach (PanelEx panel in container.allPanel)
            {
                if (counter < toRemove)
                {
                    counter++;
                }
                else
                {
                    panel.dynPanel.Location = new Point(panel.dynPanel.Location.X, panel.dynPanel.Location.Y - panel.dynPanel.Height);
                }
            }
        }
    }
    
}
