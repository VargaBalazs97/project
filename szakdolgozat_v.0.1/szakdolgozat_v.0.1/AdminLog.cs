using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using Tulpep.NotificationWindow;
using System.Text.RegularExpressions;

namespace szakdolgozat_v._0._1
{
    public partial class AdminLog : Form
    {
        public AdminLog()
        {
            InitializeComponent();
        }
        #region operations
        //Változók
        static string strFileName = "";
        static string strFilePath = "";
        //Példányosítás
        static database db = new database();
        //Jelszó titkosítás
        static string titkositas(string jelszo)
        {
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(Encoding.Unicode.GetBytes(jelszo));
            return Convert.ToBase64String(inArray);
        }
        static bool contains_special(string value)
        {
            char[] not_allowed = { ' ', '!', '#', '$', '%', '&', '(', ')', '*', '+', ',', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '`', '{', '|', '}', '~' };
            for (int i = 0; i < not_allowed.Length; i++)
            {
                if (value.Contains(not_allowed[i]))
                {
                    return true;
                }
            }
            return false;
        }
        //Email @ és .
        static bool contains_email_spec(string value)
        {
            string email_spec = "@.";
            for (int i = 0; i < email_spec.Length; i++)
            {
                if (value.Contains(email_spec[i]))
                {
                    return false;
                }
            }
            return true;
        }
        //Számot tartalmaz
        static bool contains_number(string value)
        {
            string numbers = "0123456789";
            for (int i = 0; i < numbers.Length; i++)
            {
                if (value.Contains(numbers[i]))
                {
                    return true;
                }
            }
            return false;
        }
        //Számot és nagybetűt tartalmaz
        static bool contains_number_upper(string value)
        {
            string numbers = "0123456789";
            string uppercases = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            bool[] tb = new bool[2];

            for (int i = 0; i < uppercases.Length; i++)
            {
                if (value.Contains(uppercases[i]))
                {
                    tb[1] = true;
                }
                if (i < numbers.Length)
                {
                    if (value.Contains(numbers[i]))
                    {
                        tb[0] = true;
                    }
                }
                if (tb[0] && tb[1])
                {
                    return true;
                }
            }
            return false;
        }
        //Adatbázisban szereplő értékek feltöltése a ComboBox-okba.
        private List<string> szabad()//Ezt át kell írni where-es lekézdezésre(egyszerűbb)
        {
            List<string> nemhasznalt = new List<string>();
            List<string> osszes = db.listat_ad("SELECT personal_number FROM personals");
            List<string> hasznalt = db.listat_ad("SELECT personal_number FROM users");

            bool vane;
            for (int i = 0; i < osszes.Count; i++)
            {
                vane = false;
                for (int j = 0; j < hasznalt.Count; j++)
                {
                    if (osszes[i] == hasznalt[j])
                    {
                        vane = true;
                    }
                }
                if (vane == false)
                {
                    nemhasznalt.Add(osszes[i]);
                }
            }
            return nemhasznalt;
        }
        private void get_values()
        {

            UM_user_name_Cbox.DataSource = db.listat_ad("SELECT user_name FROM users");
            UM_user_name_Cbox.SelectedIndex = -1;
            UM_personal_number_Cbox.DataSource = szabad();
            UM_personal_number_Cbox.SelectedIndex = -1;
            PM_product_name_Cbox.DataSource = db.listat_ad("SELECT product_name FROM products");
            PM_product_name_Cbox.SelectedIndex = -1;
            EM_personal_number_Cbox.DataSource = db.listat_ad("SELECT personal_number FROM personals");
            EM_personal_number_Cbox.SelectedIndex = -1;
        }
        //Személyi szám ellenőrzése
        static bool Szemelyi_e(string szemszam)
        {
            if (szemszam.Length != 8)
                return false;
            for (int i = 0; i < szemszam.Length; i++)
            {
                if (i < 6 && !char.IsNumber(szemszam[i]))
                    return false;
                if (i >= 6 && char.IsNumber(szemszam[i]))
                    return false;
            }
            return true;
        }
        //Mezűk ürítése
        private void clean(string btn)
        {
            if (btn == "UM_upload_Btn" || btn == "UM_update_Btn" || btn == "UM_delete_Btn")
            {
                UM_id_Tbox.Text = "";
                UM_user_name_Cbox.Text = "";
                UM_password_TBox.Text = "";
                UM_personal_number_Cbox.Text = "";
                UM_type_CheckBox.Checked = false;

                UM_user_name_Cbox.DataSource = db.listat_ad("SELECT user_name FROM users");
                UM_user_name_Cbox.SelectedIndex = -1;
                UM_personal_number_Cbox.DataSource = szabad();
                UM_personal_number_Cbox.SelectedIndex = -1;
            }
            else if (btn == "PM_upload_Btn" || btn == "PM_update_Btn" || btn == "PM_delete_Btn")
            {
                PM_id_Tbox.Text = "";
                PM_product_name_Cbox.Text = "";
                PM_bar_code_Tbox.Text = "";
                PM_PBox.Image = null;

                PM_product_name_Cbox.DataSource = db.listat_ad("SELECT product_name FROM products");
                PM_product_name_Cbox.SelectedIndex = -1;
            }
            else if (btn == "EM_upload_Btn" || btn == "EM_update_Btn" || btn == "EM_delete_Btn")
            {
                EM_id_Tbox.Text = "";
                EM_personal_number_Cbox.Text = "";
                EM_name_Tbox.Text = "";
                EM_date_of_birth_Tbox.Text = "";
                EM_address_Tbox.Text = "";
                EM_email_Tbox.Text = "";
                EM_phone_number_Tbox.Text = "";
                EM_PBox.Image = null;

                EM_personal_number_Cbox.DataSource = db.listat_ad("SELECT personal_number FROM personals");
                EM_personal_number_Cbox.SelectedIndex = -1;
                UM_personal_number_Cbox.DataSource = szabad();
                UM_personal_number_Cbox.SelectedIndex = -1;
            }
        }
        #endregion
        #region open_close
        //Admin felület megnyitása
        private void AdminLog_Load(object sender, EventArgs e)
        {
            get_values();
        }
        //Admin felület bezárása
        private void AdminLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        #endregion
        #region restriction_tooltips
        //In progress...
        private void click_with_tips(string btn)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.info;
            popup.TitleText = "Jelentés";

