using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzles
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string getLife()
        {
            return numericUpDown1.Text;
        }
        public string getTime()
        {
            return numericUpDown2.Text;
        }

    }
}
