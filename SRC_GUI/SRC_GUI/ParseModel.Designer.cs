namespace ParseModelProject
{
    partial class ParseModel
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
            this.listBoxStatus = new System.Windows.Forms.ListBox();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textWorkingDir = new System.Windows.Forms.TextBox();
            this.textAppPower = new System.Windows.Forms.TextBox();
            this.textAsyncPower = new System.Windows.Forms.TextBox();
            this.textTotalPower = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.FormattingEnabled = true;
            this.listBoxStatus.ItemHeight = 12;
            this.listBoxStatus.Location = new System.Drawing.Point(12, 131);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(412, 292);
            this.listBoxStatus.TabIndex = 0;
            // 
            // numCount
            // 
            this.numCount.Location = new System.Drawing.Point(349, 68);
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(75, 22);
            this.numCount.TabIndex = 1;
            this.numCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(349, 94);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Working Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Application Power";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Asyncronous Power";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Total Power";
            // 
            // textWorkingDir
            // 
            this.textWorkingDir.Location = new System.Drawing.Point(121, 12);
            this.textWorkingDir.Name = "textWorkingDir";
            this.textWorkingDir.Size = new System.Drawing.Size(303, 22);
            this.textWorkingDir.TabIndex = 7;
            // 
            // textAppPower
            // 
            this.textAppPower.Location = new System.Drawing.Point(121, 40);
            this.textAppPower.Name = "textAppPower";
            this.textAppPower.Size = new System.Drawing.Size(176, 22);
            this.textAppPower.TabIndex = 8;
            // 
            // textAsyncPower
            // 
            this.textAsyncPower.Location = new System.Drawing.Point(121, 68);
            this.textAsyncPower.Name = "textAsyncPower";
            this.textAsyncPower.Size = new System.Drawing.Size(176, 22);
            this.textAsyncPower.TabIndex = 9;
            // 
            // textTotalPower
            // 
            this.textTotalPower.Location = new System.Drawing.Point(121, 96);
            this.textTotalPower.Name = "textTotalPower";
            this.textTotalPower.Size = new System.Drawing.Size(176, 22);
            this.textTotalPower.TabIndex = 10;
            // 
            // ParseModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 435);
            this.Controls.Add(this.textTotalPower);
            this.Controls.Add(this.textAsyncPower);
            this.Controls.Add(this.textAppPower);
            this.Controls.Add(this.textWorkingDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.listBoxStatus);
            this.Name = "ParseModel";
            this.Text = "Step 5 : ParseModel (Final Step)";
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxStatus;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textWorkingDir;
        private System.Windows.Forms.TextBox textAppPower;
        private System.Windows.Forms.TextBox textAsyncPower;
        private System.Windows.Forms.TextBox textTotalPower;

    }
}

