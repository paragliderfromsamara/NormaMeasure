namespace NormaMeasure.DevicesForms
{
    partial class coeffsSynchronyzeStatus
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
            this.serialNumberLbl = new System.Windows.Forms.Label();
            this.setLinkLbl = new System.Windows.Forms.Label();
            this.loadParamsLbl = new System.Windows.Forms.Label();
            this.saveParamsLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serialNumberLbl
            // 
            this.serialNumberLbl.AutoSize = true;
            this.serialNumberLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.serialNumberLbl.Location = new System.Drawing.Point(40, 26);
            this.serialNumberLbl.Name = "serialNumberLbl";
            this.serialNumberLbl.Size = new System.Drawing.Size(247, 24);
            this.serialNumberLbl.TabIndex = 0;
            this.serialNumberLbl.Text = "Серийный номер 20YY-NN";
            // 
            // setLinkLbl
            // 
            this.setLinkLbl.AutoSize = true;
            this.setLinkLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.setLinkLbl.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.setLinkLbl.Location = new System.Drawing.Point(51, 66);
            this.setLinkLbl.Name = "setLinkLbl";
            this.setLinkLbl.Size = new System.Drawing.Size(136, 20);
            this.setLinkLbl.TabIndex = 1;
            this.setLinkLbl.Text = "Установка связи";
            // 
            // loadParamsLbl
            // 
            this.loadParamsLbl.AutoSize = true;
            this.loadParamsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.loadParamsLbl.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.loadParamsLbl.Location = new System.Drawing.Point(51, 96);
            this.loadParamsLbl.Name = "loadParamsLbl";
            this.loadParamsLbl.Size = new System.Drawing.Size(173, 20);
            this.loadParamsLbl.TabIndex = 2;
            this.loadParamsLbl.Text = "Загрузка параметров";
            // 
            // saveParamsLbl
            // 
            this.saveParamsLbl.AutoSize = true;
            this.saveParamsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.saveParamsLbl.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.saveParamsLbl.Location = new System.Drawing.Point(51, 126);
            this.saveParamsLbl.Name = "saveParamsLbl";
            this.saveParamsLbl.Size = new System.Drawing.Size(235, 20);
            this.saveParamsLbl.TabIndex = 3;
            this.saveParamsLbl.Text = "Сохранение параметров в БД";
            // 
            // coeffsSynchronyzeStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 186);
            this.ControlBox = false;
            this.Controls.Add(this.saveParamsLbl);
            this.Controls.Add(this.loadParamsLbl);
            this.Controls.Add(this.setLinkLbl);
            this.Controls.Add(this.serialNumberLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "coeffsSynchronyzeStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Синхронизация параметров";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serialNumberLbl;
        private System.Windows.Forms.Label setLinkLbl;
        private System.Windows.Forms.Label loadParamsLbl;
        private System.Windows.Forms.Label saveParamsLbl;
    }
}