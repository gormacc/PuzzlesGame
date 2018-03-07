using System;
using System.IO;
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
    public partial class Form1 : Form
    {
        public Label[] rowlabel;
        public Label[] columnlabel;
        public Button[,] buttons;
        public bool[,] fields;
        public bool[,] scored;
        public int[] rownumber;
        public int[] columnnumber;
        public int size = 4;
        public static int lifes = 3;
        public int lifeleft = 3;
        public static int time = 10;
        public static int fieldNumber = 0;
        public int fieldleft;
        public int timeleft = 10;
        public int score = 0;
        public Timer timer = new Timer();
        public EventHandler MouseLeft;
        public EventHandler MouseEnt;
        public MouseEventHandler MouseDow;
        public EventHandler TicToc;
        bool MDFLAG = false;
        bool TTFLAG = false;
        bool MDMODEFLAG = false;

        public Form1()
        {
            InitializeHandlers();
            InitializeComponent();
            InitializeLabels();
            InitializeButtons();
            InitializeDownStrip();
            SetFields();
        }

        public void InitializeHandlers()
        {
            this.MouseLeft = new System.EventHandler(this.MouseLeaveEve);
            this.MouseEnt = new System.EventHandler(this.MouseEnterEve);
            this.MouseDow = new System.Windows.Forms.MouseEventHandler(this.MouseDownEve);
            this.TicToc = new System.EventHandler(this.timerTick);
        }
        public void InitializeDownStrip()
        {
            this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
            this.toolStripStatusLabel2.Text = string.Format("score: {0}", this.score);
            this.toolStripProgressBar1.Maximum = Form1.time;
            this.toolStripProgressBar1.Value = Form1.time;
        }
        public void InitializeLabels()
        {

            this.rowlabel = new Label[this.size];
            this.columnlabel = new Label[this.size];
            for (int i = 0; i < this.size; i++)
            {
                //wierszowo
                this.rowlabel[i] = new Label();
                this.rowlabel[i].AutoSize = true;
                this.rowlabel[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
                this.rowlabel[i].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
                this.rowlabel[i].Name = string.Format("rowlabel{0}", i);
                this.rowlabel[i].Text = "";
                this.rowlabel[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.tableLayoutPanel1.Controls.Add(this.rowlabel[i], i + 1, 0);
                // kolumnowo
                this.columnlabel[i] = new Label();
                this.columnlabel[i].AutoSize = true;
                this.columnlabel[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
                this.columnlabel[i].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
                this.columnlabel[i].Name = string.Format("columnlabel{0}", i);
                this.columnlabel[i].Text = "";
                this.columnlabel[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.tableLayoutPanel1.Controls.Add(this.columnlabel[i], 0, i + 1);
            }
        }

        public void InitializeButtons()
        {
            this.buttons = new Button[this.size, this.size];

            for (int i = 0; i < this.size; i++)
                for (int j = 0; j < this.size; j++)
                {
                    this.buttons[i, j] = new Button();
                    this.buttons[i, j].AllowDrop = true;
                    this.buttons[i, j].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
                    this.buttons[i, j].BackColor = System.Drawing.Color.BlueViolet;
                    this.buttons[i, j].Text = "?";
                    this.buttons[i, j].Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
                    this.buttons[i, j].BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                    this.buttons[i, j].Name = string.Format("button{0}{1}", i, j);
                    this.buttons[i, j].MouseEnter += this.MouseEnt;
                    this.buttons[i, j].MouseLeave += this.MouseLeft;
                    this.tableLayoutPanel1.Controls.Add(this.buttons[i, j], j + 1, i + 1);
                }
        }

        public void SetFields()
        {
            this.fields = new bool[this.size, this.size];
            this.scored = new bool[this.size, this.size];
            this.columnnumber = new int[this.size];
            this.rownumber = new int[this.size];
            Form1.fieldNumber = 0;
        }

        public void InitializeFields()
        {
            SetFields();
            int num;
            Random rand = new Random();
            for (int i = 0; i < this.size; i++)
                for (int j = 0; j < this.size; j++)
                {
                    num = rand.Next();
                    if (num % 2 == 0)
                    {
                        this.fields[i, j] = true;
                        this.rownumber[i] += 1;
                        this.columnnumber[j] += 1;
                        Form1.fieldNumber += 1;
                    }
                }
            for (int i = 0; i < this.size; i++)
            {
                this.rowlabel[i].Text = string.Format("{0}", this.rownumber[i]);
                this.columnlabel[i].Text = string.Format("{0}", this.columnnumber[i]);
            }
            this.fieldleft = Form1.fieldNumber;
        }

        private void MouseEnterEve(object sender, System.EventArgs e)
        {
            if (((Button)sender).BackColor == System.Drawing.Color.BlueViolet)
            {
                ((Button)sender).BackColor = System.Drawing.Color.Yellow;
                ((Button)sender).Text = "";
            }
        }

        private void MouseLeaveEve(object sender, System.EventArgs e)
        {
            if (((Button)sender).BackColor == System.Drawing.Color.Yellow)
            {
                ((Button)sender).BackColor = System.Drawing.Color.BlueViolet;
                ((Button)sender).Text = "?";
            }
        }

        private void TurnRed(object sender , EventArgs e, int i , int j , Timer timer)
        {
            this.buttons[j,i].BackColor = System.Drawing.Color.BlueViolet;
            this.buttons[j,i].Text = "?";
            timer.Stop();
        }

        private void MouseDownEve(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            string name = ((Button)sender).Name;
            int i = int.Parse(name[7].ToString());
            int j = int.Parse(name[6].ToString());
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

               if (!this.fields[i, j])                                   
                {
                    ((Button)sender).BackColor = System.Drawing.Color.Red;
                    ((Button)sender).Text = "";
                    Timer newTimer = new Timer();
                    newTimer.Interval = 600;
                    newTimer.Tick += (object senderB, EventArgs eB) => TurnRed(senderB, eB, i, j,newTimer);
                    newTimer.Start();
                    


                    this.lifeleft -= 1;
                    this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
                    if (this.lifeleft == 0)
                        Congratz();
                }
                else
                {
                    ((Button)sender).BackColor = System.Drawing.Color.Black;
                    if (!this.scored[i, j])
                    {
                        this.scored[i, j] = true;
                        this.score += 50;
                        this.fieldleft -= 1;
                        this.toolStripStatusLabel2.Text = string.Format("score: {0}", this.score);
                    }
                    if(this.fieldleft == 0)
                    {
                        InitializeFields();
                        for (int k = 0; k < this.size; k++)
                            for (int l = 0; l < this.size; l++)
                            {
                                this.buttons[k, l].BackColor = System.Drawing.Color.BlueViolet;
                                this.buttons[k, l].Text = "?";
                            }

                        if (!this.MDFLAG)
                        {
                            for (int k = 0; k < this.size; k++)
                                for (int l = 0; l < this.size; l++)
                                    this.buttons[k, l].MouseDown += this.MouseDow;

                            this.MDFLAG = true;
                        }
                        this.toolStripProgressBar1.Value = Form1.time;
                        this.timeleft = Form1.time;
                        this.lifeleft = Form1.lifes;
                        this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
                        this.score += 500;
                        this.toolStripStatusLabel2.Text = string.Format("score: {0}", this.score);
                        this.timer.Start();
                        this.timer.Interval = 1000;
                        if (!this.TTFLAG)
                        {
                            this.timer.Tick += this.TicToc;
                            this.TTFLAG = true;
                        }
                    }
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (!this.scored[i, j])
                {
                    ((Button)sender).BackColor = System.Drawing.Color.White;
                    ((Button)sender).Text = "";
                }
            }

        }

        private void MouseDownEditMode(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            string name = ((Button)sender).Name;
            int i = int.Parse(name[7].ToString());
            int j = int.Parse(name[6].ToString());

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ((Button)sender).BackColor = System.Drawing.Color.Black;
                this.fields[i, j] = true;
            }
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ((Button)sender).BackColor = System.Drawing.Color.White;
                this.fields[i, j] = false;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) // Edit Mode Button
        {
            this.toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.toolStripMenuItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStrip1.BackColor = System.Drawing.Color.Yellow;
            this.fields = new bool[this.size, this.size];
            this.scored = new bool[this.size, this.size];
            this.rownumber = new int[this.size];
            this.columnnumber = new int[this.size];
            this.toolStripMenuItem1.Enabled = false;
            this.settingsToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = true;

            if (this.TTFLAG)
                this.timer.Stop();

            for (int i = 0; i < this.size; i++)
                for (int j = 0; j < this.size; j++)
                {
                    this.buttons[i, j].BackColor = System.Drawing.Color.White;
                    this.buttons[i, j].Text = "";
                }

            for (int i = 0; i < this.size; i++)
            {
                this.rowlabel[i].Text = "";
                this.columnlabel[i].Text = "";
            }

            if (this.MDFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        this.buttons[i, j].MouseDown -= this.MouseDow;
                this.MDFLAG = false;
            }

            if (!this.MDMODEFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                    {
                        this.buttons[i, j].MouseLeave -= this.MouseLeft;
                        this.buttons[i, j].MouseEnter -= this.MouseEnt;
                        this.buttons[i, j].MouseDown += new MouseEventHandler(this.MouseDownEditMode);
                    }
                this.MDMODEFLAG = true;
            }


        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e) // GameButton
        {
            this.toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem3.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.menuStrip1.BackColor = System.Drawing.Color.Empty;
            this.toolStripMenuItem1.Enabled = true;
            this.settingsToolStripMenuItem.Enabled = true;
            this.openToolStripMenuItem.Enabled = true;
            this.saveToolStripMenuItem.Enabled = false;


            for (int i = 0; i < this.size; i++)
                for (int j = 0; j < this.size; j++)
                {
                    this.buttons[i, j].BackColor = System.Drawing.Color.BlueViolet;
                    this.buttons[i, j].Text = "?";
                }

            if (!this.MDFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        this.buttons[i, j].MouseDown += this.MouseDow;
                this.MDFLAG = true;
            }

            if (this.MDMODEFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                    {
                        this.buttons[i, j].MouseLeave += this.MouseLeft;
                        this.buttons[i, j].MouseEnter += this.MouseEnt;
                        this.buttons[i, j].MouseDown -= new MouseEventHandler(this.MouseDownEditMode);
                    }
                Form1.fieldNumber = 0;
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        if (this.fields[i, j])
                        {
                            this.rownumber[i] += 1;
                            this.columnnumber[j] += 1;
                            Form1.fieldNumber += 1;
                        }
                this.fieldleft = Form1.fieldNumber;

                for (int i = 0; i < this.size; i++)
                {
                    this.rowlabel[i].Text = string.Format("{0}", this.rownumber[i]);
                    this.columnlabel[i].Text = string.Format("{0}", this.columnnumber[i]);
                }

                this.MDMODEFLAG = false;
            }
            // zaczyannie gry do nowa 

            this.toolStripProgressBar1.Value = Form1.time;
            this.timeleft = Form1.time;
            this.lifeleft = Form1.lifes;
            this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
            this.score = 0;
            this.toolStripStatusLabel2.Text = string.Format("score: {0}", this.score);
            this.timer.Start();
            this.timer.Interval = 1000;
            if (!this.TTFLAG)
            {
                this.timer.Tick += this.TicToc;
                this.TTFLAG = true;
            }


        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e) // New Game
        {
            InitializeFields();
            for (int i = 0; i < this.size; i++)
                for (int j = 0; j < this.size; j++)
                {
                    this.buttons[i, j].BackColor = System.Drawing.Color.BlueViolet;
                    this.buttons[i, j].Text = "?";
                }

            if (!this.MDFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        this.buttons[i, j].MouseDown += this.MouseDow;
                    
                this.MDFLAG = true;
            }
            this.toolStripProgressBar1.Value = Form1.time;
            this.timeleft = Form1.time;
            this.lifeleft = Form1.lifes;
            this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
            this.score = 0;
            this.toolStripStatusLabel2.Text = string.Format("score: {0}", this.score);
            this.timer.Start();
            this.timer.Interval = 1000;
            if (!this.TTFLAG)
            {
                this.timer.Tick += this.TicToc;
                this.TTFLAG = true;
            }
        }
        private void timerTick(object sender, System.EventArgs e)
        {
            this.toolStripProgressBar1.Value -= 1;
            if (this.toolStripProgressBar1.Value == 0)
                Congratz();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TTFLAG)
                this.timer.Stop();
            Form2 form = new Form2();
            DialogResult dr = form.ShowDialog(this);

            if(this.MDFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        this.buttons[i, j].MouseDown -= this.MouseDow;
                this.MDFLAG = false;
            }

            if (dr == DialogResult.OK)
            {
                Form1.lifes = int.Parse(form.getLife());
                this.lifeleft = Form1.lifes;
                this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
                Form1.time = int.Parse(form.getTime());
                InitializeDownStrip();
                form.Close();

            }
        } // Settings

        private void Congratz()
        {
            string message = string.Format("Your final score is : {0}", this.score);
            this.timer.Stop();
            if (this.MDFLAG)
            {
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        this.buttons[i, j].MouseDown -= this.MouseDow;
                this.MDFLAG = false;
            }
            MessageBox.Show(this, message, "Congratulations");
        }

        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            this.timer.Stop();

            switch (MessageBox.Show(this, "Are you sure?", "Confirmation", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) // save
        {
            if (this.TTFLAG)
                this.timer.Stop();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PuzzleGame file|*.pg";
            saveFileDialog1.Title = "Save a PuzzleGame file";
            saveFileDialog1.ShowDialog();


            if (saveFileDialog1.FileName != "")
            {

                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);

                for(int i = 0; i < this.size; i++)
                {
                    for (int j = 0; j < this.size; j++)
                    {
                        if (this.fields[i, j])
                            sw.Write("1");
                        else
                            sw.Write("0");
                    }
                    sw.WriteLine();
                }

                sw.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) // open
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "PuzzleGame file|*.pg";
            openFileDialog1.Title = "Open a PuzzleGame file";


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader sr = new StreamReader(openFileDialog1.FileName);

                            for (int i = 0; i < this.size; i++)
                            {
                                string text = sr.ReadLine();
                                for (int j = 0; j < this.size; j++)
                                {
                                    int a = int.Parse(text[j].ToString());
                                    if (a == 1)
                                        this.fields[i, j] = true;
                                    else
                                        this.fields[i, j] = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
                }


                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                    {
                        this.buttons[i, j].BackColor = System.Drawing.Color.BlueViolet;
                        this.buttons[i, j].Text = "?";
                    }

                if (!this.MDFLAG)
                {
                    for (int i = 0; i < this.size; i++)
                        for (int j = 0; j < this.size; j++)
                            this.buttons[i, j].MouseDown += this.MouseDow;
                    this.MDFLAG = true;
                }

                this.rownumber = new int[this.size];
                this.columnnumber = new int[this.size];
                Form1.fieldNumber = 0;
                for (int i = 0; i < this.size; i++)
                    for (int j = 0; j < this.size; j++)
                        if (this.fields[i, j])
                        {
                            this.rownumber[i] += 1;
                            this.columnnumber[j] += 1;
                            Form1.fieldNumber += 1;
                        }
                this.fieldleft = Form1.fieldNumber;

                for (int i = 0; i < this.size; i++)
                {
                    this.rowlabel[i].Text = string.Format("{0}", this.rownumber[i]);
                    this.columnlabel[i].Text = string.Format("{0}", this.columnnumber[i]);
                }

                this.toolStripProgressBar1.Value = Form1.time;
                this.timeleft = Form1.time;
                this.lifeleft = Form1.lifes;
                this.toolStripStatusLabel1.Text = string.Format("lifes: {0}", this.lifeleft);
                this.score = 0;
                this.toolStripStatusLabel2.Text = string.Format("score: {0}", this.score);
                this.timer.Start();
                this.timer.Interval = 1000;
                if (!this.TTFLAG)
                {
                    this.timer.Tick += this.TicToc;
                    this.TTFLAG = true;
                }

            }
        }

    }  
}
