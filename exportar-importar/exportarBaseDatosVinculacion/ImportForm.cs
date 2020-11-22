using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace exportarBaseDatosVinculacion
{
    public partial class ImportForm : Form
    {
        public ImportForm()
        {
            InitializeComponent();
        }

        protected void cboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        DataTableCollection tableCollection;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Files| *.xls; *.xlsx" })
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilename.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            tableCollection = result.Tables;
                            cboSheet.Items.Clear();
                            foreach (DataTable table in tableCollection)
                            {
                                cboSheet.Items.Add(table.TableName);//agregar hoja al combobox
                            }
                        }
                    }
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string caption = "Salir";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            result = MessageBox.Show("Esta seguro que desea salir?", caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
                MainForm mf = new MainForm();
                mf.Show();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=HoursTracker2;Trusted_Connection=True;");
                cnn.Open();
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la conexion a la base de datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string selectedSheet = cboSheet.GetItemText(cboSheet.SelectedItem);
                string filePath = txtFilename.Text;
                string conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";

                OleDbConnection origen = default(OleDbConnection);
                origen = new OleDbConnection(conexion);

                OleDbCommand seleccion = default(OleDbCommand);
                seleccion = new OleDbCommand("Select * from [" + selectedSheet + "$]", origen);

                OleDbDataAdapter adaptador = new OleDbDataAdapter();
                adaptador.SelectCommand = seleccion;

                DataSet ds = new DataSet();
                adaptador.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

                origen.Close();

                SqlConnection conexion_destino = new SqlConnection();
                conexion_destino.ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=HoursTrackerDemo;Trusted_Connection=True;";
                string delQuery = "Delete From " + selectedSheet;
                string identReset = "DBCC CHECKIDENT('" + selectedSheet + "', RESEED, 0)";
                conexion_destino.Open();

                SqlCommand cmd = new SqlCommand(delQuery, conexion_destino);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(identReset, conexion_destino);
                cmd.ExecuteNonQuery();


                SqlBulkCopy importar = default(SqlBulkCopy);
                importar = new SqlBulkCopy(conexion_destino);
                importar.DestinationTableName = selectedSheet;
                importar.WriteToServer(ds.Tables[0]);
                conexion_destino.Close();

                MessageBox.Show("Tabla Importada Exitosamente!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm fm = new MainForm();
            fm.Show();
        }
    }
}
