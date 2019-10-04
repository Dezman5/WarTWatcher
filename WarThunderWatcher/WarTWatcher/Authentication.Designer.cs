namespace WarTWatcher
{
    partial class Authentication
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
			this.Login = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.Password = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Auth = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Login
			// 
			this.Login.Location = new System.Drawing.Point(74, 9);
			this.Login.Name = "Login";
			this.Login.Size = new System.Drawing.Size(144, 20);
			this.Login.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Login:";
			// 
			// Password
			// 
			this.Password.Location = new System.Drawing.Point(74, 35);
			this.Password.Name = "Password";
			this.Password.PasswordChar = '*';
			this.Password.Size = new System.Drawing.Size(144, 20);
			this.Password.TabIndex = 2;
			this.Password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Password_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Password:";
			// 
			// Auth
			// 
			this.Auth.Location = new System.Drawing.Point(15, 61);
			this.Auth.Name = "Auth";
			this.Auth.Size = new System.Drawing.Size(203, 23);
			this.Auth.TabIndex = 4;
			this.Auth.Text = "Log in";
			this.Auth.UseVisualStyleBackColor = true;
			this.Auth.Click += new System.EventHandler(this.Auth_Click);
			// 
			// Authentication
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(230, 95);
			this.Controls.Add(this.Auth);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Password);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Login);
			this.Name = "Authentication";
			this.Text = "Authentication";
			this.Load += new System.EventHandler(this.Authentication_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Login;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Auth;
	}
}