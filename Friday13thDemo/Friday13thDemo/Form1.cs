using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Friday13thDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<DateTime> allDates = new List<DateTime>();
        private void button1_Click(object sender, EventArgs e)
        {
            allDates.Clear();
            listBoxDates.Items.Clear();

            DateTime start = DateTime.Today;
            DateTime end = start.AddYears(5);

            DateTime date = new DateTime(start.Year, start.Month, 13);

            // move backwards if start date is after this month's 13th
            if (start.Day > 13)
                date = date.AddMonths(1);

            // Loop through every month for 5 years
            while (date <= end)
            {
                allDates.Add(date);
                listBoxDates.Items.Add(date.ToString("dd MMMM yyyy"));

                date = date.AddMonths(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBoxDates.Items.Clear();

            var fridays = allDates.FindAll(d => d.DayOfWeek == DayOfWeek.Friday);

            foreach (var d in fridays)
            {
                listBoxDates.Items.Add(d.ToString("dd MMMM yyyy"));
            }
        }
    }
}
