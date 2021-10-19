using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Class2_Startup
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[10, 10];
        //Scratchpad to "write" changes to
        bool[,] scratchPad = new bool[10, 10];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Coral;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            //scratchPad = universe;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x, y] == true && CountNeighborsFinite((int)x, (int)y) < 2)
                    {
                        //Any living cell in the current universe with less than 2 living
                        //neighbors dies in the next generation as if by under-population.
                        //If a cell meets this criteria in the universe array then make
                        //the same cell dead in the scratch pad array.
                        scratchPad[x, y] = false;
                    }
                    else if (universe[x, y] == true && CountNeighborsFinite((int)x, (int)y) > 3)
                    {
                        //Any living cell with more than 3 living neighbors will die in
                        //the next generation as if by over - population.If so in the
                        //universe then kill it in the scratch pad.
                        scratchPad[x, y] = false;
                    }else if (universe[x, y] == true && CountNeighborsFinite((int)x, (int)y) == 3)
                    {
                        //Any living cell with 2 or 3 living neighbors will live on into
                        //the next generation.If this is the case in the universe then
                        //the same cell lives in the scratch pad.
                        scratchPad[x, y] = true;
                    }
                    else if (universe[x, y] == true && CountNeighborsFinite((int)x, (int)y) == 2)
                    {
                        //Any living cell with 2 or 3 living neighbors will live on into
                        //the next generation.If this is the case in the universe then
                        //the same cell lives in the scratch pad.
                        scratchPad[x, y] = true;
                    }
                    else if(universe[x, y] == false && CountNeighborsFinite((int)x, (int)y) == 3)
                    {
                        //Any dead cell with exactly 3 living neighbors will be born into
                        //the next generation as if by reproduction. If so in the universe
                        //then make that cell alive in the scratch pad.
                        scratchPad[x, y] = true;
                    }
                }
            }

            // Swap Arrays Code
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count, Update status strip generations
            generations++;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void Randomize()
        {
            // Random rand = new Random(); Time 
            // Takes a seed for seed in constructor
            Random rand = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                { 
                    // Call Next with (0, 2) 
                    //if random number == 0 then turn the call on, otherwise turn it off
                    //invalidate when its done
                }
            }
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            //this stringformat object is sent through the drawstring method that centers neighboors text
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);
            Brush neighboorBrush = new SolidBrush(gridColor);
            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 2);
            Font neighboorFont = new Font(this.Font, FontStyle.Bold);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;
                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                        e.Graphics.DrawString(CountNeighborsFinite((int)x, (int)y).ToString(), neighboorFont, neighboorBrush, cellRect, stringFormat);

                    }
                    else if (universe[x, y] == false && CountNeighborsFinite((int)x, (int)y) != 0)
                    {
                        e.Graphics.DrawString(CountNeighborsFinite((int)x, (int)y).ToString(), neighboorFont, neighboorBrush, cellRect, stringFormat);
                    }
                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            neighboorBrush.Dispose();
            neighboorFont.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];

                // Tell Windows you need to repaint

            }
            graphicsPanel1.Invalidate();
        }

        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            //Commenting this out caused the generations text to show correctly
            //graphicsPanel1.Invalidate();
            return count;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x, y] == true)
                    {
                        universe[x, y] = false;
                    }
                    if (scratchPad[x, y] == true)
                    {
                        scratchPad[x, y] = false;
                    }
                }
            }
            generations = 0;
            graphicsPanel1.Invalidate();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    if(row.StartsWith("!") == true)
                    {
                        // If the row begins with '!' then it is a comment
                        // and should be ignored.
                        continue;
                    }else if(row.StartsWith("!") == false)
                    {
                        // If the row is not a comment then it is a row of cells.
                        // Increment the maxHeight variable for each row read.
                        maxHeight++;
                    }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad, need to call universe = new bool[maxwidth, maxheight]
                // to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                scratchPad = new bool[maxWidth, maxHeight];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                int yPos = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    if(row.StartsWith("!") == true)
                    {
                        // If the row begins with '!' then
                        // it is a comment and should be ignored.
                        continue;
                    }else if(row.StartsWith("!") == false)
                    {
                        // If the row is not a comment then 
                        // it is a row of cells and needs to be iterated through.
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            if(row[xPos] == 'O')
                            {
                                // If row[xPos] is a 'O' (capital O) then
                                // set the corresponding cell in the universe to alive.
                                universe[xPos, yPos] = true;
                            }else if(row[xPos] == '.')
                            {
                                // If row[xPos] is a '.' (period) then
                                // set the corresponding cell in the universe to dead.
                                universe[xPos, yPos] = false;
                            }
                        }
                    }
                    //increment yPos
                    yPos++;
                }
                graphicsPanel1.Invalidate();

                // Close the file.
                reader.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                DateTime time = DateTime.Now;
                writer.WriteLine("!" + time.ToString());

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        if(universe[x,y] == true)
                        {
                            // If the universe[x,y] is alive then append 'O' (capital O)
                            // to the row string.
                            currentRow += "O";
                        }else if(universe[x,y] == false)
                        {
                            // Else if the universe[x,y] is dead then append '.' (period)
                            // to the row string.
                            currentRow += ".";
                        }
                    }
                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void Run_Click(object sender, EventArgs e)
        {
            timer.Enabled = true; // start timer running
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false; // start timer running
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
