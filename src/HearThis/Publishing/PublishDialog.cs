using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using L10NSharp;

namespace HearThis.Publishing
{
	public partial class PublishDialog : Form
	{
		private readonly PublishingModel _model;

		enum State
		{
			Setup,
			Working,
			Success,
			Failure
		}

		private State _state = State.Setup;
		private BackgroundWorker _worker;

		public PublishDialog(PublishingModel model)
		{
			InitializeComponent();
			if (ReallyDesignMode)
				return;
			_model = model;
			_logBox.ShowDetailsMenuItem = true;
			_logBox.ShowCopyToClipboardMenuItem = true;
			UpdateDisplay(State.Setup);
		}
		protected new bool ReallyDesignMode
		{
			get
			{
				return (base.DesignMode || GetService(typeof(IDesignerHost)) != null) ||
					(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			}
		}
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void UpdateDisplay(State state)
		{
			_state = state;
			UpdateDisplay();
		}
		private void UpdateDisplay()
		{
			_destinationLabel.Text = _model.PublishThisProjectPath;

			switch (_state)
			{
				case State.Setup:
					string tooltip;
					_flacRadio.Enabled = true;
					_oggRadio.Enabled = FlacEncoder.IsAvailable(out tooltip);
					toolTip1.SetToolTip(_oggRadio, tooltip);
					_mp3Radio.Enabled = LameEncoder.IsAvailable(out tooltip);
					_saberRadio.Enabled = _mp3Radio.Enabled;
					toolTip1.SetToolTip(_mp3Radio, tooltip);
					_mp3Link.Visible = !_mp3Radio.Enabled;
					_saberLink.Visible = !_saberRadio.Enabled;
					_megavoiceRadio.Enabled = true;
					break;
				case State.Working:
					_publishButton.Enabled = false;
					_flacRadio.Enabled = _oggRadio.Enabled = _mp3Radio.Enabled = _saberRadio.Enabled = _megavoiceRadio.Enabled = false;
					break;
				case State.Success:
					 button1.Text = GetCloseTextForCancelButton();
					 _flacRadio.Enabled = _oggRadio.Enabled = _mp3Radio.Enabled = _saberRadio.Enabled = _megavoiceRadio.Enabled = false;
					_publishButton.Enabled = false;
					_openFolderLink.Text = _model.PublishThisProjectPath;
					_openFolderLink.Visible = true;
					break;
				case State.Failure:
					button1.Text = GetCloseTextForCancelButton();
					_flacRadio.Enabled = _oggRadio.Enabled = _mp3Radio.Enabled = _saberRadio.Enabled = _megavoiceRadio.Enabled = false;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static string GetCloseTextForCancelButton()
		{
			return LocalizationManager.GetString("PublishDialog.Close","&Close", "Cancel Button text changes to this after successful publish");
		}

		private void _publishButton_Click(object sender, EventArgs e)
		{

			if (_saberRadio.Checked)
				_model.PublishingMethod = new SaberPublishingMethod();
			else if(_megavoiceRadio.Checked)
				_model.PublishingMethod = new MegaVoicePublishingMethod();
			else if (_mp3Radio.Checked)
				_model.PublishingMethod = new BunchOfFilesPublishingMethod(new LameEncoder());
			else if (_flacRadio.Checked)
				_model.PublishingMethod = new BunchOfFilesPublishingMethod(new FlacEncoder());
			else if (_oggRadio.Checked)
				_model.PublishingMethod = new BunchOfFilesPublishingMethod(new OggEncoder());


			//IAudioEncoder encoder = _mp3Radio.Enabled ? new LameEncoder() : new FlacEncoder();
			UpdateDisplay(State.Working);
			_worker = new BackgroundWorker();
			_worker.DoWork += new DoWorkEventHandler(_worker_DoWork);
			_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
			_worker.WorkerSupportsCancellation = true;
			_worker.RunWorkerAsync();

			UpdateDisplay(State.Working);
		}

		void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			 UpdateDisplay();
		}

		void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			_state = _model.Publish(_logBox) ? State.Success : State.Failure;
		}

		private void _openFolderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(_model.PublishThisProjectPath);
		}

		private void _mp3Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show(LocalizationManager.GetString("PublishDialog.Restart", "Before or after installing 'Lame for Audacity', you'll need to restart HearThis"));
			Process.Start("http://audacity.sourceforge.net/help/faq?s=install&i=lame-mp3");
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			if(_worker ==null || !_worker.IsBusy)
			{
				Close();
				return;
			}

			_logBox.CancelRequested = true;

			if(_worker!=null)
				_worker.CancelAsync();
		}

		private void _changeDestinationLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (var dlg = new FolderBrowserDialog())
			{
				dlg.SelectedPath = _model.PublishRootPath;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					_model.PublishRootPath = dlg.SelectedPath;
					UpdateDisplay();
				}
			}
		}
	}
}
