using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KIL
{

    public partial class Form1 : Form
    {

        private Queue<IssueClass> allIssues = new Queue<IssueClass>();
        private Queue<IssueClass> activeIssues = new Queue<IssueClass>();
        private Queue<IssueClass> completedIssues = new Queue<IssueClass>();

        private string path = @"\\PRODDATA-DC1\Groups\Clients\WLP2\Known Issues.txt";
        //private string path = @"C:\Users\Zaidongy\Desktop\Known Issues.txt";

        public Form1()
        {
            InitializeComponent();
            displayContents();
        }

        private void displayContents(bool showActive = false, bool showCompleted = false)
        {
            clearCheckBoxes();
            getAllIssues();
            getSomeIssues();

            if (!showActive && !showCompleted)
                addCheckBoxesToControl(allIssues);
            else if (showActive)
                addCheckBoxesToControl(activeIssues);
            else //show completed
                addCheckBoxesToControl(completedIssues);
        }

        void getAllIssues()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                int i = 0;
                string curIssue = "", issue = "", X = "";
                while (!reader.EndOfStream)
                {
                    string date = "";
                    try
                    {
                        string line = reader.ReadLine().Trim();
                        if (line == string.Empty || line == "Date Reported | Issue")
                            continue;
                        string[] split1 = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] split2 = split1[0].Split(new char[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                        if (split1.Length == 2)
                            issue = split1[1];
                        else
                            issue = split1[0];
                        curIssue = curIssue + " " + issue.Trim();

                        date = split2[1];
                        X = split2[0];
                    }
                    catch (Exception Ex)
                    {
                        if (Ex is EndOfStreamException)
                            return;
                        else if (Ex is IndexOutOfRangeException)
                        { ; }
                        else { continue; }
                    }

                    bool newIssue = true;
                    try
                    {
                        DateTime dt = DateTime.Parse(date);
                        curIssue = issue;
                    }
                    catch (Exception) { newIssue = false; }

                    IssueClass Is;
                    if (!newIssue)
                    {
                        if (allIssues.Count() == 0)
                            continue;
                        Is = allIssues.LastOrDefault();
                        Is.Issue = curIssue;
                        continue;
                    }

                    Is = new IssueClass();
                    bool hasX;
                    if (X.ToUpper().Contains("X"))
                        hasX = true;
                    else
                        hasX = false;
                    Is.Date = date;
                    Is.Issue = issue;
                    Is.Done = hasX;
                    allIssues.Enqueue(Is);
                    i++;
                }
            }
        }

        void getSomeIssues()
        {
            foreach (IssueClass issue in allIssues)
            {
                if (!issue.Done)
                    activeIssues.Enqueue(issue);
                else
                    completedIssues.Enqueue(issue);
            }
        }

        void clearCheckBoxes()
        {
            //reset form to empty state          
            for (int i = 0; i < this.Controls.Count; i++)
            {
                try
                {
                    ((CheckBox)this.Controls[i]).Dispose();
                    i--;
                }
                catch (InvalidCastException) { ;}
            }

            allIssues.Clear();
            activeIssues.Clear();
            completedIssues.Clear();
        }

        void addCheckBoxesToControl(Queue<IssueClass> IssuesToBeAdded)
        {
            int i = 0;
            foreach (IssueClass issue in IssuesToBeAdded)
            {
                CheckBox cb = new CheckBox();
                cb.Text = issue.Issue;
                cb.Checked = issue.Done;
                cb.Enabled = !cb.Checked;
                cb.Location = new Point(40, 60 + (25 * i));
                cb.AutoSize = true;
                this.Controls.Add(cb);
                i++;
            }
        }

        private void onlyShowActiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onlyShowActiveToolStripMenuItem.Checked = !onlyShowActiveToolStripMenuItem.Checked;
            onlyShowCompletedToolStripMenuItem.Checked = false;
            displayContents(onlyShowActiveToolStripMenuItem.Checked, onlyShowCompletedToolStripMenuItem.Checked);
        }

        private void onlyShowCompletedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onlyShowCompletedToolStripMenuItem.Checked = !onlyShowCompletedToolStripMenuItem.Checked;
            onlyShowActiveToolStripMenuItem.Checked = false;
            displayContents(onlyShowActiveToolStripMenuItem.Checked, onlyShowCompletedToolStripMenuItem.Checked);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newIssueForm issueForm = new newIssueForm();
            issueForm.ShowDialog();
            if (issueForm.getIssue().Issue == "")
                return;//user canceled
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("");
                writer.WriteLine("[ ] " + issueForm.getIssue().Date + " | " + issueForm.getIssue().Issue);
            }
            //MessageBox.Show("New issue added");
            displayContents();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //foreach(IssueClass issue in allIssues)
            //{
            //    using (StreamReader reader = new StreamReader(path))
            //    {
            //        int nLine = 0;
            //        while (!reader.EndOfStream)
            //        {
            //            string line = reader.ReadLine();
            //            if (line.Contains(issue.Date))
            //            {
            //                string[] fileLines = File.ReadAllLines(path);
            //                //add the x in the file
            //            }
            //        }
            //    }
            //}

            displayContents();
        }
    }
}
