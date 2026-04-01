using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace bmi計算機
{
    public partial class fmBMI : Form
    {
        private Point originalLocation;
        private int threshold = 120; // 游標感應距離
        private int speed = 8;        // 每幀移動的速度（數值越小越平滑）
        private Timer gameTimer;
        private bool btnstatus = false;
        // 原本按鈕逃跑的 Timer
        private Timer gameTimer2;

        // 🌟 新增：專屬背景動畫的 Timer
        private Timer bgTimer;

        // 用來控制背景顏色的 RGB 變數
        private int r = 255, g = 0, b = 0;
        public fmBMI()
        {
            InitializeComponent();
            originalLocation = btnRun.Location;
            gameTimer = new Timer();
            gameTimer.Interval = 15; // 約 60 FPS
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // 初始化背景動畫 Timer
            bgTimer = new Timer();
            bgTimer.Interval = 5; // 每 50 毫秒更新一次顏色
            bgTimer.Tick += BgTimer_Tick;
            bgTimer.Start();
        }
        // 背景動畫的觸發事件
        private void BgTimer_Tick(object sender, EventArgs e)
        {
            // 簡單的 RGB 漸變邏輯 (紅 -> 綠 -> 藍 -> 紅)
            if (r > 0 && b == 0) { r -= 5; g += 5; }
            if (g > 0 && r == 0) { g -= 5; b += 5; }
            if (b > 0 && g == 0) { b -= 5; r += 5; }

            // 確保數值在 0~255 的安全範圍內
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            // 更新視窗的背景顏色
            this.BackColor = Color.FromArgb(r, g, b);

            int lb2x = label2.Location.X;
            int lb2y = label2.Location.Y;
            lb2x+=5;
            if (lb2x > this.ClientSize.Width)
                lb2x = 0;
            label2.Location= new Point(lb2x, lb2y);
            int lb3x = label3.Location.X;
            int lb3y = label3.Location.Y;
            lb3x += 5;
            if (lb3x > this.ClientSize.Width)
                lb3x = 0;
            label3.Location = new Point(lb3x, lb3y);


        }

        private void label3_Click(object sender, EventArgs e)
        {
            //do nothing
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            bool isHeightValid = double.TryParse(txtHeight.Text, out double height);
            bool isWeightValid = double.TryParse(txtWeight.Text, out double weight);
            string[] strResultList = { "體重過輕", "健康體位", "體位過重", "輕度肥胖", "中度肥胖", "重度肥胖" };
            Color[] colorList = { Color.Blue, Color.Green, Color.Orange, Color.DarkOrange, Color.Red, Color.Purple };


            if (isHeightValid && isWeightValid)
            {
                height /= 100;

                double bmi = weight / (height * height);

                lblResult.Text = $"{bmi:F2}";
                string strResult = "";
                Color colorResult = Color.Black;
                int resultIndex = 0;
                if (bmi < 18.5)
                {
                    resultIndex = 0;
                }
                else if (bmi < 24)
                {
                    resultIndex = 1;
                }
                else if (bmi < 27)
                {
                    resultIndex = 2;
                }
                else if (bmi < 30)
                {
                    resultIndex = 3;
                }
                else if (bmi < 35)
                {
                    resultIndex = 4;
                }
                else
                {
                    resultIndex = 5;
                }
                strResult = strResultList[resultIndex];
                colorResult = colorList[resultIndex];

                lblResult.Text = $"{bmi:F2} ({strResult})";
                lblResult.BackColor = colorResult;

            }
            else
            {
                MessageBox.Show("請輸入有效的數字。", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fmBMI_Load(object sender, EventArgs e)
        {

        }

        private void fmBMI_MouseMove(object sender, MouseEventArgs e)
        {
            

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void lblResult_MouseHover(object sender, EventArgs e)
        {

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (txtHeight.Text == "" || txtWeight.Text == "") 
                btnstatus = false;
            else
                btnstatus = true;

            if (btnstatus == true)
            {
                btnRun.Location = originalLocation;
                return;
            }

            // 3. 取得滑鼠在 Form 上的相對位置
            Point mousePos = this.PointToClient(Cursor.Position);

            // 4. 計算按鈕中心點
            Point btnCenter = new Point(
                btnRun.Location.X + btnRun.Width / 2,
                btnRun.Location.Y + btnRun.Height / 2
            );

            // 5. 計算距離
            double distance = Math.Sqrt(Math.Pow(mousePos.X - btnCenter.X, 2) + Math.Pow(mousePos.Y - btnCenter.Y, 2));

            // 6. 如果滑鼠靠近，計算逃跑路徑
            if (distance < threshold)
            {
                int nextX = btnRun.Location.X;
                int nextY = btnRun.Location.Y;

                // 往滑鼠相反的方向平滑移動
                if (mousePos.X < btnCenter.X) nextX += speed;
                else nextX -= speed;

                if (mousePos.Y < btnCenter.Y) nextY += speed;
                else nextY -= speed;

                // 7. 邊界判斷：如果超過邊界，就重置回原位
                if (nextX < 0 || nextY < 0 ||
                    nextX + btnRun.Width > this.ClientSize.Width ||
                    nextY + btnRun.Height > this.ClientSize.Height)
                {
                    btnRun.Location = originalLocation;
                }
                else
                {
                    // 移動到新位置
                    btnRun.Location = new Point(nextX, nextY);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}