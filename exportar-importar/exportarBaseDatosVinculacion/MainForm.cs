using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using _Excel = Microsoft.Office.Interop.Excel;


namespace exportarBaseDatosVinculacion
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void DelegateMethod(int num)
        {
            progressBar.Value = num;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=HoursTracker2;Trusted_Connection=True;");
                cnn.Open();
                cnn.Close();
            }catch (Exception ex)
            {
                MessageBox.Show("Error en la conexion a la base de datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DialogResult result;
                string caption = "ADVERTENCIA";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                result = MessageBox.Show("TIEMPO DE ESPERA: 15 MIN \n. Desea continuar?", caption, buttons);
                if (result != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                //updating form values
                exportButton.Enabled = false;
                importButton.Enabled = false;
                labelNotClose.Visible = true;
                closeButton.Enabled = false;
                progressBar.Visible = true;

                //starting exportation
                _Excel.Application xlApp;
                _Excel.Workbook xlWorkBook = null;
                _Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (_Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = "Fecha Exportacion";
                DateTime now = DateTime.Now;
                xlWorkSheet.Cells[1, 1] = now.ToString("dddd, dd MMMM yyyy HH:mm:ss");

                SqlConnection cnn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=HoursTracker2;Trusted_Connection=True;");
                cnn.Open();

                //CONTANDO TABLAS
                System.Data.DataTable schema = cnn.GetSchema("Tables");
                int tableCount = schema.AsEnumerable().Count(t => t.Field<string>("TABLE_TYPE") == "BASE TABLE");
                labelAmountTables.Text = "Tablas: 1" + "/" + tableCount;
                labelAmountTables.Visible = true;

                //GETTING TABLE NAMES
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }

                string sql = null;
                System.Data.DataTable sqlTable = null;
                SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
                DataSet ds = new DataSet();
                _Excel.Sheets worksheets = null;
                _Excel.Worksheet xlNewSheet = null;
                labelTitle.Text = "EXPORTANDO TABLA: ";

                for (int l = 0; l < tableCount; l++)
                {
                    progressBar.Value = 0;
                    labelNameTable.Text = TableNames[l];
                    labelAmountTables.Text = "Tablas: " + l + "/" + tableCount;
                    sql = "SELECT * FROM " + TableNames[l];
                    dscmd = null;
                    ds = null;
                    sqlTable = null;

                    worksheets = xlWorkBook.Worksheets;
                    xlNewSheet = (_Excel.Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
                    xlNewSheet.Name = TableNames[l];

                    xlNewSheet = (_Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    dscmd = new SqlDataAdapter(sql, cnn);
                    ds = new DataSet();
                    dscmd.Fill(ds);
                    sqlTable = ds.Tables[0];
                    progressBar.Maximum = sqlTable.Rows.Count;
                    exportOperation(sqlTable, xlNewSheet);
                }

                //AL TERMINAR EXPORTACIONES
                labelAmountTables.Text = "Tablas: " + tableCount + "/" + tableCount;
                progressBar.Value = progressBar.Maximum;
                labelTitle.Text = "EXPORTACION";
                labelNameTable.Text = "TERMINADA.";
                importButton.Enabled = true;
                exportButton.Enabled = true;
                labelNotClose.Visible = false;
                closeButton.Enabled = true;
                cnn.Close();
                xlApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exportOperation(System.Data.DataTable sqlTable, Worksheet xlNewSheet)
        {
                for (int i = 1; i < sqlTable.Columns.Count + 1; i++)
                {
                    xlNewSheet.Cells[1, i] = sqlTable.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < sqlTable.Rows.Count; j++)
                {

                    for (int k = 0; k < sqlTable.Columns.Count; k++)
                    {
                        xlNewSheet.Cells[j + 2, k + 1] = sqlTable.Rows[j].ItemArray[k].ToString();
                    }
                    progressBar.Value = j;
                }            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string caption = "Cerrar sesion";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            result = MessageBox.Show("Esta seguro que desea salir?", caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Close();
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            Hide();
            ImportForm im = new ImportForm();
            im.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
        }
    }
}
