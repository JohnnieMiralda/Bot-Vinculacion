namespace exportarBaseDatosVinculacion
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.exportButton = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelNotClose = new System.Windows.Forms.Label();
            this.labelAmountTables = new System.Windows.Forms.Label();
            this.labelNameTable = new System.Windows.Forms.Label();
            this.importButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.closeButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.SystemColors.Control;
            this.exportButton.ForeColor = System.Drawing.Color.Black;
            this.exportButton.Location = new System.Drawing.Point(46, 147);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(118, 40);
            this.exportButton.TabIndex = 0;
            this.exportButton.Text = "Exportar base datos";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(26, 36);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(116, 13);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Seleccione una opcion";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelNotClose);
            this.panel1.Controls.Add(this.labelAmountTables);
            this.panel1.Controls.Add(this.labelNameTable);
            this.panel1.Controls.Add(this.importButton);
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.closeButton);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.exportButton);
            this.panel1.Location = new System.Drawing.Point(134, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 247);
            this.panel1.TabIndex = 2;
            // 
            // labelNotClose
            // 
            this.labelNotClose.AutoSize = true;
            this.labelNotClose.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelNotClose.Location = new System.Drawing.Point(49, 11);
            this.labelNotClose.Name = "labelNotClose";
            this.labelNotClose.Size = new System.Drawing.Size(246, 13);
            this.labelNotClose.TabIndex = 12;
            this.labelNotClose.Text = "Por favor no cerrar aplicacion durante exportacion.";
            this.labelNotClose.Visible = false;
            // 
            // labelAmountTables
            // 
            this.labelAmountTables.AutoSize = true;
            this.labelAmountTables.Location = new System.Drawing.Point(43, 104);
            this.labelAmountTables.Name = "labelAmountTables";
            this.labelAmountTables.Size = new System.Drawing.Size(35, 13);
            this.labelAmountTables.TabIndex = 11;
            this.labelAmountTables.Text = "label1";
            this.labelAmountTables.Visible = false;
            // 
            // labelNameTable
            // 
            this.labelNameTable.AutoSize = true;
            this.labelNameTable.Location = new System.Drawing.Point(161, 36);
            this.labelNameTable.Name = "labelNameTable";
            this.labelNameTable.Size = new System.Drawing.Size(13, 13);
            this.labelNameTable.TabIndex = 10;
            this.labelNameTable.Text = "  ";
            // 
            // importButton
            // 
            this.importButton.BackColor = System.Drawing.SystemColors.Control;
            this.importButton.ForeColor = System.Drawing.Color.Black;
            this.importButton.Location = new System.Drawing.Point(177, 147);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(118, 40);
            this.importButton.TabIndex = 9;
            this.importButton.Text = "Importar base datos";
            this.importButton.UseVisualStyleBackColor = false;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(46, 64);
            this.progressBar.Maximum = 10;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(249, 37);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 8;
            this.progressBar.Value = 1;
            this.progressBar.Visible = false;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.SystemColors.Control;
            this.closeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.closeButton.Location = new System.Drawing.Point(202, 202);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(118, 32);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Salir";
            this.closeButton.UseMnemonic = false;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(615, 342);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exportar Base de Datos ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Label labelNameTable;
        private System.Windows.Forms.Label labelAmountTables;
        private System.Windows.Forms.Label labelNotClose;
    }
}