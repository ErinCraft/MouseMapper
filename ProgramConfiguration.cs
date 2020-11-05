using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseMapper
{
    public class ProgramConfiguration
    {
        private string nome;
        private string alias;
        private string dir;
        private string left;
        private string right;
        private string middle;
        private string xb1;
        private string xb2;
        private int gamemod;

        private ushort decode(string todecode)
        {
            switch (todecode)
            {
                case "q":
                    return 0x10;
                case "w":
                    return 0x11;
                case "e":
                    return 0x12;
                case "r":
                    return 0x13;
                case "t":
                    return 0x14;
                case "y":
                    return 0x15;
                case "u":
                    return 0x16;
                case "i":
                    return 0x17;
                case "o":
                    return 0x18;
                case "p":
                    return 0x19;
                case "a":
                    return 0x1e;
                case "s":
                    return 0x1f;
                case "d":
                    return 0x20;
                case "f":
                    return 0x21;
                case "g":
                    return 0x22;
                case "h":
                    return 0x23;
                case "j":
                    return 0x24;
                case "k":
                    return 0x25;
                case "l":
                    return 0x26;
                case "z":
                    return 0x2c;
                case "x":
                    return 0x2d;
                case "c":
                    return 0x2e;
                case "v":
                    return 0x2f;
                case "b":
                    return 0x30;
                case "n":
                    return 0x31;
                case "m":
                    return 0x32;
                case "1":
                    return 0x02;
                case "2":
                    return 0x03;
                case "3":
                    return 0x04;
                case "4":
                    return 0x05;
                case "5":
                    return 0x06;
                case "6":
                    return 0x07;
                case "7":
                    return 0x08;
                case "8":
                    return 0x9;
                case "9":
                    return 0x0a;
                case "0":
                    return 0x0b;
                default:
                    return 0x00;
            }
        }
        private string checkcases(string toCheck)
        {
            switch (toCheck)
            {
                case "CTRL":
                    return "^";
                case "SHIFT":
                    return "+";
                case "ALT":
                    return "%";
                //CASO PARTICOLARE: I TASTI COINCIDONO ESATTAMENTE CON I RETURN DI SOPRA: PER EVITARE DISCORDIE NEL METODO SendKeys() SI PONE LO STESSO RISULTATO TRA PARENTESI GRAFFE
                case "^":
                    return "{^}";
                case "+":
                    return "{+}";
                case "%":
                    return "{%}";
                //ALTRI TASTI POSSIBILI
                case "ENT":
                    return "{ENTER}";
                case "DEL":
                    return "{DEL}";
                case "BACK":
                    return "{BS}";
                case "BREAK":
                    return "{BREAK}";
                //TASTI FRECCA
                case "UP":
                    return "{UP}";
                case "LEFT":
                    return "{LEFT}";
                case "RIGHT":
                    return "{RIGHT}";
                case "DOWN":
                    return "{DOWN}";
                case "PGUP":
                    return "{PGUP}";
                case "PGDOWN":
                    return "{PGDN}";
                //TASTI FUNZIONE
                case "F1":
                    return "{F1}";
                case "F2":
                    return "{F2}";
                case "F3":
                    return "{F3}";
                case "F4":
                    return "{F4}";
                case "F5":
                    return "{F5}";
                case "F6":
                    return "{F6}";
                case "F7":
                    return "{F7}";
                case "F8":
                    return "{F8}";
                case "F9":
                    return "{F9}";
                case "F10":
                    return "{F10}";
                case "F11":
                    return "{F11}";
                case "F12":
                    return "{F12}";
                //TASTO INVARIATO
                case "left":
                case "right":
                case "middle":
                case "xb1":
                case "xb2":
                    return "";

                default:
                    return toCheck;
            }
        }
        
        public ProgramConfiguration()
        {
            nome = "";
            alias = "";
            dir = "";
            left = "";
            right = "";
            middle = "";
            xb1 = "";
            xb2 = "";
            gamemod = 0;
        }
        public ProgramConfiguration(string s)
        {
            //FILTRO LA STRINGA s CORRISPONDENTE ALLA PORZIONE DI TESTO PER LA CONFIGURAZIONE DEL PROGRAMMA
            string[] prop;
            prop = s.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            prop[1] = prop[1].Replace("name=", "");
            setNome(prop[1]);

            prop[2] = prop[2].Replace("alias=", "");
            setAlias(prop[2]);

            prop[3] = prop[3].Replace("dir=", "");
            setDir(prop[3]);

            prop[4] = prop[4].Replace("left=", "");
            setLeft(prop[4]);

            prop[5] = prop[5].Replace("right=", "");
            setRight(prop[5]);

            prop[6] = prop[6].Replace("middle=", "");
            setMiddle(prop[6]);

            prop[7] = prop[7].Replace("xb2=", "");
            setXb2(prop[7]);

            prop[8] = prop[8].Replace("xb1=", "");
            setXb1(prop[8]);

            prop[9] = prop[9].Replace("game=", "");
            setGameMode(prop[9]);
        }
        //GETTER
        public string getNome()
        {
            return nome;
        }
        public string getAlias()
        {
            return alias;
        }
        public string getDir()
        {
            return dir;
        }
        public string getLeft()
        {
            if (left.Equals("left"))
                return "Invariato";
            else return left;
        }
        public string getRight()
        {
            if (right.Equals("right"))
                return "Invariato";
            else return right;
        }
        public string getMiddle()
        {
            if (middle.Equals("middle"))
                return "Invariato";
            else return middle;
        }
        public string getXb1()
        {
            if (xb1.Equals("xb1"))
                 return "Invariato";
            else return xb1;
        }
        public string getXb2()
        {
            if (xb2.Equals("xb2"))
                return "Invariato";
            else return xb2;
        }
        public int getGameMode()
        {
            return gamemod;
        }
        //SETTER PER LA CONFIGURAZIONE MANUALE O LA MODIFICA
        public void setNome(string s)
        {
            nome = s;
        }
        public void setAlias(string s)
        {
            alias = s;
        }
        public void setLeft(string s)
        {
            left = s;
        }
        public void setDir(string s)
        {
            dir = s;
        }
        public void setRight(string s)
        {
            right = s;
        }
        public void setMiddle(string s)
        { 
            middle = s;
        }
        public void setXb1(string s)
        {
            xb1 = s;
        }
        public void setXb2(string s)
        {
            xb2 = s;
        }
        public void setGameMode(string s = "", int t = -1)
        {
            if (s.Equals(""))
                gamemod = t;
            else
                gamemod = int.Parse(s);
        }
        //CONVERSIONE PER IL METODO SendKeys()
        public ushort getLdecode(string l)
        {
            l = l.Replace(" ", "");
            string[] all = l.Split('+');

            for(int i = 0; i < all.Length; i++)
            {
                //SPLITTA I COMANDI 
            }

            ushort command = decode(l);

            return command;
        }
 
        public ushort getRdecode(string l)
        {
            l = l.Replace(" ", "");
            string[] all = l.Split('+');

            for (int i = 0; i < all.Length; i++)
            {
                //SPLITTA I COMANDI 
            }

            ushort command = decode(l);

            return command;
        }
        public ushort getMdecode(string l)
        {
            l = l.Replace(" ", "");
            string[] all = l.Split('+');

            for (int i = 0; i < all.Length; i++)
            {
                //SPLITTA I COMANDI 
            }

            ushort command = decode(l);

            return command;
        }
        public ushort getXb1decode(string l)
        {
            l = l.Replace(" ", "");
            string[] all = l.Split('+');

            for (int i = 0; i < all.Length; i++)
            {
                //SPLITTA I COMANDI 
            }

            ushort command = decode(l);

            return command;
        }
        public ushort getXb2decode(string l)
        {
            l = l.Replace(" ", "");
            string[] all = l.Split('+');

            for (int i = 0; i < all.Length; i++)
            {
                //SPLITTA I COMANDI 
            }

            ushort command = decode(l);

            return command;
        }
        public string getXb2Code()
        {
            string s = xb2;
            s = s.Replace(" ", "");
            string[] Keys = s.Split('+');

            string code = "";

            for(int i = 0; i < Keys.Length; i++)
            {
                code += checkcases(Keys[i]);
            }

            return code;
        }
        public string getLCode()
        {
            string s = left;
            s = s.Replace(" ", "");
            string[] Keys = s.Split('+');

            string code = "";

            for (int i = 0; i < Keys.Length; i++)
            {
                code += checkcases(Keys[i]);
            }

            return code;
        }
        public string getRCode()
        {
            string s = right;
            s = s.Replace(" ", "");
            string[] Keys = s.Split('+');

            string code = "";

            for (int i = 0; i < Keys.Length; i++)
            {
                code += checkcases(Keys[i]);
            }

            return code;
        }
        public string getMCode()
        {
            string s = middle;
            s = s.Replace(" ", "");
            string[] Keys = s.Split('+');

            string code = "";

            for (int i = 0; i < Keys.Length; i++)
            {
                code += checkcases(Keys[i]);
            }

            return code;
        }
        public string getXb1Code()
        {
            string s = xb1;
            s = s.Replace(" ", "");
            string[] Keys = s.Split('+');

            string code = "";

            for (int i = 0; i < Keys.Length; i++)
            {
                code += checkcases(Keys[i]);
            }

            return code;
        }
        //RESTITUISCE IL CODICE PER COMPILARE IL FILE .config, CHE SARÀ COMPOSTO DELLA CONCATENAZIONE DI TUTTI QUESTI PER OGNI OGGETTO CHE C'È
        public string getTxtCode()
        {
            string toCompile = "";

            toCompile += "#" + '\n';
            toCompile += "name=" + getNome() + '\n';
            toCompile += "alias=" + getAlias() + '\n';
            toCompile += "dir=" + getDir() + '\n';
            toCompile += "left=" + left + '\n';
            toCompile += "right=" + right + '\n';
            toCompile += "middle=" + middle + '\n';
            toCompile += "xb2=" + xb2 + '\n';
            toCompile += "xb1=" + xb1+ '\n';
            toCompile += "game=" + gamemod + '\n';

            return toCompile;
        }
    }

}
