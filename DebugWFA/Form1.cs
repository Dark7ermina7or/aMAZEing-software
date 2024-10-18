using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DungeonGenerator;

namespace DebugWFA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        static NumericUpDown[] dataInputsNumericUpDown;
        static Label[] dataInputLabel;
        static TextBox displayResult;
        static Button StartDebugButton;


        private void Form1_Load(object sender, EventArgs e)
        {
            FormSettings();
            UI();
        }

        void UI()
        {
            dataInputsNumericUpDown = new NumericUpDown[5];
            dataInputLabel = new Label[dataInputsNumericUpDown.Length];
            displayResult = new TextBox()
            {
                Parent = this,
                Size = new Size(650, 650),
                Location = new Point(525, 25),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 13f),
                ForeColor = Color.Black,
            };

            StartDebugButton = new Button()
            {
                Parent = this,
                Size = new Size(500, 30),
                Location = new Point(10, 660),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Text = "Generate"
            };
            StartDebugButton.Click += StartDebugButton_Click;

            for (int i = 0; i < dataInputsNumericUpDown.Length; i++)
            {
                dataInputsNumericUpDown[i] = new NumericUpDown()
                {
                    Parent = this,
                    Size = new Size(130, 30),
                    Location = new Point(10, 10 + 30 * i),
                };

                dataInputLabel[i] = new Label()
                {
                    Parent = this,
                    Size = new Size(400, 30),
                    Location = new Point(150, 12 + 30 * i),
                    Text = "Not implemented.",
                };

            }

            #region inputFields
            dataInputLabel[0].Text = "Seed (0 >> Random)";
            dataInputsNumericUpDown[0].Value = 0; //Zero >> Random Seed
            dataInputsNumericUpDown[0].Minimum = -2147483648;
            dataInputsNumericUpDown[0].Maximum = 2147483647;
            dataInputLabel[0].Visible = false;
            dataInputsNumericUpDown[0].Visible = false;

            dataInputLabel[1].Text = "MapWidth (-2 for borders)";
            dataInputsNumericUpDown[1].Value = 10;
            dataInputsNumericUpDown[1].Minimum = 7;
            dataInputsNumericUpDown[1].Maximum = 52;

            dataInputLabel[2].Text = "MapHeight (-2 for borders)";
            dataInputsNumericUpDown[2].Value = 10;
            dataInputsNumericUpDown[2].Minimum = 7;
            dataInputsNumericUpDown[2].Maximum = 52;

            dataInputLabel[3].Text = "Noise";
            dataInputsNumericUpDown[3].Value = 0;
            dataInputsNumericUpDown[3].Minimum = 0;
            dataInputsNumericUpDown[3].Maximum = 30;

            dataInputLabel[4].Text = "Noise Cluster Size";
            dataInputsNumericUpDown[4].Value = 1;
            dataInputsNumericUpDown[4].Minimum = 1;
            dataInputsNumericUpDown[4].Maximum = 5;

            #endregion

        }

        private void StartDebugButton_Click(object sender, EventArgs e)
        {
            StartDebugButton.Enabled = false;
            StartDebugButton.Text = "Working...";

            Refresh();

            DGMain.SettingsLayout SL = new DGMain.SettingsLayout
            {
                Seed = Convert.ToInt32(dataInputsNumericUpDown[0].Value),
                MapWidth = Convert.ToInt32(dataInputsNumericUpDown[2].Value),
                MapHeight = Convert.ToInt32(dataInputsNumericUpDown[1].Value),
                Noise = Convert.ToInt32(dataInputsNumericUpDown[3].Value),
                NoiseClusterSize = Convert.ToInt32(dataInputsNumericUpDown[4].Value),
            };

            DGMain DungeonMain = new DGMain(SL);

            displayResult.Text = chararraytostring(DungeonMain.TheDungeon);

            StartDebugButton.Enabled = true;
            StartDebugButton.Text = "Generate";

            Refresh();
        }

        static string chararraytostring(char[,] DF)
        {
            string line = "";

            for (int i = 0; i < DF.GetLength(0); i++)
            {
                for (int j = 0; j < DF.GetLength(1); j++)
                {
                    if (DF[i, j] != '\0') { line += DF[i, j]; }
                    else { line += " "; }
                }
                line += "\r\n";
            }

            return line;
        }

        void FormSettings()
        {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Text = "Debug - Dungeon Generator - alfa";
            Font = new Font("Consolas", 13f);
            ClientSize = new Size(1200, 700);
            CenterToScreen();

            BackColor = Color.FromArgb(32, 32, 32);
            ForeColor = Color.Gray;
        }
    }
}
