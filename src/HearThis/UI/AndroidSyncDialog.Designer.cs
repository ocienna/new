namespace HearThis.UI
{
	partial class AndroidSyncDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AndroidSyncDialog));
			this.qrBox = new System.Windows.Forms.PictureBox();
			this.betterLabel1 = new SIL.Windows.Forms.Widgets.BetterLabel();
			this.betterLabel2 = new SIL.Windows.Forms.Widgets.BetterLabel();
			this.betterLabel3 = new SIL.Windows.Forms.Widgets.BetterLabel();
			this.playStoreLinkLabel = new SIL.Windows.Forms.Widgets.BetterLinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this._ipAddressBox = new System.Windows.Forms.TextBox();
			this._syncButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.qrBox)).BeginInit();
			this.SuspendLayout();
			// 
			// qrBox
			// 
			this.qrBox.Location = new System.Drawing.Point(25, 194);
			this.qrBox.Name = "qrBox";
			this.qrBox.Size = new System.Drawing.Size(151, 126);
			this.qrBox.TabIndex = 1;
			this.qrBox.TabStop = false;
			// 
			// betterLabel1
			// 
			this.betterLabel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.betterLabel1.Enabled = false;
			this.betterLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.betterLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.betterLabel1.IsTextSelectable = false;
			this.betterLabel1.Location = new System.Drawing.Point(25, 107);
			this.betterLabel1.Multiline = true;
			this.betterLabel1.Name = "betterLabel1";
			this.betterLabel1.ReadOnly = true;
			this.betterLabel1.Size = new System.Drawing.Size(335, 81);
			this.betterLabel1.TabIndex = 2;
			this.betterLabel1.TabStop = false;
			this.betterLabel1.Text = resources.GetString("betterLabel1.Text");
			// 
			// betterLabel2
			// 
			this.betterLabel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.betterLabel2.Enabled = false;
			this.betterLabel2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.betterLabel2.IsTextSelectable = false;
			this.betterLabel2.Location = new System.Drawing.Point(33, 96);
			this.betterLabel2.Multiline = true;
			this.betterLabel2.Name = "betterLabel2";
			this.betterLabel2.ReadOnly = true;
			this.betterLabel2.Size = new System.Drawing.Size(100, 0);
			this.betterLabel2.TabIndex = 3;
			this.betterLabel2.TabStop = false;
			// 
			// betterLabel3
			// 
			this.betterLabel3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.betterLabel3.Enabled = false;
			this.betterLabel3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.betterLabel3.IsTextSelectable = false;
			this.betterLabel3.Location = new System.Drawing.Point(25, 27);
			this.betterLabel3.Multiline = true;
			this.betterLabel3.Name = "betterLabel3";
			this.betterLabel3.ReadOnly = true;
			this.betterLabel3.Size = new System.Drawing.Size(335, 38);
			this.betterLabel3.TabIndex = 4;
			this.betterLabel3.TabStop = false;
			this.betterLabel3.Text = "HearThis Android is an experimental program that can synchronize with HearThis so" +
    " that one or more speakers can use their Android phones to do recordings.";
			// 
			// playStoreLinkLabel
			// 
			this.playStoreLinkLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.playStoreLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
			this.playStoreLinkLabel.ForeColor = System.Drawing.Color.Blue;
			this.playStoreLinkLabel.IsTextSelectable = false;
			this.playStoreLinkLabel.Location = new System.Drawing.Point(25, 70);
			this.playStoreLinkLabel.Multiline = true;
			this.playStoreLinkLabel.Name = "playStoreLinkLabel";
			this.playStoreLinkLabel.Size = new System.Drawing.Size(215, 17);
			this.playStoreLinkLabel.TabIndex = 5;
			this.playStoreLinkLabel.TabStop = false;
			this.playStoreLinkLabel.Text = "Become a tester of HearThis Android";
			this.playStoreLinkLabel.URL = "https://play.google.com/apps/testing/org.sil.hearthis";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(295, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "To sync with Android: tap Menu button, Sync, scan this code";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(22, 338);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(351, 32);
			this.label3.TabIndex = 3;
			this.label3.Text = "Alternatively, you can enter the code from the Android Synchronization screen her" +
    "e and then click \"Syncrhonize\".";
			// 
			// _ipAddressBox
			// 
			this._ipAddressBox.Location = new System.Drawing.Point(25, 382);
			this._ipAddressBox.Name = "_ipAddressBox";
			this._ipAddressBox.Size = new System.Drawing.Size(100, 20);
			this._ipAddressBox.TabIndex = 1;
			// 
			// _syncButton
			// 
			this._syncButton.Location = new System.Drawing.Point(144, 379);
			this._syncButton.Name = "_syncButton";
			this._syncButton.Size = new System.Drawing.Size(75, 23);
			this._syncButton.TabIndex = 0;
			this._syncButton.Text = "Synchronize";
			// 
			// AndroidSyncDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 448);
			this.Controls.Add(this._syncButton);
			this.Controls.Add(this._ipAddressBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.playStoreLinkLabel);
			this.Controls.Add(this.betterLabel3);
			this.Controls.Add(this.betterLabel2);
			this.Controls.Add(this.betterLabel1);
			this.Controls.Add(this.qrBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AndroidSyncDialog";
			this.Text = "Synchronize with HearThis for Android";
			((System.ComponentModel.ISupportInitialize)(this.qrBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.PictureBox qrBox;
		private SIL.Windows.Forms.Widgets.BetterLinkLabel playStoreLinkLabel;
		private SIL.Windows.Forms.Widgets.BetterLabel betterLabel1;
		private SIL.Windows.Forms.Widgets.BetterLabel betterLabel2;
		private SIL.Windows.Forms.Widgets.BetterLabel betterLabel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _ipAddressBox;
		private System.Windows.Forms.Button _syncButton;
	}
}