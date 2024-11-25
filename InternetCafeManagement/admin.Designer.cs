namespace InternetCafeManagement
{
    partial class admin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(admin));
            this.pictureUsers = new System.Windows.Forms.PictureBox();
            this.ComputersBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComputersBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureUsers
            // 
            this.pictureUsers.Image = ((System.Drawing.Image)(resources.GetObject("pictureUsers.Image")));
            this.pictureUsers.Location = new System.Drawing.Point(69, 48);
            this.pictureUsers.Name = "pictureUsers";
            this.pictureUsers.Size = new System.Drawing.Size(147, 135);
            this.pictureUsers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureUsers.TabIndex = 7;
            this.pictureUsers.TabStop = false;
            // 
            // ComputersBox
            // 
            this.ComputersBox.Image = ((System.Drawing.Image)(resources.GetObject("ComputersBox.Image")));
            this.ComputersBox.Location = new System.Drawing.Point(310, 48);
            this.ComputersBox.Name = "ComputersBox";
            this.ComputersBox.Size = new System.Drawing.Size(147, 135);
            this.ComputersBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ComputersBox.TabIndex = 6;
            this.ComputersBox.TabStop = false;
            // 
            // admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 554);
            this.Controls.Add(this.pictureUsers);
            this.Controls.Add(this.ComputersBox);
            this.Name = "admin";
            this.Text = "admin";
            this.Load += new System.EventHandler(this.admin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComputersBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureUsers;
        private System.Windows.Forms.PictureBox ComputersBox;
    }
}