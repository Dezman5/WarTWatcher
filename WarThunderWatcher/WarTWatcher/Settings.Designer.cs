namespace WarTWatcher
{
	partial class Settings
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
			this.Player2ListBox = new System.Windows.Forms.ComboBox();
			this.Player1ListBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(35, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Команда1:";
			// 
			// Player2ListBox
			// 
			this.Player2ListBox.FormattingEnabled = true;
			this.Player2ListBox.Location = new System.Drawing.Point(125, 54);
			this.Player2ListBox.Name = "Player2ListBox";
			this.Player2ListBox.Size = new System.Drawing.Size(245, 21);
			this.Player2ListBox.TabIndex = 20;
			// 
			// Player1ListBox
			// 
			this.Player1ListBox.FormattingEnabled = true;
			this.Player1ListBox.Location = new System.Drawing.Point(125, 26);
			this.Player1ListBox.Name = "Player1ListBox";
			this.Player1ListBox.Size = new System.Drawing.Size(245, 21);
			this.Player1ListBox.TabIndex = 19;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(35, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "Команда2:";
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(410, 97);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Player2ListBox);
			this.Controls.Add(this.Player1ListBox);
			this.Controls.Add(this.label1);
			this.Name = "Settings";
			this.Text = "Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.ComboBox Player2ListBox;
		public System.Windows.Forms.ComboBox Player1ListBox;
		private System.Windows.Forms.Label label2;
	}
}