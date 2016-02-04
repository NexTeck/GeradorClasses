namespace Gerador_Classes_BD
{
    partial class JanelaPrincipal
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtDiretorioBD = new System.Windows.Forms.TextBox();
            this.txtDiretorioGerado = new System.Windows.Forms.TextBox();
            this.btnGerar = new System.Windows.Forms.Button();
            this.btnBuscarDirBD = new System.Windows.Forms.Button();
            this.btnBuscarDirGerar = new System.Windows.Forms.Button();
            this.btnDesktop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Diretorio do Banco de Dados";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Diretorio gerado";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblStatus.Location = new System.Drawing.Point(12, 102);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(203, 67);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Esse programa foi criado para gerar classes das tabelas de um Banco de Dados Sql " +
    "Server.\r\n\r\nAndrei 29/10/2015";
            // 
            // txtDiretorioBD
            // 
            this.txtDiretorioBD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiretorioBD.Location = new System.Drawing.Point(15, 25);
            this.txtDiretorioBD.Name = "txtDiretorioBD";
            this.txtDiretorioBD.Size = new System.Drawing.Size(221, 20);
            this.txtDiretorioBD.TabIndex = 1;
            // 
            // txtDiretorioGerado
            // 
            this.txtDiretorioGerado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiretorioGerado.Location = new System.Drawing.Point(15, 69);
            this.txtDiretorioGerado.Name = "txtDiretorioGerado";
            this.txtDiretorioGerado.Size = new System.Drawing.Size(221, 20);
            this.txtDiretorioGerado.TabIndex = 1;
            // 
            // btnGerar
            // 
            this.btnGerar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGerar.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnGerar.Location = new System.Drawing.Point(221, 146);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(75, 23);
            this.btnGerar.TabIndex = 2;
            this.btnGerar.Text = "Gerar Objetos";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click);
            // 
            // btnBuscarDirBD
            // 
            this.btnBuscarDirBD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscarDirBD.Location = new System.Drawing.Point(242, 25);
            this.btnBuscarDirBD.Name = "btnBuscarDirBD";
            this.btnBuscarDirBD.Size = new System.Drawing.Size(54, 23);
            this.btnBuscarDirBD.TabIndex = 2;
            this.btnBuscarDirBD.Text = "Buscar";
            this.btnBuscarDirBD.UseVisualStyleBackColor = true;
            this.btnBuscarDirBD.Click += new System.EventHandler(this.btnBuscarDirBD_Click);
            // 
            // btnBuscarDirGerar
            // 
            this.btnBuscarDirGerar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscarDirGerar.Location = new System.Drawing.Point(242, 69);
            this.btnBuscarDirGerar.Name = "btnBuscarDirGerar";
            this.btnBuscarDirGerar.Size = new System.Drawing.Size(54, 22);
            this.btnBuscarDirGerar.TabIndex = 2;
            this.btnBuscarDirGerar.Text = "Buscar";
            this.btnBuscarDirGerar.UseVisualStyleBackColor = true;
            this.btnBuscarDirGerar.Click += new System.EventHandler(this.btnBuscarDirGerar_Click);
            // 
            // btnDesktop
            // 
            this.btnDesktop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDesktop.Location = new System.Drawing.Point(221, 97);
            this.btnDesktop.Name = "btnDesktop";
            this.btnDesktop.Size = new System.Drawing.Size(75, 23);
            this.btnDesktop.TabIndex = 3;
            this.btnDesktop.Text = "Desktop";
            this.btnDesktop.UseVisualStyleBackColor = true;
            this.btnDesktop.Click += new System.EventHandler(this.btnDesktop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(308, 181);
            this.Controls.Add(this.btnDesktop);
            this.Controls.Add(this.btnBuscarDirGerar);
            this.Controls.Add(this.btnBuscarDirBD);
            this.Controls.Add(this.btnGerar);
            this.Controls.Add(this.txtDiretorioGerado);
            this.Controls.Add(this.txtDiretorioBD);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(324, 220);
            this.Name = "Form1";
            this.Text = "Gerador Classes BD";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtDiretorioBD;
        private System.Windows.Forms.TextBox txtDiretorioGerado;
        private System.Windows.Forms.Button btnGerar;
        private System.Windows.Forms.Button btnBuscarDirBD;
        private System.Windows.Forms.Button btnBuscarDirGerar;
        private System.Windows.Forms.Button btnDesktop;
    }
}

