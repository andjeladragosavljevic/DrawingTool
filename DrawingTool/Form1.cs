using System.Drawing.Printing;
namespace DrawingTool
{
    public partial class Form1 : Form
    {
        Pen pen = new Pen(Color.Black, 3);
        Bitmap bitmap;
        Graphics g, graph;
        Point old = new Point();
        Point curr = new Point();
        bool mouseDown = false;
        bool ellipse = false;
        bool rectangle = false;
        bool fill = false;
        bool gradient = false;
        bool opacity = false;
        bool drawn = false;
        Color colorBefore = Color.Black; // Boja olovke prije izbora gumice
        //Boje za gradient
        Color one = Color.AntiqueWhite;
        Color two = Color.DarkMagenta;
        float sizeBefore = 3; // Debljina olovke prije izbora gumice
        int opacityPercentage = 100;
        Rectangle rect = new Rectangle();
        public Form1()
        {

            InitializeComponent();

            toolStrip2.AutoSize = false; toolStrip1.Height = 100;
            toolStrip2.ImageScalingSize = new Size(50, 50);

            g = panel1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            bitmap = new Bitmap(this.Width, this.Height);
            graph = Graphics.FromImage(bitmap);
            panel1.BackgroundImage = bitmap;

            pen.EndCap = pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;

        }



        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            curr = e.Location;
            if (mouseDown)
            {
                curr = e.Location;
                g.DrawLine(pen, old, curr);
                graph.DrawLine(pen, old, curr);
                old = curr;

            }
            lbPosition.Text = $"Position: ({e.Location.X}, {e.Location.Y})";

        }


        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            pen.Width = 3;
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pen.Width = 5;
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            pen.Width = 10;
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            pen.Width = 15;
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            pen.Width = 20;
        }



        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pen.Color = colorDialog.Color;
                toolStripButton2.BackColor = colorDialog.Color;
            }

        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ellipse = true;

        }
        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rectangle = true;
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            DoDragDrop(pen, DragDropEffects.Move);
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fill = true;
        }
        private void fillToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fill = true;
        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }
        private void dashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }
        private void dotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }
        private void dashToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }
        private void dashDotDotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (drawn)
                {
                    var res = MessageBox.Show("Oops your drawings will be deleted!", "Paper change warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (res == DialogResult.OK)
                    {
                        paperColor(colorDialog.Color);
                    }
                }
                else
                {
                    paperColor(colorDialog.Color);
                }

            }
        }
        public void paperColor(Color c)
        {
            SolidBrush temp = new SolidBrush(c);

            g.FillRectangle(temp, new Rectangle(panel1.Location, panel1.Size));
            graph.FillRectangle(temp, new Rectangle(panel1.Location, panel1.Size));
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gradient = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            panel1.BackColor = Color.White;
            g.Clear(panel1.BackColor);
            graph.Clear(panel1.BackColor);
            bitmap = new Bitmap(this.Width, this.Height);
            graph = Graphics.FromImage(bitmap);
            panel1.BackgroundImage = bitmap;

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            colorBefore = pen.Color;
            sizeBefore = pen.Width;
            pen = new Pen(Color.White, 10);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = " Image (*.jpg; *.jpeg; *.png; *.bmp)| *.jpg; *.jpeg; *.png; *.bmp";
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                switch (saveFileDialog.Filter)
                {
                    case ".jpg":
                        format = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case ".jpeg":
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ".png":
                        format = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                }
                bitmap.Save(saveFileDialog.FileName, format);

            }

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            PrintDialog printDialog = new PrintDialog();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printDialog.Document = pd;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }

        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void toolStripDropDownButton4_Click_1(object sender, EventArgs e)
        {
            fill = false;
            gradient = false;
            gradient = true;
        }
        private void colorOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                one = color.Color;
            }

        }
        private void colorTwoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                two = color.Color;
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            gradient = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files(*.jpg; *.jpeg; *png; *.bmp)| *.jpg; *.jpeg; *.png; *.bmp ";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bitmap = new Bitmap(openFileDialog.FileName);
                graph = Graphics.FromImage(bitmap);
                panel1.BackgroundImage = bitmap;

            }
        }

        private void toolStripDropDownButton5_Click(object sender, EventArgs e)
        {
            fill = false;
            gradient = false;
            opacity = true;
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            opacityPercentage = 100;
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            opacityPercentage = 50;
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            opacityPercentage = 25;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            opacity = false;
            gradient = false;
            fill = true;

        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            fill = false;

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            pen.Color = colorBefore;
            pen.Width = sizeBefore;
            pen.EndCap = pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void toolStripSeparator4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
            // TO - DO
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {

            //rect.Contains((MouseEventArgs)e.X);
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (rect.Contains(e.Location))
            {
                contextMenuStrip1.Show();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {

            mouseDown = true;
            old = e.Location;
            drawn = true;
            rect = new Rectangle(e.X, e.Y, 100, 100);


            if (ellipse)
            {
                if (fill)
                {
                    Brush brush = new SolidBrush(pen.Color);
                    g.FillEllipse(brush, rect);
                    graph.FillEllipse(brush, rect);


                }
                else if (gradient)
                {

                    System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(79, 79), one, two);

                    g.FillEllipse(brush, rect);
                    graph.FillEllipse(brush, rect);
                }
                else if (opacity)
                {
                    SolidBrush brush;

                    if (opacityPercentage == 100)
                    {
                        brush = new SolidBrush(Color.FromArgb(255, pen.Color.R, pen.Color.G, pen.Color.G));
                    }
                    else if (opacityPercentage == 50)
                    {
                        brush = new SolidBrush(Color.FromArgb(128, pen.Color.R, pen.Color.G, pen.Color.G));

                    }
                    else
                    {
                        brush = new SolidBrush(Color.FromArgb(64, pen.Color.R, pen.Color.G, pen.Color.G));
                    }
                    opacity = false;
                    g.FillEllipse(brush, rect);
                    graph.FillEllipse(brush, rect);

                }
                else
                {

                    g.DrawEllipse(pen, rect);
                    graph.DrawEllipse(pen, rect);

                }
                ellipse = false;

            }
            else if (rectangle)
            {
                if (fill)
                {
                    SolidBrush brush = new SolidBrush(pen.Color);

                    g.FillRectangle(brush, rect);
                    graph.FillRectangle(brush, rect);

                }
                else if (gradient)
                {

                    System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 40), new Point(79, 40), one, two);
                    g.FillRectangle(brush, rect);
                    graph.FillRectangle(brush, rect);
                }
                else if (opacity)
                {
                    SolidBrush brush;

                    if (opacityPercentage == 100)
                    {
                        brush = new SolidBrush(Color.FromArgb(255, pen.Color.R, pen.Color.G, pen.Color.G));
                    }
                    else if (opacityPercentage == 50)
                    {
                        brush = new SolidBrush(Color.FromArgb(128, pen.Color.R, pen.Color.G, pen.Color.G));

                    }
                    else
                    {
                        brush = new SolidBrush(Color.FromArgb(64, pen.Color.R, pen.Color.G, pen.Color.G));
                    }
                    opacity = false;
                    g.FillRectangle(brush, rect);
                    graph.FillRectangle(brush, rect);

                }
                else
                {
                    g.DrawRectangle(pen, rect);
                    graph.DrawRectangle(pen, rect);

                }
                rectangle = false;


            }


        }
    }
}