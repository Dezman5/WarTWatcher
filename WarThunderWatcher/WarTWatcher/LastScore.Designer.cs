namespace WarTWatcher
{
	partial class LastScore
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
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.Player1ListBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(85, 98);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(153, 44);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(85, 49);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(153, 20);
			this.textBox1.TabIndex = 1;
			// 
			// Player1ListBox
			// 
			this.Player1ListBox.FormattingEnabled = true;
			this.Player1ListBox.Location = new System.Drawing.Point(45, 12);
			this.Player1ListBox.Name = "Player1ListBox";
			this.Player1ListBox.Size = new System.Drawing.Size(245, 21);
			this.Player1ListBox.TabIndex = 20;
			// 
			// LastScore
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(335, 175);
			this.Controls.Add(this.Player1ListBox);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Name = "LastScore";
			this.Text = "LastScore";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		public System.Windows.Forms.ComboBox Player1ListBox;
	}
}