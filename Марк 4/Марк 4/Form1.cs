using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAudio;
using WindowsInput;
using WindowsInput.Native;
using System.Diagnostics;

namespace Марк_4
{
    public partial class Form1 : Form
    {
        InputSimulator sim = new InputSimulator();
        private AudioPlayer Player;
        string str = "";
        List<string> arr = new List<string>();


        public Form1()
        {
            InitializeComponent();
            Player = new AudioPlayer();
            Player.PlayingStatusChanged += (s, e) => button1.Text = e == Status.Playing ? "Pause" : "Play";
            Player.AudioSelected += (s, e) =>
            {
                trackBar1.Maximum = (int)e.Duration;
                label1.Text = e.Name;
                label3.Text = e.DurationTime.ToString(@"mm\:ss");
                listBox1.SelectedItem = e.Name;
            };
            Player.ProgressChanged += (s, e) =>
            {
                trackBar1.Value = (int)e; // ошибка при воспроизведении midi
                label2.Text = ((AudioPlayer)s).PositionTime.ToString(@"mm\:ss");
            };
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Location = new Point(flowLayoutPanel1.Location.X + flowLayoutPanel1.Width / 2, flowLayoutPanel1.Location.Y + flowLayoutPanel1.Height / 2);
            flowLayoutPanel1.BackColor = Color.Transparent;
            label1.Text = "";
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;

            label2.Text = "";
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.White;

            label3.Text = "";
            label3.BackColor = Color.Transparent;
            label3.ForeColor = Color.White;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Play")
                Player.Play();
            else if (((Button)sender).Text == "Pause")
                Player.Pause();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Audio Files|*.wav;*mid;"
            }
            )
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < dialog.FileNames.Count(); i++)
                    {
                            arr.Add(dialog.FileNames[i]);
                    }

                    str = dialog.FileName;
                    Player.LoadAudio(dialog.FileNames);
                    listBox1.Items.Clear();

                    //listBox1.Items.AddRange(Player.Playlist);

                    for (int i = 0; i < arr.Count(); i++)
                    {
                        //if (listBox1.FindString(arr[i]) == -1)
                            listBox1.Items.Add(arr[i]);
                    }
                    //arr.Clear();
                }
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedItem == null)
                return;
            Player.SelectAudio(((ListBox)sender).SelectedIndex);
            str = listBox1.SelectedItem.ToString();
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            if (str != "")
            {
                if (str[str.Length - 1] != 'v' || str[str.Length - 2] != 'a' || str[str.Length - 3] != 'w')
                {
                    MessageBox.Show("Вы пытаетесь перевести перевести " + str[str.Length - 3] + str[str.Length - 2] + str[str.Length - 1] + " файл. Выберите wav файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Process.Start("C:/Windows/system32/cmd.exe");
                    sim.Keyboard.Sleep(1000);
                    sim.Keyboard.TextEntry("cd C:/Users/Hunte/PycharmProjects/pythonProject8");
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    sim.Keyboard.TextEntry("python main.py");
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    sim.Keyboard.TextEntry(str);
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    sim.Keyboard.TextEntry("exit");
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                }
            }
        }


        private void trackBar1_Scroll(object sender, EventArgs e) => Player.Position = ((TrackBar)sender).Value;

        private void trackBar2_Scroll(object sender, EventArgs e) => Player.Volume = ((TrackBar)sender).Value;
    }
}
