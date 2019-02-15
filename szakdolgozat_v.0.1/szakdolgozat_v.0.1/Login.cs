using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace szakdolgozat_v._0._1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        static database db = new database();
        string megfelelt(string nev, string jelszo)
        {
            string pw = titkositas(jelszo);
            string type = db.erteket_ad(string.Format(@"SELECT type FROM users WHERE user_name = '{0}' AND password = '{1}';", nev, pw));
            return type;
        }
        static string titkositas(string jelszo)
        {
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(Encoding.Unicode.GetBytes(jelszo));
            return Convert.ToBase64String(inArray);
        }
        private void L_SINGIN_Btn_Click(object sender, EventArgs e)
        {
            string belephet = megfelelt(user_name_TBox.Text, password_TBox.Text);
            //MessageBox.Show(belephet);
            if (belephet == "True")
            {
                this.Hide();
                AdminLog al = new AdminLog();
                al.Show();
            }
            else if (belephet == "False")
            {
                this.Hide();
                UserLog ul = new UserLog();
                ul.Show();
            }
            else
            {
                MessageBox.Show("Nem létezik ilyen felhasználó vagy rossz jelszó.", "Hibás adatok", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}