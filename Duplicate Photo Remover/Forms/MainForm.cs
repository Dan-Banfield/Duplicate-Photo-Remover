using System.Windows.Forms;

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
                MessageBox.Show("Please select a valid directory first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DeleteDuplicateFiles(SelectedDirectory);
        }

        private void closeButton_Click(object sender, System.EventArgs e) =>
            Application.Exit();

        #endregion

        #region Methods

        private void DeleteDuplicateFiles(string directory)
        {
            // TODO: Delete duplicate files.
        }

        #endregion
    }
}
