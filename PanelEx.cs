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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace MouseMapper
{
    public class PanelEx
    {

        //LOCATION E SIZE DEI CONTROLLI. VERRANNO INDICATE COME DELLE COSTANTI IN QUANTO SEMPLIFICA LA GESTIONE DELLE FUNZIONI DIMINUENDONE IL NUMERO DI PARAMETRI 

        //PANNELLO DI CONTENIMENTO
        private const int wP = 691;
        private const int hP = 50;
        private const int lPx = 4;
        private const int lPy = 5;

        //PICTUREBOX: DUE SOLI PARAMETRI IN QUANTO HA FORMA QUADRATA. INOLTRE LA DISTANZA DAI BORDI È EQUIVALENTE IN TUTTI E DUE LATI SINISTRO E SUPERIORE
        private const int whPb = 42;    
        private const int lPB = 3;      

        //LABEL NOME ALIAS
        private const int lLblx = 48;
        private const int lLbly = 13;

        //CASELLE DI TESTO. SI PRENDERANNO IN ESAME SOLO I VALORI CHE CAMBIANO: ESSENDO CHE LE CASELLE SONO A DUE A DUE PARELLE; LE MISURE SARANNO IN COMUNE
        private const int wTxt = 140;
        private const int hTxt = 20;
        private const int lTxt12x = 95;
        private const int lTxt34x = 312;
        private const int lTxt5x = 522;
        private const int lTxt24y = 77;
        private const int lTxt135y = 51;

        //LABEL ASSOCIATI ALLE CASELLE DI TESTO. VALE LO STESSO PRINCIPIO
        private const int lLbl12x = 55;
        private const int lLbl34x = 256;
        private const int lLbl5x = 471;
        private const int lLbl135y = 54;
        private const int lLbl24y = 80;

        //BOTTONE ELIMINA / SCEGLI
        private const int wCbtn = 64;
        private const int wMbtn = 87;
        private const int hCMbtn = 25;

        private const int lCbx = 622;
        private const int lMbx = 529;
        private const int lMCby = 13;

        //DICHIARAZIONE DEGLI OGGETTI

        //PANNELLO CONTENITORE
        public Panel dynPanel = new Panel();

        //PICTUREBOX CHE CATTURA L'ICONA DEL PROGRAMMA
        private PictureBox dpB = new PictureBox();

        //LABEL PER IL TITOLO DELLA CONFIGURAZIONE
        public Label dlabel = new Label();

        //TEXTBOX DA AGGIUNGERE AL PANNELLO
        private TextBox txt1 = new TextBox();
        private TextBox txt2 = new TextBox();
        private TextBox txt3 = new TextBox();
        private TextBox txt4 = new TextBox();
        private TextBox txt5 = new TextBox();

        //LABEL DA AGGIUNGERE AL PANNELLO
        private Label lbl1 = new Label();
        private Label lbl2 = new Label();
        private Label lbl3 = new Label();
        private Label lbl4 = new Label();
        private Label lbl5 = new Label();

        //BOTTONI ELIMINA E MODIFICA
        public Button Cancelbtn = new Button();
        private Button Modifybtn = new Button();

        //TIMER PER LE ANIMAZIONI
        private System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer t2 = new System.Windows.Forms.Timer();

        //VARIABILI RICHIESTE PRIVATE
        private string file, dir;
        private bool toMod;

        private void Timer_Expand(object sender, EventArgs e)
        {
            if (dynPanel.Height == 108)
                t1.Stop();
            else dynPanel.Height+=2;
        }
        private void Timer_Collapse(object sender, EventArgs e)
        {
            if (dynPanel.Height == 50)
                t2.Stop();
            else dynPanel.Height-=2;
        }
        private void dyn_click(object sender, EventArgs e)
        {
            if (dynPanel.Height == 50)
            {
                t1.Start();
            }
            else if (dynPanel.Height == 108)
                t2.Start();
        }
        //METODI PUBBLICI

        //COSTRUTTORE
        public PanelEx(int index)
        {
            dynPanel.Size = new Size(wP, hP);
            if(index == 0)
                dynPanel.Location = new Point(lPx, (lPy*index) + 5);
            else
                dynPanel.Location = new Point(lPx, (lPy * index) + (hP*index) + 5);

            dynPanel.BorderStyle = BorderStyle.FixedSingle;
            dynPanel.BackColor = Color.FromArgb(Properties.Settings.Default.R, Properties.Settings.Default.G, Properties.Settings.Default.B);

            dynPanel.TabIndex = 0;

            dynPanel.Visible = true;

            //AGGIUNGO PULSANTI
            Cancelbtn.Size = new Size(wCbtn, hCMbtn);
            Modifybtn.Size = new Size(wMbtn, hCMbtn);

            Cancelbtn.Location = new Point(lCbx, lMCby);
            Modifybtn.Location = new Point(lMbx, lMCby);

            Cancelbtn.FlatStyle = Modifybtn.FlatStyle = FlatStyle.Flat;
            Cancelbtn.Font = Modifybtn.Font = Properties.Settings.Default.DefaulttxtFont;
            Cancelbtn.Text = "Elimina";
            Modifybtn.Text = "Modifica";

            //IMPOSTO I TIMER E GLI HANDLER ASSOCIATI
            dynPanel.Click += new EventHandler(dyn_click);

            t1.Interval = t2.Interval = 10;
            t1.Tick += new EventHandler(Timer_Expand);
            t2.Tick += new EventHandler(Timer_Collapse);

            dir = "";
            file = "";
            toMod = false;

        }
        public void addLabel(string t = "Not Set")
        {

            dlabel.Font = Properties.Settings.Default.DefaultlblFont;
            if (t.Equals(""))
                dlabel.Text = "Not Set";
            else
                dlabel.Text = t;
            dlabel.AutoSize = true;

            dlabel.Location = new Point (lLblx, lLbly);

            dynPanel.Controls.Add(dlabel);
        }
        public void addPBox (Image bimg)
        { 
            dpB.Size = new Size(whPb, whPb);

            dpB.Location = new Point (lPB, lPB);

            dpB.BorderStyle = BorderStyle.None;

            dpB.BackgroundImageLayout = ImageLayout.Stretch;

            dpB.BackgroundImage = bimg;


            dynPanel.Controls.Add(dpB);
        }
        public void addTbox(string text1, string text2, string text3, string text4, string text5)
        {
            //PRINTF("TI AMO <3!")

            txt1.Size = txt2.Size = txt3.Size = txt4.Size = txt5.Size = new Size(wTxt, hTxt);

            txt1.Location = new Point(lTxt12x, lTxt135y);
            txt2.Location = new Point(lTxt12x, lTxt24y); 
            txt3.Location = new Point(lTxt34x, lTxt135y); 
            txt4.Location = new Point(lTxt34x, lTxt24y); 
            txt5.Location = new Point(lTxt5x, lTxt135y);

            txt1.Text = text1;
            txt2.Text = text2;
            txt3.Text = text3;
            txt4.Text = text4;
            txt5.Text = text5;

            dynPanel.Controls.Add(txt1);
            dynPanel.Controls.Add(txt2);
            dynPanel.Controls.Add(txt3);
            dynPanel.Controls.Add(txt4);
            dynPanel.Controls.Add(txt5);

        }
        public void addTLbl()
        {
            //PRINTF("TI AMO <3!")

            lbl1.Location = new Point(lLbl12x, lLbl135y);
            lbl2.Location = new Point(lLbl12x, lLbl24y);
            lbl3.Location = new Point(lLbl34x, lLbl135y);
            lbl4.Location = new Point(lLbl34x, lLbl24y); 
            lbl5.Location = new Point(lLbl5x, lLbl135y);
            
            lbl1.Text = "Sinistro";
            lbl2.Text = "Destro";
            lbl3.Text = "Laterale 1";
            lbl4.Text = "Laterale 2";
            lbl5.Text = "Centrale";

            lbl1.Font = lbl2.Font = lbl2.Font = lbl3.Font = lbl4.Font = lbl5.Font = Properties.Settings.Default.DefaulttxtFont;

            dynPanel.Controls.Add(lbl1);
            dynPanel.Controls.Add(lbl2);
            dynPanel.Controls.Add(lbl3);
            dynPanel.Controls.Add(lbl4);
            dynPanel.Controls.Add(lbl5);

        }
        public void addButtons(EventHandler Cancel, EventHandler Modify, string namecancel, string namemodify)
        {
            Cancelbtn.Name = namecancel;
            Modifybtn.Name = namemodify;

            Cancelbtn.Click += Cancel;
            Modifybtn.Click += Modify;

            dynPanel.Controls.Add(Cancelbtn);
            dynPanel.Controls.Add(Modifybtn);
        }
        public string getlblAlias()
        {
            return dlabel.Text;
        }
        public string getLeft()
        {
            return txt1.Text;
        }
        public string getRight()
        {
            return txt2.Text;
        }
        public string getMiddle()
        {
            return txt5.Text;
        }
        public string getXb1()
        {
            return txt4.Text;
        }
        public string getXb2()
        {
            return txt3.Text;
        }
        public string getFilename()
        {
            return file;
        }
        public string getDir()
        {
            return dir;
        }

        public bool gettoModify()
        {
            return toMod;
        }
        public void setMod(int x)
        {
            if (x == 0)
            {
                toMod = false;
            }
            else toMod = true;
        }
        public void setDir(string s)
        {
            dir = s;
        }
        public void setImage(Image bimg)
        {
            dpB.BackgroundImage = bimg;
        }
        public void setAlias(string s)
        {
            dlabel.Text = s;
        }
    }
}