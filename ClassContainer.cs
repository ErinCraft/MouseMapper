using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using System.Diagnostics;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using MouseMapper.Properties;
using System.Windows;

namespace MouseMapper
{
    class ClassContainer
    {
        //VARIABILI
        #region variabili  
        //PANNELLO CONTENITORE - POSIZIONE E SIZE
        private const int wP1 = 716;
        private const int hP1 = 526;
        private const int lP1x = 8;
        private const int lP1y = 44;

        //BOTTONE ELIMINA CONFIGURAZIONE - POSIZIONE E SIZE
        private const int whBd = 25;
        private const int lBdx = 661;
        private const int lBdy = 77;

        //CHECKBOX - POSIZIONE E SIZE
        private const int lCbx = 476;
        private const int lCby = 80;

        //LABEL PER LISTE VUOTE
        private const int lOpsx = 319;
        private const int lOpsy = 176;
        private const int lNinsx = 231;
        private const int lNinsy = 202; 
        private const int lhowtoAx = 208;
        private const int lhowtoAy = 228; 
        private const int lAsoftgamesx = 203;
        private const int lAsoftgamesy = 254; 
        private const int lIdentx = 53;
        private const int lIdenty = 280;

        //OGGETTI GRAFICI
        //PANNELLO CONTENITORE
        public Panel p1 = new Panel();
        public Panel p2 = new Panel();

        //SCROLLBAR VERTICALI
        public VScrollBar b1 = new VScrollBar();
        public VScrollBar b2 = new VScrollBar();

        //LABEL PER LISTA VUOTA
        Label Ops = new Label();
        Label Nins = new Label();
        Label howTo = new Label();
        Label AsoftGames = new Label();
        Label Identify = new Label(); 
        Label Opsg = new Label();
        Label Ninsg = new Label();
        Label Howtog = new Label();
        Label AddSG = new Label();
        Label IdG = new Label();

        //ALTRE VARIABILI
        int lastCheck = 0;
        public int conta = 0;  //DIPENDE DALLA LETTURA + AGGIUNTA MANUALE
        public int contasoftware = 0, contagames = 0;
        private int GameorSoftware;

        //LISTE
        public List<ProgramConfiguration> ListConfig = new List<ProgramConfiguration>();  //UTILIZZO UNA LISTA PER ARCHIVIARE GLI OGGETTI E POTERLI RECUPERARE QUANDO NE NECESSITO
        public List<ProgramConfiguration> ListConfigGames = new List<ProgramConfiguration>();
        public List<ProgramConfiguration> AllConfig = new List<ProgramConfiguration>();
        public List<PanelEx> allPanel = new List<PanelEx>();
        public List<PanelEx> allPanelGames = new List<PanelEx>();
        private List<Button> allDelete = new List<Button>();
        private List<Button> allDeleteGames = new List<Button>();

        public SubForm adder = new SubForm();
        #endregion

