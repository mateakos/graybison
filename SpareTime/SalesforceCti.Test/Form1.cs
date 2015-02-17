using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Spare.Jsonp;

namespace Spare.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Spare.Jsonp.WebServiceManager wsMgr = new WebServiceManager(urlTxt.Text, Convert.ToInt32(portTxt.Text));
            wsMgr.Start();
        }
    }
}
