using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace szakdolgozat_v._0._1
{
    public partial class calendar : Form
    {
        public calendar()
        {
            InitializeComponent();
        }
        public string birth_date = "";
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            birth_date = monthCalendar1.SelectionStart.Date.ToString("yyyy-MM-dd");
            this.Close();
        }

        private void calendar_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