        //COSTRUTTORE
        public ClassContainer() {

            p1.Size = p2.Size = new Size(wP1, hP1);
            p1.Location = p2.Location = new Point(lP1x, lP1y);
            p1.BorderStyle = p2.BorderStyle = BorderStyle.FixedSingle;

            b1.Dock = b2.Dock = DockStyle.Right;
            b1.Visible = b2.Visible = true;
            b1.SmallChange = b1.LargeChange = b2.SmallChange = b2.LargeChange = 1;

            Ops.Location = new Point(lOpsx, lOpsy);
            Nins.Location = new Point(lNinsx, lNinsy);
            howTo.Location = new Point(lhowtoAx, lhowtoAy);
            AsoftGames.Location = new Point(lAsoftgamesx, lAsoftgamesy);
            Identify.Location = new Point(lIdentx, lIdenty);

            Ops.Text = "Ops!";
            howTo.Text = "Per aggiungerne uno, clicca sul";
            AsoftGames.Text = "Menù -> Programmi -> Aggiungi";
            Ops.Font = AsoftGames.Font = new Font(new Font("calibri", 16f), System.Drawing.FontStyle.Bold);
            Nins.Font = howTo.Font = Identify.Font = new Font("calibri light", 16f);
            Ops.AutoSize = howTo.AutoSize = AsoftGames.AutoSize = Identify.AutoSize = Nins.AutoSize = true;
            
            Opsg.Location = Ops.Location;
            Opsg.Font = Ops.Font;
            Opsg.Text = Ops.Text;
            
            Ninsg.Location = Nins.Location;
            Ninsg.Font = Nins.Font;
            Ninsg.Text = Nins.Text;

            Howtog.Location = howTo.Location;
            Howtog.Font = howTo.Font;
            Howtog.Text = howTo.Text;

            AddSG.Location = AsoftGames.Location;
            AddSG.Font = AsoftGames.Font;
            AddSG.Text = AsoftGames.Text;

            IdG.Location = Identify.Location;
            IdG.Font = Identify.Font;
            IdG.Text = Identify.Text;

            Opsg.AutoSize = Ninsg.AutoSize = Howtog.AutoSize = AddSG.AutoSize = IdG.AutoSize = true;
            Ops.BackColor = Opsg.BackColor = Nins.BackColor = Ninsg.BackColor = howTo.BackColor = Howtog.BackColor = AddSG.BackColor = AsoftGames.BackColor = Identify.BackColor = IdG.BackColor = Color.Transparent; 
            checkIfEmpty();

            b1.Scroll += new ScrollEventHandler(b1Scroll);
            b2.Scroll += new ScrollEventHandler(b2Scroll);

            p1.Controls.Add(b1);
            p2.Controls.Add(b2);

            GameorSoftware = 0;
           
        }
        //METODI ESTETICI
        #region metodi estetici
        //AGGIORNA LE PICTUREBOX DELLE CONFIGURAZIONI CON LE ICONE ASSOCIATE AL PROGRAMMA
        public void setPBox()
        {
            foreach (PanelEx panel in allPanel)
            {
                foreach (ProgramConfiguration pg in ListConfig)
                {
                    if (panel.dlabel.Text.Equals(pg.getAlias()) && !pg.getNome().Equals(""))
                    {
                        Icon x = Icon.ExtractAssociatedIcon(pg.getDir());
                        Image y = x.ToBitmap();
                        panel.setImage(y);
                    }
                }

            }
            foreach (PanelEx panel in allPanelGames)
            {
                foreach (ProgramConfiguration pg in ListConfigGames)
                {
                    if (panel.dlabel.Text.Equals(pg.getAlias()) && !pg.getNome().Equals(""))
                    {
                        Icon x = Icon.ExtractAssociatedIcon(pg.getDir());
                        Image y = x.ToBitmap();
                        panel.setImage(y);
                    }
                }

            }
        }
        public void checkIfEmpty()
        {
            if (ListConfig.Count() == 0 && p1.Contains(Ops) == false)
            {
                p1.Controls.Add(Ops);
                p1.Controls.Add(Nins);
                Nins.Text = "Nessun software inserito!";
                p1.Controls.Add(howTo);
                p1.Controls.Add(AsoftGames);
                p1.Controls.Add(Identify);
                Identify.Text = "crea un Alias, trova il tuo programma e identificalo come software";
            }
            if (ListConfigGames.Count() == 0 && p2.Contains(Ops) == false)
            {
                p2.Controls.Add(Opsg);
                p2.Controls.Add(Ninsg);
                Ninsg.Text = "Nessun gioco inserito!";
                p2.Controls.Add(Howtog);
                p2.Controls.Add(AddSG);
                p2.Controls.Add(IdG);
                IdG.Text = "crea un Alias, trova il tuo programma e identificalo come gioco";
            }
            if (ListConfig.Count != 0 && p1.Contains(Ops) == true){
                p1.Controls.Remove(Ops);
                p1.Controls.Remove(Nins);
                p1.Controls.Remove(howTo);
                p1.Controls.Remove(AsoftGames);
                p1.Controls.Remove(Identify);
            }
            if (ListConfigGames.Count != 0 && p2.Contains(Opsg) == true){
                p2.Controls.Remove(Opsg);
                p2.Controls.Remove(Ninsg);
                p2.Controls.Remove(Howtog);
                p2.Controls.Remove(AddSG);
                p2.Controls.Remove(IdG);
            }
        }
        #endregion

