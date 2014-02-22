using System.Drawing;

namespace SRC_GUI
{
    partial class Measure
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
            this.lbTime = new System.Windows.Forms.Label();
            this.listBoxStatus = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbTime
            // 
            this.lbTime.BackColor = System.Drawing.Color.Black;
            this.lbTime.Font = new System.Drawing.Font("Times New Roman", 140.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.ForeColor = System.Drawing.Color.White;
            this.lbTime.Location = new System.Drawing.Point(12, 9);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(383, 172);
            this.lbTime.TabIndex = 31;
            this.lbTime.Text = "0";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.FormattingEnabled = true;
            this.listBoxStatus.ItemHeight = 12;
            this.listBoxStatus.Location = new System.Drawing.Point(12, 184);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(383, 172);
            this.listBoxStatus.TabIndex = 32;
            // 
            // Measure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 369);
            this.Controls.Add(this.listBoxStatus);
            this.Controls.Add(this.lbTime);
            this.Name = "Measure";
            this.Text = "Form1";
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.ListBox listBoxStatus;
    }
}