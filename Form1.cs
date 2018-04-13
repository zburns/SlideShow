using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SlideShow
{
    public partial class Form1 : Form
    {
        string foldertoshow = null;
        string[] files = null;
        string[] music = null;
        int currentfile = 0;
        System.Windows.Forms.Timer timer = new Timer();
        WMPLib.WindowsMediaPlayer wplayer;

        public Form1()
        {
            InitializeComponent();
            foldertoshow = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";
            labelFolderToShow.Text = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderdialog = new FolderBrowserDialog();
            folderdialog.Description = "Select folder to show";
            if (folderdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foldertoshow = folderdialog.SelectedPath;
                labelFolderToShow.Text = folderdialog.SelectedPath;
            }
            else
            {
                foldertoshow = null;
                labelFolderToShow.Text = "";
            }
            folderdialog.Dispose();
            folderdialog = null;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (foldertoshow != null)
            {
                pictureBox.Dock = DockStyle.Fill;
                files = getFiles(foldertoshow, "*.gif|*.jpg|*.jpeg|*.png|*.bmp", System.IO.SearchOption.TopDirectoryOnly);
                

                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
                //this.TopMost = true;
                this.Bounds = System.Windows.Forms.Screen.GetBounds(this);

                timer.Interval = Convert.ToInt32(numericUpDown1.Value) * 1000;
                timer.Tick += timer_Tick;
                timer.Start();
                try
                {
                    if (checkBox1.Checked)
                    {
                        music = getFiles(foldertoshow, "*.mp3", System.IO.SearchOption.TopDirectoryOnly);
                        if (music.Length > 0)
                        {
                            //play music
                            wplayer = new WMPLib.WindowsMediaPlayer();
                            wplayer.URL = music[0];
                            wplayer.settings.setMode("Loop", true);
                            wplayer.controls.play();
                        }
                    }
                }
                catch
                {

                }
            }
        }

        public string[] getFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
        {
            // ArrayList will hold all file names
            System.Collections.ArrayList alFiles = new System.Collections.ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(System.IO.Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            try
            {
                pictureBox.Image = System.Drawing.Image.FromFile(files[currentfile]);
                currentfile++;
            }
            catch
            {
                currentfile = 0;
                if (!checkBox2.Checked)
                    this.Close();
            }
            timer.Enabled = true;
        }
    }
}
