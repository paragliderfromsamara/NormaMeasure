namespace NormaMeasure.Teraohmmeter.Forms
{
    partial class TeraCoeffsForm
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
            this.SuspendLayout();
            // 
            // TeraCoeffsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 325);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TeraCoeffsForm";
            this.Text = "VoltageCoeffsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TeraCoeffsForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.TeraCoeffsForm_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}