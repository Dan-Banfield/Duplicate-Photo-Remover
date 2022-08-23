using System;
using System.IO;
using System.Linq;
using System.CodeDom;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Duplicate_Photo_Remover
{
    public partial class MainForm : Form
    {
        private string selectedDirectory = null;

        #region Global Variables

        private string SelectedDirectory
        {
            get
            {
                return selectedDirectory;
            }
            set
            {
                selectedDirectory = value;
                selectedDirectoryTextBox.Text = selectedDirectory;
            }
        }

        private enum Status
        {
            READY,
            BUSY
        };

        private Status currentStatus = Status.READY;

        #endregion

        // Enables double-buffering.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;    
                return handleParam;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void selectDirectoryButton_Click(object sender, System.EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SelectedDirectory = fbd.SelectedPath;
                }
            }
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            if (SelectedDirectory == null)
            {
                ShowErrorMessage("Please select a valid directory first.");
                return;
            }
            DeleteDuplicatePhotos(SelectedDirectory);
        }

        private void closeButton_Click(object sender, System.EventArgs e) =>
            Application.Exit();

        #endregion

        #region Methods

        private async void DeleteDuplicatePhotos(string directory)
        {
            SetStatus(Status.BUSY);

            int fileCount = await GetDirectoryFileCount(directory);
            statusProgressBar.Maximum = fileCount;

            HashSet<string> fileHashList = new HashSet<string>();
            string tempMD5Hash = string.Empty;

            foreach (string file in Directory.GetFiles(directory))
            {
                await Task.Run(() => tempMD5Hash = GenerateMD5Checksum(file));
                if (fileHashList.Contains(tempMD5Hash))
                {
                    await Task.Run(() => File.Delete(file));
                }
                else
                {
                    fileHashList.Add(tempMD5Hash);
                }
                statusProgressBar.Value += 1;
            }

            ShowInformationMessage("All duplicate photos were deleted successfully.");
            SetStatus(Status.READY);
        }

        private void SetStatus(Status status)
        {
            currentStatus = status;

            switch (status)
            {
                case Status.READY:
                    statusLabel.Text = "READY";
                    statusProgressBar.Value = 0;
                    startButton.Enabled = true;
                    break;
                case Status.BUSY:
                    statusLabel.Text = "BUSY";
                    startButton.Enabled = false;
                    break;
            }
        }

        private async Task<int> GetDirectoryFileCount(string directory)
        {
            int result = 0;

            await Task.Run(() => 
            {
                result = Directory.GetFiles(directory).Count();
            });

            return result;
        }

        private string GenerateMD5Checksum(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    byte[] hash = md5.ComputeHash(fileStream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInformationMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
