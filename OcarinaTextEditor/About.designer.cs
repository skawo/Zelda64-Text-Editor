namespace Zelda64TextEditor
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.CreditsHeader = new System.Windows.Forms.Label();
            this.Credits = new System.Windows.Forms.Label();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.LblVersionX = new System.Windows.Forms.Label();
            this.LblVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // CreditsHeader
            // 
            this.CreditsHeader.AutoSize = true;
            this.CreditsHeader.BackColor = System.Drawing.Color.White;
            this.CreditsHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditsHeader.Location = new System.Drawing.Point(252, 103);
            this.CreditsHeader.Name = "CreditsHeader";
            this.CreditsHeader.Size = new System.Drawing.Size(50, 13);
            this.CreditsHeader.TabIndex = 4;
            this.CreditsHeader.Text = "Credits:";
            // 
            // Credits
            // 
            this.Credits.AutoSize = true;
            this.Credits.BackColor = System.Drawing.Color.White;
            this.Credits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Credits.Location = new System.Drawing.Point(252, 127);
            this.Credits.Name = "Credits";
            this.Credits.Size = new System.Drawing.Size(154, 130);
            this.Credits.TabIndex = 5;
            this.Credits.Text = "Programming:\r\nSageofMirrors, Skawo\r\n\r\nSpecial Thanks:\r\nZelda 64 Decompilation Tea" +
    "ms\r\nCloudmodding Wiki\r\nHylian Modding Server\r\n\r\nDedicated to CrookedPoe\r\nR.I.P";
            // 
            // Logo
            // 
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(13, 36);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(233, 221);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logo.TabIndex = 7;
            this.Logo.TabStop = false;
            // 
            // LblVersionX
            // 
            this.LblVersionX.AutoSize = true;
            this.LblVersionX.BackColor = System.Drawing.Color.White;
            this.LblVersionX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersionX.Location = new System.Drawing.Point(296, 73);
            this.LblVersionX.Name = "LblVersionX";
            this.LblVersionX.Size = new System.Drawing.Size(57, 13);
            this.LblVersionX.TabIndex = 8;
            this.LblVersionX.Text = "Version: ";
            // 
            // LblVersion
            // 
            this.LblVersion.AutoSize = true;
            this.LblVersion.BackColor = System.Drawing.Color.White;
            this.LblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersion.Location = new System.Drawing.Point(359, 73);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(39, 13);
            this.LblVersion.TabIndex = 9;
            this.LblVersion.Text = "X.X.X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(274, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "ZELDA 64";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(251, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "TEXT EDITOR";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(414, 270);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LblVersion);
            this.Controls.Add(this.LblVersionX);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.Credits);
            this.Controls.Add(this.CreditsHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label CreditsHeader;
        private System.Windows.Forms.Label Credits;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Label LblVersionX;
        private System.Windows.Forms.Label LblVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}