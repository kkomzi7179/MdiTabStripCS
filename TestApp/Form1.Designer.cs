namespace TestApp
{
	partial class Form1
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.mdiTabStrip1 = new MdiTabStripCS.MdiTabStrip();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			((System.ComponentModel.ISupportInitialize)(this.mdiTabStrip1)).BeginInit();
			this.SuspendLayout();
			// 
			// mdiTabStrip1
			// 
			this.mdiTabStrip1.ActiveTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.mdiTabStrip1.AllowDrop = true;
			this.mdiTabStrip1.Dock = System.Windows.Forms.DockStyle.Top;
			this.mdiTabStrip1.InactiveTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.mdiTabStrip1.Location = new System.Drawing.Point(0, 25);
			this.mdiTabStrip1.MdiNewTabImage = null;
			this.mdiTabStrip1.MdiNewTabVisible = true;
			this.mdiTabStrip1.MdiWindowState = MdiTabStripCS.MdiChildWindowState.Maximized;
			this.mdiTabStrip1.MinimumSize = new System.Drawing.Size(50, 33);
			this.mdiTabStrip1.MouseOverTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.mdiTabStrip1.Name = "mdiTabStrip1";
			this.mdiTabStrip1.NewTabToolTipText = "New Tab";
			this.mdiTabStrip1.Padding = new System.Windows.Forms.Padding(5, 3, 20, 5);
			this.mdiTabStrip1.Size = new System.Drawing.Size(284, 35);
			this.mdiTabStrip1.TabIndex = 0;
			this.mdiTabStrip1.Text = "mdiTabStrip1";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(284, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.mdiTabStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.mdiTabStrip1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MdiTabStripCS.MdiTabStrip mdiTabStrip1;
		private System.Windows.Forms.ToolStrip toolStrip1;
	}
}

