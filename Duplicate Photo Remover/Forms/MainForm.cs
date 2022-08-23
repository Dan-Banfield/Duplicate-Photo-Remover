using System.Windows.Forms;

namespace Duplicate_Photo_Remover
{
    public partial class MainForm : Form
    {
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
    }
}
