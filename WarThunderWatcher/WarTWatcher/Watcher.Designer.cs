namespace WarTWatcher
{
	partial class Watcher
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.test = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// test
			// 
			this.test.Location = new System.Drawing.Point(12, 26);
			this.test.Name = "test";
			this.test.Size = new System.Drawing.Size(187, 82);
			this.test.TabIndex = 13;
			this.test.Text = "Старт";
			this.test.UseVisualStyleBackColor = true;
			this.test.Click += new System.EventHandler(this.test_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(234, 26);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(187, 82);
			this.button1.TabIndex = 14;
			this.button1.Text = "Стоп";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// Watcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(433, 129);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.test);
			this.Name = "Watcher";
			this.Text = "Watcher";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Watcher_FormClosed);
			this.Load += new System.EventHandler(this.Watcher_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button test;
		private System.Windows.Forms.Button button1;
	}
}

