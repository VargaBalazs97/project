using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;


namespace szakdolgozat_v._0._1
{
    class database
    {
        string kapcs_string = @"datasource=localhost;database=dbp_p14_vbm;username=root;password=;charset=utf8";
        public MySqlConnection kapcs_mysql;
        MySqlDataAdapter adapter_mysql = new MySqlDataAdapter();

        public bool megnyitas()
        {
            try { kapcs_mysql = new MySqlConnection(kapcs_string); kapcs_mysql.Open(); return true; }
            catch { return false; }
        }
        public bool bezaras()
        {
            try { kapcs_mysql.Close(); return true; }
            catch { return false; }
        }

        public bool vegrehajt(string LKS)
        {
            bezaras();
            if (megnyitas())
            {
                MySqlCommand lekerdezes = new MySqlCommand(LKS, kapcs_mysql);
                lekerdezes.ExecuteNonQuery();
                return true;
            }
            return false;
        }
        public string erteket_ad(string LKS)
        {
            bezaras();
            if (megnyitas())
            {
                MySqlCommand lekerdezes = new MySqlCommand(LKS, kapcs_mysql);
                try
                {
                    return lekerdezes.ExecuteScalar().ToString();
                }
                catch
                {
                    return "hiba1";
                }
            }
            return "hiba";
        }
        public string[] erteket_ad_rekord(string LKS)
        {
            bezaras();
            if (megnyitas())
            {
                MySqlCommand lekerdezes = new MySqlCommand(LKS, kapcs_mysql);
                try
                {
                    MySqlDataReader olvaso;
                    olvaso = lekerdezes.ExecuteReader();
                    int n = olvaso.FieldCount;
                    if (n > 0)
                    {
                        string[] sv = new string[n];
                        olvaso.Read();
                        for (int i = 0; i < n; i++)
                        {
                            sv[i] = olvaso[i].ToString();
                        }
                        return sv;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        public DataSet tablazatot_ad(string LKS)
        {
            bezaras();
            DataSet adatok = new DataSet();
            if (megnyitas())
            {
                adapter_mysql.SelectCommand = new MySqlCommand(LKS, kapcs_mysql);
                adapter_mysql.Fill(adatok);
            }
            return adatok;
        }
        
        public void keppel_feltolt(string imgPath, string LKS)
        {
            bezaras();
            if (megnyitas())
            {
                FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                BufferedStream bf = new BufferedStream(fs);
                byte[] buffer = new byte[bf.Length];
                bf.Read(buffer, 0, buffer.Length);
                byte[] buffer_new = buffer;

                MySqlCommand command = new MySqlCommand("", kapcs_mysql);
                LKS += " @image)";
                command.CommandText = LKS;
                command.Parameters.AddWithValue("@image", buffer_new);
                command.ExecuteNonQuery();
            }
        }

        public void keppel_frissit(string hol, string mi, string imgPath, string LKS )
        {
            bezaras();
            if (megnyitas())
            {
                FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                BufferedStream bf = new BufferedStream(fs);
                byte[] buffer = new byte[bf.Length];
                bf.Read(buffer, 0, buffer.Length);
                byte[] buffer_new = buffer;

                MySqlCommand command = new MySqlCommand("", kapcs_mysql);
                LKS += "@image WHERE "+ hol+"='"+mi+"'";
                command.CommandText = LKS;
                command.Parameters.AddWithValue("@image", buffer_new);
                command.ExecuteNonQuery();
            }
        }
        public Image kepet_ad(string LKS)
        {
            bezaras();
            DataSet adatok = new DataSet();
            Image image = null;
            if (megnyitas())
            {
                adapter_mysql.SelectCommand = new MySqlCommand(LKS, kapcs_mysql);
                adapter_mysql.Fill(adatok,"image");

                Byte[] imageBytes = (Byte[])(adatok.Tables["image"].Rows[0]["picture"]);
                MemoryStream buf = new MemoryStream(imageBytes);                   
                image = Image.FromStream(buf);
            }
            return image;
        }

        public List<string> listat_ad(string LKS)
        {
            bezaras();
            List<string> ertekek = new List<string>();
            if (megnyitas())
            {
                MySqlCommand lekerdezes = new MySqlCommand(LKS, kapcs_mysql);
                MySqlDataReader olvaso = lekerdezes.ExecuteReader();
                while (olvaso.Read())
                {
                    ertekek.Add(olvaso[0].ToString());
                }
            }
            return ertekek;
        }
    }
}