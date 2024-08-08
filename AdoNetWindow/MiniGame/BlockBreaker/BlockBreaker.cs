using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.MiniGame
{
    public partial class BlockBreaker : Form
    {
        Graphics g;
        int nBlocks = 20;   // 최초 20개
        Rectangle racket = new Rectangle();
        Rectangle[] blocks = new Rectangle[100];  // 블록 rectagle 배열
        bool[] bVisible = new bool[100];
        Rectangle ball;

        Brush racketColor = new SolidBrush(Color.Red);
        Brush blockColor = new SolidBrush(Color.Orange);
        Brush ballColor = new SolidBrush(Color.Gold);
        Pen pen = new Pen(Color.Black);

        int racketY = 480;  // racket의 위치
        int formW = 300;  // Form의 폭
        int racketW = 50;
        int racketH = 10;

        int blockY = 60;    // 블록 위치
        int blockW = 30;
        int blockH = 20;

        int ballW = 10;
        int ballH = 10;

        Timer myTimer = new Timer();
        private double slope;       // 공의 기울기 0.5~2 랜덤     
        private double vDir = 1;         // 수직방향 1=아래, -1=위

        Random r = new Random();
        private int clearedBlocks = 0;
        private int MaxBlocks = 100;

        System.Media.SoundPlayer myPlayer;
        System.Media.SoundPlayer[] blockSound = new System.Media.SoundPlayer[4];
        private int initialBallPosition;    // 최초 볼의 위치(y 좌표)

        int currentSpeed = 1; //속도
        public BlockBreaker()
        {
            InitializeComponent();
            ClientSize = new Size(300, 500);
            this.Text = "Block Breaker v1.0";

            initbVisible();
            initRacket();
            initBlocks();
            StartBall();

            initSound();

            myTimer.Tick += myTimer_Tick;

            myTimer.Interval = 100 * currentSpeed;  // milliseconds
            myTimer.Start();
        }

        private void initSound()
        {
            myPlayer = new System.Media.SoundPlayer();
            myPlayer.SoundLocation = "../../sounds/ir_end.wav";

            for (int i = 0; i < 4; i++)
            {
                blockSound[i] = new System.Media.SoundPlayer();
            }
            blockSound[0].SoundLocation = "../../sounds/Windows Battery Critical.wav";
            blockSound[1].SoundLocation = "../../sounds/Windows Battery Low.wav";
            blockSound[2].SoundLocation = "../../sounds/Windows Notify.wav";
            blockSound[3].SoundLocation = "../../sounds/Windows User Account Control.wav";
        }
        private void initbVisible()
        {
            for (int i = 0; i < nBlocks; i++)
                bVisible[i] = true;
        }
        private void initRacket()
        {
            racket.X = formW / 2 - racketW / 2;  // 중앙
            racket.Y = racketY;
            racket.Width = racketW;
            racket.Height = racketH;
        }

        private void initBall()
        {
            ball = new Rectangle();
            ball.X = formW / 2 - ballW / 2;    // 중앙
            ball.Y = blockY + nBlocks / 10 * blockH;
            initialBallPosition = ball.Y;
            ball.Width = ballW;
            ball.Height = ballH;
        }
        private void drawBall()
        {
            g.FillRectangle(ballColor, ball);
            g.DrawRectangle(pen, ball);
        }

        private void drawRacket()
        {
            g.FillRectangle(racketColor, racket);
        }
        private void initBlocks()
        {
            for (int i = 0; i < nBlocks; i++)
                blocks[i] = new Rectangle(i % 10 * blockW, blockY + blockH * (i / 10),
                     blockW - 1, blockH - 1);
        }

        private void drawBlocks()
        {
            for (int i = 0; i < nBlocks; i++)
                if (bVisible[i])
                    g.FillRectangle(blockColor, blocks[i]);
        }

        private void BlockBreaker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        {
                            int speed;
                            if (!int.TryParse(lbSpeed.Text, out speed))
                                speed = 1;
                            speed++;

                            if (speed > 5)
                                speed = 5;
                            lbSpeed.Text = speed.ToString();

                            currentSpeed = speed;
                            myTimer.Stop();
                            myTimer.Interval = 100 / currentSpeed;  // milliseconds
                            myTimer.Start();

                        }
                        break;
                    case Keys.Down:
                        {
                            int speed;
                            if (!int.TryParse(lbSpeed.Text, out speed))
                                speed = 1;
                            speed--;

                            if (speed < 1)
                                speed = 1;
                            lbSpeed.Text = speed.ToString();

                            currentSpeed = speed;
                            myTimer.Stop();
                            myTimer.Interval = 100 * currentSpeed;  // milliseconds
                            myTimer.Start();
                        }
                        break;
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                racket.X -= 10;
                if (racket.X < 0)
                    racket.X = 0;
            }
            else if (e.KeyCode == Keys.Right)
            {
                racket.X += 10;
                if (racket.X > formW - racketW)
                    racket.X = formW - racketW;
            }
            else if (e.KeyCode == Keys.Up)
            {
                racket.Y -= 10;
                if (racket.Y < initialBallPosition)     // 최초 볼 높이보다 더 높아지지는 않게
                    racket.Y = initialBallPosition;
            }
            else if (e.KeyCode == Keys.Down)
            {
                racket.Y += 10;
                if (racket.Y > ClientSize.Height - 20)
                    racket.Y = ClientSize.Height - 20;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                myTimer.Stop();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                myTimer.Start();
            }
            
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;     // 또는 this.CreateGraphics();
            drawRacket();
            drawBlocks();
            drawBall();
        }
        private void StartBall()
        {
            initBall();
            slope = r.Next(5, 20) / 10.0;
            if (r.Next(2) % 2 == 1)
                slope = -slope;
        }
        void myTimer_Tick(object sender, EventArgs e)
        {
            double dX;
            double unit = ballW;

            dX = unit / slope;
            ball.X += (int)dX;  // 속도 조절을 위해 slope로 나누어 줌, +- 구분할 필요 없음
            ball.Y += (int)(vDir * slope * dX);

            // ball이 좌우 벽에 충돌하였는지 체크
            if (ball.X < 0 || ball.X > formW - ballW)
            {
                //myPlayer.Play();
                slope = -slope;
            }
            // ball이 천장이나 라켓에 맞았는지 체크
            if (ball.Y < 0 || racket.IntersectsWith(ball))
            {
                //myPlayer.Play();
                vDir = -vDir;
            }

            Invalidate();

            // ball이 화면 아래보다 더 내려갔다면, 라켓으로 받아내지 못한 것이므로 죽은 것
            if (ball.Y > ClientSize.Height)
            {
                StartBall();
                return;
            }

            // 블록의 맨 아래 위치보다 볼이 더 아래에 있다면 블록과의 intersect를 비교하지 않는다
            if (ball.Y > blockY + blockH * nBlocks / 10)
                return;

            Random sRnd = new Random();
            for (int i = 0; i < nBlocks; i++)
            {
                if (bVisible[i] && ball.IntersectsWith(blocks[i]))
                {
                    bVisible[i] = false;
                    vDir = -vDir;

                    int index = sRnd.Next(0, 3);
                    //blockSound[index].Play();
                    if (++clearedBlocks >= nBlocks)
                    {
                        myTimer.Stop();

                        if (nBlocks == MaxBlocks)
                        {
                            MessageBox.Show(this,"Final Stage Cleared!", "Mission Completed!!", MessageBoxButtons.OK);
                            this.Close();
                        }
                        else
                        {
                            string str = string.Format("Level {0} Completed! Play More?", nBlocks / 20);
                            DialogResult result = MessageBox.Show(this,str, "Level Clear!", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                                newLevelStart();
                            else
                                Close();
                        }
                    }
                }
            }
        }
        private void newLevelStart()
        {
            if (nBlocks > MaxBlocks)
                return; // nBlocks = MaxBlocks;

            nBlocks += 20;  // 20개 씩 증가

            initbVisible();
            initRacket();
            initBall();
            initBlocks();

            clearedBlocks = 0;

            StartBall();
            myTimer.Start();
        }
        
    }
}