            switch (btn)
            {
                //Megszorítások és ToolTip-ek a felhasználói adatok felviteléhez
                case "UM_upload_Btn":
                case "UM_update_Btn":
                    //empty
                    if (UM_user_name_Cbox.Text == "" || UM_password_TBox.Text == "" || UM_personal_number_Cbox.Text == "")//üres-e
                    {
                        popup.ContentText = "Minden mezőt ki kell töltened.";
                    }
                    //user_name
                    else if (btn == "UM_upload_Btn" && UM_user_name_Cbox.Text == db.erteket_ad(string.Format(@"SELECT user_name FROM users WHERE user_name='{0}'", UM_user_name_Cbox.Text)))//létezik-e
                    {
                        popup.ContentText = "Ez a felhasználónév már foglalt.";
                    }
                    else if (UM_user_name_Cbox.Text.Length < 5 || UM_user_name_Cbox.Text.Length > 20)//megfelelő hoszzú-e
                    {
                        popup.ContentText = "Felhasználónév követelmény\n● Min. 5 karakter\n● Max. 20 karakter";
                    }
                    else if (contains_special(UM_user_name_Cbox.Text) == true)//tiltott karaktert tartalmaz-e
                    {
                        popup.ContentText = "Érvénytelen karakterek\n● Szóköz\n● !#$%&()*+,/:;<=>?@[\\]^`{|}~";
                    }
                    //password
                    else if (UM_password_TBox.Text.Length < 8)
                    {
                        popup.ContentText = "Jelszó követelmény\n● Min. 8 karakter";
                    }
                    else if (contains_special(UM_password_TBox.Text) == true)//tiltott karaktert tartalmaz-e
                    {
                        popup.ContentText = "Érvénytelen karakterek\n● Szóköz\n● !#$%&()*+,/:;<=>?@[\\]^`{|}~";
                    }
                    else if (contains_number_upper(UM_password_TBox.Text) == false)//megkövetelt karaktereket tartalmaz-e 
                    {
                        popup.ContentText = "Érvénytelen karakterlánc\n● Kisbetű\n● Nagybetű\n● Szám";
                    }
                    //user_name and password
                    else if (UM_user_name_Cbox.Text == UM_password_TBox.Text)
                    {
                        popup.ContentText = "A felhasználónév és jelszó páros nem eggyezhet meg.";
                    }
                    //Minden feltételnek megfeleltek az értékek
                    else
                    {
                        if (btn == "UM_upload_Btn")
                        {
                            int admin;
                            string pw = titkositas(UM_password_TBox.Text);
                            if (UM_type_CheckBox.Checked == true)
                            {
                                admin = 1;
                            }
                            else
                            {
                                admin = 0;
                            }

                            db.vegrehajt(string.Format(@"INSERT INTO users(user_name,password,type,personal_number) VALUES ('{0}','{1}','{2}','{3}');", UM_user_name_Cbox.Text, pw, admin, UM_personal_number_Cbox.Text));
                            popup.ContentText = "A felhasználó rögzítése sikeres volt.";
                        }
                        else if (btn == "UM_update_Btn")
                        {
                            string darab = db.erteket_ad(string.Format(@"SELECT COUNT(user_name) FROM users WHERE user_name='{0}'", UM_user_name_Cbox.Text));
                            if (darab == "0")//létezik-e
                            {
                                popup.ContentText = "Ez a felhasználónév még nincs használatban ezért nem módosíthatod.";
                            }
                            else
                            {
                                int admin;
                                string pw = titkositas(UM_password_TBox.Text);
                                if (UM_type_CheckBox.Checked == true)
                                {
                                    admin = 1;
                                }
                                else
                                {
                                    admin = 0;
                                }
                                db.vegrehajt(string.Format(@"UPDATE users SET user_name='{0}',password='{1}',type='{2}',personal_number='{3}' WHERE user_name='{4}';", UM_user_name_Cbox.Text, pw, admin, UM_personal_number_Cbox.Text, UM_user_name_Cbox.Text));
                                popup.ContentText = "A felhasználó módosítása sikeres volt.";
                            }
                        }
                    }
                    popup.Popup();
                    break;
                case "PM_upload_Btn":
                case "PM_update_Btn":
                    //empty
                    if (PM_product_name_Cbox.Text == "" || PM_bar_code_Tbox.Text == "")//üres-e
                    {
                        popup.ContentText = "Minden mezőt ki kell töltened.";
                    }
                    //product_name
                    else if (btn == "PM_upload_Btn" && PM_product_name_Cbox.Text == db.erteket_ad(string.Format(@"SELECT product_name FROM products WHERE product_name='{0}'", PM_product_name_Cbox.Text)))//létezik-e
                    {
                        popup.ContentText = "Ez a termék név már foglalt.";
                    }
                    //bar_code
                    else if (PM_bar_code_Tbox.Text.Length < 13 || PM_bar_code_Tbox.Text.Length > 17)//megfelelő hoszzú-e
                    {
                        popup.ContentText = "Felhasználónév követelmény\n● Min. 13 karakter\n● Max. 17 karakter";
                    }
                    //img
                    else if (PM_PBox.Image == null)
                    {
                        popup.ContentText = "Válassz ki egy illusztrációt.";
                    }
                    else
                    {
                        if (btn == "PM_upload_Btn")
                        {
                            string imgPath = strFilePath + "\\" + strFileName;
                            db.keppel_feltolt(imgPath, string.Format(@"INSERT INTO products(product_name,bar_code,picture) VALUES ('{0}','{1}',", PM_product_name_Cbox.Text, PM_bar_code_Tbox.Text));
                            popup.ContentText = "A termék adatainak rögzítése sikeres!";
                        }
                        else if (btn == "PM_update_Btn")
                        {
                            if (strFileName != "" && strFilePath != "")
                            {
                                string imgPath = strFilePath + "\\" + strFileName;
                                db.keppel_frissit("product_name", PM_product_name_Cbox.Text, imgPath, string.Format(@"UPDATE products SET product_name='{0}',bar_code='{1}',picture=", PM_product_name_Cbox.Text, PM_bar_code_Tbox.Text));
                            }
                            else
                            {
                                db.vegrehajt(string.Format(@"UPDATE products SET product_name='{0}',bar_code='{1}' WHERE product_name='{2}';", PM_product_name_Cbox.Text, PM_bar_code_Tbox.Text, PM_product_name_Cbox));
                            }
                            popup.ContentText = "Az termék adatainak módosítása sikeres volt!";
                        }
                    }
                    popup.Popup();
                    break;
                case "EM_upload_Btn":
                case "EM_update_Btn":
                    if (!Szemelyi_e(EM_personal_number_Cbox.Text))
                    {
                        popup.ContentText = "A bevitt érték nem személyi szám!";
                    }
                    else if (contains_number(EM_name_Tbox.Text))
                    {
                        popup.ContentText = "A név mező nem tartalmazhat számot!";
                    }
                    else if (contains_special(EM_name_Tbox.Text))//tiltott karaktert tartalmaz-e
                    {
                        popup.ContentText = "Érvénytelen karakterek\n● Szóköz\n● !#$%&()*+,/:;<=>?@[\\]^`{|}~";
                    }
                    else if (contains_email_spec(EM_email_Tbox.Text))
                    {
                        popup.ContentText = "Az e-mail címnek tartalmaznia kell @ és . karaktereket is!";
                    }
                    else if (EM_PBox.Image == null)
                    {
                        popup.ContentText = "Válassz ki egy illusztrációt.";
                    }
                    else
                    {
                        if (btn == "EM_upload_Btn")
                        {
                            string imgPath2 = strFilePath + "\\" + strFileName;
                            db.keppel_feltolt(imgPath2, string.Format(@"INSERT INTO personals(personal_number,name,date_of_birth,address,email,phone_number,picture) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}',", EM_personal_number_Cbox.Text, EM_name_Tbox.Text, EM_date_of_birth_Tbox.Text, EM_address_Tbox.Text, EM_email_Tbox.Text, EM_phone_number_Tbox.Text));
                            popup.ContentText = "Az adatok rögzítése sikeres!";
                        }
                        else if (btn == "EM_update_Btn")
                        {
                            if (strFileName != "" && strFilePath != "")
                            {
                                string imgPath = strFilePath + "\\" + strFileName;
                                db.keppel_frissit("personal_number", EM_personal_number_Cbox.Text, imgPath, string.Format(@"UPDATE personals SET personal_number='{0}',name='{1}',date_of_birth='{2}',address='{3}',email='{4}',phone_number='{5}',picture=", EM_personal_number_Cbox.Text, EM_name_Tbox.Text, EM_date_of_birth_Tbox.Text, EM_address_Tbox.Text, EM_email_Tbox.Text, EM_phone_number_Tbox.Text));
                                popup.ContentText = "Az alkalmazott adatainak módosítása sikeres volt!";
                            }
                            else
                            {
                                db.vegrehajt(string.Format(@"UPDATE personals SET personal_number='{0}',name='{1}',date_of_birth='{2}',address='{3}',email='{4}',phone_number='{5}' WHERE personal_number='{6}'", EM_personal_number_Cbox.Text, EM_name_Tbox.Text, EM_date_of_birth_Tbox.Text, EM_address_Tbox.Text, EM_email_Tbox.Text, EM_phone_number_Tbox.Text, EM_personal_number_Cbox.Text));
                                popup.ContentText = "Az alkalmazott adatainak módosítása sikeres volt!";
                            }
                        }
                    }
                    popup.Popup();
                    break;
                default:
                    popup.Popup();
                    break;
            }
        }
        #endregion
        #region user_managment
        //Felhasználó adatok feltöltése az adatbázisba
        private void UM_upload_Btn_Click(object sender, EventArgs e)
        {
            click_with_tips(UM_upload_Btn.Name);
            clean(UM_upload_Btn.Name);
        }
        //Felhasználó adatainak frissítése az adatbázisban
        private void UM_update_Btn_Click(object sender, EventArgs e)
        {
            click_with_tips(UM_update_Btn.Name);
            clean(UM_update_Btn.Name);
        }
        //Felhasználó törlése az adatbázisból
        private void UM_delete_Btn_Click(object sender, EventArgs e)
        {
            db.vegrehajt(string.Format(@"DELETE FROM users WHERE id='{0}';", UM_id_Tbox.Text));
            MessageBox.Show("A felhasználó törlése sikeres!");
            clean(UM_delete_Btn.Name);
        }
        //Felhasználó adatainak kitöltése a felhasználó kezelő felületen
        private void UM_user_name_Cbox_Leave(object sender, EventArgs e)
        {
            string vane = db.erteket_ad(string.Format(@"SELECT COUNT(id) FROM users WHERE user_name = '{0}';", UM_user_name_Cbox.Text));
            if (Convert.ToInt32(vane) == 1)
            {
                string[] adatok = db.erteket_ad_rekord(string.Format(@"SELECT id,personal_number,type FROM users WHERE user_name='{0}'", UM_user_name_Cbox.Text));
                UM_id_Tbox.Text = adatok[0];
                UM_personal_number_Cbox.Text = adatok[1];
                if (adatok[2] == "True")
                {
                    UM_type_CheckBox.Checked = true;
                }
                else
                {
                    UM_type_CheckBox.Checked = false;
                }
            }
            else
            {
                UM_id_Tbox.Text = "";
                UM_password_TBox.Text = "";
                UM_personal_number_Cbox.Text = "";
                UM_type_CheckBox.Checked = false;
            }
        }
        #endregion
        #region product_managment
        //Termék adatok feltöltése az adatbázisba
        private void PM_upload_Btn_Click(object sender, EventArgs e)
        {
            click_with_tips(PM_upload_Btn.Name);
            clean(PM_upload_Btn.Name);
        }
        //Termék adatainak frissítése az adatbázisban
        private void PM_update_Btn_Click(object sender, EventArgs e)
        {
            click_with_tips(PM_update_Btn.Name);
            clean(PM_update_Btn.Name);
        }
        //Termék törlése az adatbázisból
        private void PM_delete_Btn_Click(object sender, EventArgs e)
        {
            db.vegrehajt(string.Format(@"DELETE FROM products WHERE id='{0}';", PM_id_Tbox.Text));
            MessageBox.Show("A termék törlése sikeres!");
            clean(PM_delete_Btn.Name);
        }
        //Termék adatainak kitöltése a termék kezelő felületen
        private void PM_product_name_Cbox_Leave(object sender, EventArgs e)
        {

            string vane = db.erteket_ad(string.Format(@"SELECT COUNT(id) FROM products WHERE product_name = '{0}';", PM_product_name_Cbox.Text));
            if (Convert.ToInt32(vane) == 1)
            {
                string[] adatok = db.erteket_ad_rekord(string.Format(@"SELECT id,bar_code FROM products WHERE product_name='{0}'", PM_product_name_Cbox.Text));
                PM_id_Tbox.Text = adatok[0];
                PM_bar_code_Tbox.Text = adatok[1];
                PM_PBox.Image = db.kepet_ad("SELECT picture FROM products WHERE product_name = '" + PM_product_name_Cbox.Text + "'");
            }
            else
            {
                PM_id_Tbox.Text = "";
                PM_bar_code_Tbox.Text = "";
                PM_PBox.Image = null;
            }
        }
        //Termékhez tartozó kép kiválasztása,elhelyezése a PictureBox-ba
        private void PM_picture_pick_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Kiterjesztés(*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PM_PBox.Image = Image.FromFile(dialog.FileName);
                }
                FileInfo fInfo = new FileInfo(dialog.FileName);
                strFileName = fInfo.Name;
                strFilePath = fInfo.DirectoryName;
            }
            catch (Exception)
            {
                MessageBox.Show("Nem sikerült a képet betölteni", "Hiba", MessageBoxButtons.OK);
            }
        }
        #endregion
        #region employee_managment
        ////Személyes adatok feltöltése az adatbázisba
        private void EM_upload_Btn_Click(object sender, EventArgs e)
        {
            click_with_tips(EM_upload_Btn.Name);
            clean(EM_upload_Btn.Name);
        }
        //Személy adatainak frissítése az adatbázisban
        private void EM_update_Btn_Click(object sender, EventArgs e)
        {
            click_with_tips(EM_update_Btn.Name);
            clean(EM_update_Btn.Name);
        }
        //Személy törlése az adatbázisból
        private void EM_delete_Btn_Click(object sender, EventArgs e)
        {
            db.vegrehajt(string.Format(@"DELETE FROM personals WHERE id='{0}';", EM_id_Tbox.Text));
            MessageBox.Show("A felhasználó törlése sikeress!");
            clean(EM_delete_Btn.Name);
        }
        //Személy adatainak kitöltése a személy kezelő felületen
        private void EM_personal_number_Cbox_Leave(object sender, EventArgs e)
        {
            string vane = db.erteket_ad(string.Format(@"SELECT COUNT(id) FROM personals WHERE personal_number = '{0}';", EM_personal_number_Cbox.Text));
            if (Convert.ToInt32(vane) == 1)
            {
                string[] adatok = db.erteket_ad_rekord(string.Format(@"SELECT id,name,date_of_birth,address,email,phone_number FROM personals WHERE personal_number='{0}'", EM_personal_number_Cbox.Text));
                EM_id_Tbox.Text = adatok[0];
                EM_name_Tbox.Text = adatok[1];
                string[] sv = adatok[2].Split(' ');
                MessageBox.Show(adatok[2]);
                EM_date_of_birth_Tbox.Text = sv[0];
                EM_address_Tbox.Text = adatok[3];
                EM_email_Tbox.Text = adatok[4];
                EM_phone_number_Tbox.Text = adatok[5];
                EM_PBox.Image = db.kepet_ad("SELECT picture FROM personals WHERE personal_number = '" + EM_personal_number_Cbox.Text + "'");
            }
            else
            {
                EM_id_Tbox.Text = "";
                EM_name_Tbox.Text = "";
                EM_date_of_birth_Tbox.Text = "";
                EM_address_Tbox.Text = "";
                EM_email_Tbox.Text = "";
                EM_phone_number_Tbox.Text = "";
                EM_PBox.Image = null;
            }
        }
        //Személyhez tartozó kép kiválasztása,elhelyezése a PictureBox-ba
        private void EM_picture_pick_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Kiterjesztés(*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    EM_PBox.Image = Image.FromFile(dialog.FileName);
                }
                FileInfo fInfo = new FileInfo(dialog.FileName);
                strFileName = fInfo.Name;
                strFilePath = fInfo.DirectoryName;
            }
            catch (Exception)
            {
                MessageBox.Show("Nem sikerült a képet betölteni", "Hiba", MessageBoxButtons.OK);
            }
        }
        //Dátum kiválasztását segítő felület megnyitása és az adat átvétele
        private void EM_date_of_birth_Btn_Click(object sender, EventArgs e)
        {
            calendar newc = new calendar();
            newc.ShowDialog();
            EM_date_of_birth_Tbox.Text = newc.birth_date;
        }
        #endregion
    }
}