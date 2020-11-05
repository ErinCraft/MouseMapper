using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseMapper
{
    class SubForm
    {
        #region variabili
        //SIZE E LOCATION DI PANEL
        private const int wP = 511;
        private const int hP = 271;
        private const int lPx = 109;
        private const int lPy = 148;
        //SIZE E LOCATION DI TEXTBOXES
        private const int wTxt = 300;
        private const int hTxt = 27;
        private const int lTxt12x = 103;
        private const int lTxt1y = 71;
        private const int lTxt2y = 140;
        //SIZE E LOCATION DI LABEL
        private const int lTitlex = -1;
        private const int lTitley = 1;
        private const int lAliaslblx = 143;
        private const int lAliaslbly = 49;
        private const int lDirlblx = 164;
        private const int lDirlbly = 118;
        //SIZE E LOCATION DI RADIOBUTTON
        private const int lRdbSoftx = 103;
        private const int lRdbGamex = 275;
        private const int lRdbSGy = 185;
        //SIZE E LOCATION DI BUTTON
        private const int wChoose = 63;
        private const int hChoose = 27;
        private const int wAddCancel = 119;
        private const int hAddCancel = 43;
        private const int lChoosex = 409;
        private const int lChoosey = 140;
        private const int lAddx = 254;
        private const int lCancelx = 136;
        private const int lAddCancely = 227;

        //OGGETTI
        #region oggetti
        //PANEL
        public Panel panel = new Panel();
        //TEXTBOXES
        private TextBox aliastxt = new TextBox();
        private TextBox dirtxt = new TextBox();
        //LABEL
        private Label title = new Label();
        private Label aliaslbl = new Label();
        private Label dirlbl = new Label();
        //RADIOBUTTON
        private RadioButton softwarechoose = new RadioButton();
        private RadioButton gameschoose = new RadioButton();
        //BUTTON
        private Button choosebtn = new Button();
        private Button addbtn = new Button();
        private Button cancelbtn = new Button();

        //FILEDIALOG PER LA SCELTA DEL PROGRAMMA
        public OpenFileDialog SubOFD = new OpenFileDialog();

        //ALTRE VARIABILI
        private string alias;
        private string filename;
        private string dir;
        private int sgmod;
        #endregion

        #endregion
        //COSTRUTTORE
        public SubForm()
        {
            //DEFINISCO IL PANEL
            panel.Size = new Size(wP, hP);
            panel.Location = new Point(lPx, lPy);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.BackColor = Color.FromArgb(Properties.Settings.Default.Rf, Properties.Settings.Default.Gf, Properties.Settings.Default.Bf);

            //DEFINISCO LE TEXTBOX
            aliastxt.Size = dirtxt.Size = new Size(wTxt, hTxt);
            aliastxt.Location = new Point(lTxt12x, lTxt1y);
            dirtxt.Location = new Point(lTxt12x, lTxt2y);
            dirtxt.Enabled = false;
            aliastxt.Font = dirtxt.Font = Properties.Settings.Default.DefaultSubText;

            //DEFINISCO I LABEL
            title.Location = new Point(lTitlex, lTitley);
            aliaslbl.Location = new Point(lAliaslblx, lAliaslbly);
            dirlbl.Location = new Point(lDirlblx, lDirlbly);
            title.AutoSize = aliaslbl.AutoSize = dirlbl.AutoSize = true;
            title.Font = aliaslbl.Font = dirlbl.Font = Properties.Settings.Default.DefaultSubText;
            title.Text = "MouseMapper -  Crea una nuova configurazione";
            aliaslbl.Text = "Scegli l'alias per la configurazione:";
            dirlbl.Text = "Posizione del programma:";

            //DEFINISCO I RADIOBUTTON
            softwarechoose.Location = new Point(lRdbSoftx, lRdbSGy);
            gameschoose.Location = new Point(lRdbGamex, lRdbSGy);
            softwarechoose.AutoSize = gameschoose.AutoSize = true;
            softwarechoose.Font = gameschoose.Font = Properties.Settings.Default.DefaulttxtFont;
            softwarechoose.Text = "Identifica come software";
            gameschoose.Text = "Identifica come gioco";

            //DEFINISCO I BUTTON
            choosebtn.Size = new Size(wChoose, hChoose);
            addbtn.Size = cancelbtn.Size = new Size(wAddCancel, hAddCancel);
            choosebtn.Location = new Point(lChoosex, lChoosey);
            addbtn.Location = new Point(lAddx, lAddCancely);
            cancelbtn.Location = new Point(lCancelx, lAddCancely);
            choosebtn.Font = addbtn.Font = cancelbtn.Font = Properties.Settings.Default.DefaultSubText;
            choosebtn.FlatStyle = addbtn.FlatStyle = cancelbtn.FlatStyle = FlatStyle.Flat;
            cancelbtn.Text = "Annulla";
            addbtn.Text = "Aggiungi";
            choosebtn.Text = "...";

            //AGGIUNGO I CONTROLLI A panel
            panel.Controls.Add(title);
            panel.Controls.Add(aliaslbl);
            panel.Controls.Add(dirlbl);
            panel.Controls.Add(aliastxt);
            panel.Controls.Add(dirtxt);
            panel.Controls.Add(choosebtn);
            panel.Controls.Add(cancelbtn);
            panel.Controls.Add(addbtn);
            panel.Controls.Add(softwarechoose);
            panel.Controls.Add(gameschoose);

            //AGGIUNGO I FILTRI
            SubOFD.Filter = "Exectuable Files (*.exe)|*.exe";

            //INIZIALIZZO LE VARIABILI PRIVATE STRING E INT
            alias = "";
            filename = "";
            dir = "";
            sgmod = -1;

        }

        //SETTER
        #region setter
        public void setAlias(string s)
        {
            alias = s;
        }
        public void setAliastxt(string s)
        {
            aliastxt.Text = s;
        }
        public void setName(string s)
        {
            filename = s;
        }
        public void setDir(string s)
        {
            dir = s;
        }
        public void setDirtxt(string s)
        {
            dirtxt.Text = s;
        }
        public void setGMod(int x)
        {
            if (x == 0)
                sgmod = 0;
            else
                sgmod = 1;
            setGamemode();
        }
        public void setGamemode()
        {
            if (gameschoose.Checked || sgmod == 1)
            {
                sgmod = 1;
                gameschoose.Checked = true;
            }
            else
            {
                sgmod = 0;
                softwarechoose.Checked = true;
            }
        }
        public void setaddev (EventHandler e)
        {
            addbtn.Click += e;
        }
        public void setcanceldev(EventHandler e)
        {
            cancelbtn.Click += e;
        }
        public void setchoosedev(EventHandler e)
        {
            choosebtn.Click += e;
        }
        //SETTARE DISABLE CHECKBOXES PER MODIFICABUTTON
        public void setAcDischeck(bool active)
        {
            if(active == true)
                gameschoose.Enabled = softwarechoose.Enabled = true;
            else
                gameschoose.Enabled = softwarechoose.Enabled = false;
        }
        public void clear()
        {
            setDirtxt("");
            setDir("");
            setAlias("");
            setAliastxt("");
            setName("");
            softwarechoose.Checked = false;
            gameschoose.Checked = false;
        }
        #endregion

        //GETTER
        #region getter
        public string getAlias() {
            return alias;
        }
        public string getName()
        {
            return filename;
        }
        public string getDir()
        {
            return dir;
        }
        public int getSGmod()
        {
            return sgmod;
        }
        //DA FORM
        public string getAliasfromTxt()
        {
            return aliastxt.Text;
        }
        public string getDirfromTxt()
        {
            return dirtxt.Text;
        }
        #endregion
    }
}