        //AGGIUNTA EVENTHANDLERS
        public void addHandlers(EventHandler add, EventHandler cancel, EventHandler choose)
        {
            adder.setaddev(add);
            adder.setcanceldev(cancel);
            adder.setchoosedev(choose);
        }
        public void setWhat(int x)
        {
            GameorSoftware = x;
        }
        public int getWhat()
        {
            return GameorSoftware;
        }
        public void addtoList(Button b)
        {
            allDelete.Add(b);
        }
        public void addtoListGames(Button b)
        {
            allDeleteGames.Add(b);
        }
        public void updateNames(int x, Button btR)
        {
            if(x == 0)
            {
                allDelete.Remove(btR);
                int index = 0;
                foreach(Button b in allDelete)
                {
                    b.Name = "CancS" + index.ToString();
                    index++;
                }
            }
            else
            {
                allDeleteGames.Remove(btR);
                int index = 0;
                foreach (Button b in allDeleteGames)
                {
                    b.Name = "CancG" + index.ToString();
                    index++;
                }
            }
        }
        //EVENTHANDLERS
        #region eventhandlers
        //EVENTI DEDICATI A VSCROLLBARS
        private void b1Scroll (object sender, ScrollEventArgs e)
        {
            if (lastCheck < b1.Maximum)
            {
                if (b1.Value > lastCheck)
                {
                    foreach (PanelEx panel in allPanel)
                    {
                        panel.dynPanel.Location = new Point(panel.dynPanel.Location.X, (panel.dynPanel.Location.Y) - b1.Value + lastCheck);
                    }

                }
                else if (b1.Value < lastCheck)
                {
                    foreach (PanelEx panel in allPanel)
                    {
                        panel.dynPanel.Location = new Point(panel.dynPanel.Location.X, (panel.dynPanel.Location.Y) - b1.Value + lastCheck);
                    }
                }
            }
            lastCheck = b1.Value;

        }
        private void b2Scroll(object sender, ScrollEventArgs e)
        {
            if (lastCheck < b1.Maximum)
            {
                if (b2.Value > lastCheck)
                {
                    foreach (PanelEx panel in allPanelGames)
                    {
                        panel.dynPanel.Location = new Point(panel.dynPanel.Location.X, (panel.dynPanel.Location.Y) - b1.Value + lastCheck);
                    }

                }
                else if (b1.Value < lastCheck)
                {
                    foreach (PanelEx panel in allPanelGames)
                    {
                        panel.dynPanel.Location = new Point(panel.dynPanel.Location.X, (panel.dynPanel.Location.Y) - b1.Value + lastCheck);
                    }
                }
            }
            lastCheck = b2.Value;

        }
        //EVENTI DEDICATI A DELETEBUTTON
        private void deleteAll(object sender, EventArgs e)
        {
            
            Button button = (Button)sender;

            string filter = button.Name.Replace("deleteb", "");

            int getindex = int.Parse(filter);

            conta--;

            int counter = 0;
            foreach (PanelEx panel in allPanel)
            {
                if (counter != getindex)
                {
                    counter++;
                }
                else
                {
                    p1.Controls.Remove(panel.dynPanel);
                    allPanel.Remove(panel);
                    break;
                }
            }

            counter = 0;
            foreach (ProgramConfiguration config in ListConfig)
            {
                if (counter != getindex)
                {
                    counter++;
                }
                else
                {
                    ListConfig.Remove(config);
                    break;
                }
            }
            counter = 0;
            foreach (PanelEx panel in allPanel)
            {
                if (counter != getindex)
                {
                    counter++;
                }
                else
                {
                    panel.dynPanel.Location = new Point(panel.dynPanel.Location.X, panel.dynPanel.Location.Y - panel.dynPanel.Height - 5);
                }
            }

        }   //DA SISTEMARE
        //EVENTI DEDICATI A PANELEX.LABELALIAS
        public  void changeAlias(object sender, EventArgs e)
        {
            Label x = (Label)sender;

            string s = Microsoft.VisualBasic.Interaction.InputBox("Inserisci il nuovo Alias:", "Alias", "Default Response");

            foreach (ProgramConfiguration pg in ListConfig)
            {
                if (pg.getAlias().Equals(x.Text))
                {
                    x.Text = s;
                    pg.setAlias(x.Text);
                    sender = x;
                    break;
                }
            }

        }   //DA SISTEMARE
        #endregion

    }
}
