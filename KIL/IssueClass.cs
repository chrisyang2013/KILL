using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIL
{
    public class IssueClass
    {
        private string m_issue;
        public string Issue
        {
            get { return m_issue; }
            set { m_issue = value; }
        }

        private DateTime m_date;
        public string Date
        {
            get { return m_date.ToString("MM-dd-yyyy"); }
            set { m_date = DateTime.Parse(value);}
        }

        private bool m_done;
        public bool Done
        {
            get { return m_done; }
            set { m_done = value;}
        }

        public IssueClass() { }

        public IssueClass(string date, string issue, bool done)
        {
            m_date = DateTime.Parse(date);
            m_issue = issue;
            m_done = done;
        }
    }
}
