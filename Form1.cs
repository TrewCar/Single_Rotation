using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private int[,] Screen;

    private Graphics g;

    private int SZ = 5;

    private int RectX;
    private int RectY;

    private int Shift = 0;
    private bool rotate = true;
    public Form1()
    {
        InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        RectX = pictureBox1.Width / SZ;
        RectY = pictureBox1.Height / SZ;

        LoadPicture();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        for (int x = Shift; x < Screen.GetLength(0) - Shift; x += 2)
        {
            for (int y = Shift; y < Screen.GetLength(1) - Shift; y += 2)
            {
                int sum = Screen[x, y] + Screen[x + 1, y] + Screen[x, y + 1] + Screen[x + 1, y + 1];
                if (sum != 1)
                {
                    continue;
                }
               
                if(rotate) //Поворот по часовой стрелке
                {
                    (Screen[x, y], Screen[x + 1, y], Screen[x + 1, y + 1], Screen[x, y + 1]) = 
                        (Screen[x + 1, y], Screen[x + 1, y + 1], Screen[x, y + 1], Screen[x, y]);
                }
                else       //Поворот против часовой стрелки
                {
                    (Screen[x, y], Screen[x, y + 1], Screen[x + 1, y + 1], Screen[x + 1, y]) = 
                        (Screen[x, y + 1], Screen[x + 1, y + 1], Screen[x + 1, y], Screen[x, y]);
                    
                }
            }
        }

        for (int x = 0; x < Screen.GetLength(0); x++)
        {
            for (int y = 0; y < Screen.GetLength(1); y++)
            {
                g.FillRectangle(new SolidBrush(Screen[x, y] == 1 ? Color.Yellow : Color.Gray), x * SZ, y * SZ, SZ, SZ);
            }
        }

        pictureBox1.Invalidate();
        Shift = -Shift + 1;
    }
    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.X < 0 || e.Y < 0 || e.X > pictureBox1.Width || e.Y > pictureBox1.Height)
            return;

        if (e.Button == MouseButtons.Left)
        {

            int x = e.X / SZ;
            int y = e.Y / SZ;

            Screen[x, y] = 1;

            g.FillRectangle(new SolidBrush(Color.Yellow), x * SZ, y * SZ, SZ, SZ);
            pictureBox1.Invalidate();
        }
        if(e.Button == MouseButtons.Right)
        {
            int x = e.X / SZ;
            int y = e.Y / SZ;

            Screen[x, y] = 0;

            g.FillRectangle(new SolidBrush(Color.Gray), x * SZ, y * SZ, SZ, SZ);
            pictureBox1.Invalidate();
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Timer.Start();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        Timer.Stop();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        if (!Timer.Enabled)
        {
            Shift = -Shift + 1;
            rotate = !rotate;
        }
        else
        {
            Timer.Stop();
            Shift = -Shift + 1;
            rotate = !rotate;
            Timer.Start();
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        try
        { 
            Timer.Interval = int.Parse(textBox1.Text); 
        }
        catch { }
    }

    private void button4_Click(object sender, EventArgs e)
    {
        Timer.Stop();
        LoadPicture();
    }

    private void LoadPicture()
    {
        Screen = new int[RectX, RectY];

        pictureBox1.Image = new Bitmap(Width, Height);

        g = Graphics.FromImage(pictureBox1.Image);
    }
}

