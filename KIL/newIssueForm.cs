using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KIL
{
    public partial class newIssueForm : Form
    {
        private IssueClass m_issue = new IssueClass();
        public IssueClass getIssue() { return m_issue; }

        public newIssueForm()
        {
            InitializeComponent();
            this.Text = "Create new issue";
            dateLabel.Text += DateTime.Now.ToString("MM-dd-yyyy");
            m_issue.Date = DateTime.Now.ToString("MM-dd-yyyy");
            m_issue.Done = false;
            m_issue.Issue = "";
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            m_issue.Issue = "";
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            m_issue.Issue = string.Join(" ", issueTextBox.Lines);
            this.Close();
        }
    }
}
