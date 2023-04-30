using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Zelda64TextEditor
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            LblVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        }
    }
}
