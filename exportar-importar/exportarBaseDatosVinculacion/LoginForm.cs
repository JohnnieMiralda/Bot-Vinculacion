using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exportarBaseDatosVinculacion
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            panelLogin.BackColor = Color.FromArgb(200, Color.White);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            MainForm fm = new MainForm();
            fm.Show();
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string caption = "Salir";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            result =  MessageBox.Show("Esta seguro que desea salir?", caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
