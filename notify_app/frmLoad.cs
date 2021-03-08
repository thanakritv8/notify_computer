using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notify_app
{
    public partial class frmLoad : Form
    {
        public frmLoad()
        {
            InitializeComponent();
        }

        Thread _t;

        private TMTNotifyEntities _context = new TMTNotifyEntities();

        private void frmLoad_Load(object sender, EventArgs e)
        {
            _t = new Thread(new ThreadStart(run));
            _t.Start();
        }

        delegate void SetTextCallback(int num_rnd);

        private void SetText(int num_rnd)
        {
            if (this.label1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { num_rnd });
            }
            else
            {
                label1.Text = num_rnd.ToString(); ;
            }
        }

        private void run()
        {
            while (_t.IsAlive)
            {
                int num_rnd = RandomNumber(1, 1000);
                string com_name = Environment.MachineName;

                action_data ad = (from hh in _context.action_data
                                  where hh.computer_name == com_name
                                  select hh).FirstOrDefault();
                ad.num_rnd = num_rnd;
                ad.update_by = "admin";
                ad.update_date = DateTime.Now;
                _context.SaveChanges();


                SetText(num_rnd);

                Thread.Sleep(5000);
            }
        }

        private readonly Random _random = new Random();

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        private void frmLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            _t.Abort();
        }
    }
}
