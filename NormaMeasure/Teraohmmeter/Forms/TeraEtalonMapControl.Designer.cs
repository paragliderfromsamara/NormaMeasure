namespace NormaMeasure.Teraohmmeter.Forms
{
    partial class TeraEtalonMapControl
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
            this.createOrSaveButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.buttonsMenu = new System.Windows.Forms.Panel();
            this.buttonsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // createOrSaveButton
            // 
            this.createOrSaveButton.Location = new System.Drawing.Point(0, 3);
            this.createOrSaveButton.Name = "createOrSaveButton";
            this.createOrSaveButton.Size = new System.Drawing.Size(88, 23);
            this.createOrSaveButton.TabIndex = 18;
            this.createOrSaveButton.Text = "Добавить";
            this.createOrSaveButton.UseVisualStyleBackColor = true;
            this.createOrSaveButton.Click += new System.EventHandler(this.createOrSaveButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 14);
            this.label10.TabIndex = 19;
            this.label10.Text = "Название";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(21, 37);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(185, 22);
            this.textBoxName.TabIndex = 20;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(97, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(88, 23);
            this.deleteButton.TabIndex = 21;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // buttonsMenu
            // 
            this.buttonsMenu.Controls.Add(this.deleteButton);
            this.buttonsMenu.Controls.Add(this.createOrSaveButton);
            this.buttonsMenu.Location = new System.Drawing.Point(21, 85);
            this.buttonsMenu.Name = "buttonsMenu";
            this.buttonsMenu.Size = new System.Drawing.Size(185, 52);
            this.buttonsMenu.TabIndex = 22;
            // 
            // TeraEtalonMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 140);
            this.Controls.Add(this.buttonsMenu);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label10);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeraEtalonMapControl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Карта эталонов тераомметра";
            this.buttonsMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button createOrSaveButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Panel buttonsMenu;
    }
}