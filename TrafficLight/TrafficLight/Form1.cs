using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace TrafficLight
{
    public partial class Form1 : Form
    {
        private SerialPort _serialPort;
        byte flag = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _serialPort.Open();
        }

        byte colorLed2 = 0;
        byte colorLed1 = 0;

        private void DisplayData(string data)
        {
            try
            {
                // Tách dữ liệu dựa trên dấu phẩy
                string[] values = data.Split(',');

                if (values.Length == 2)
                {
                    // Gán giá trị cho từng TextBox
                    textBox1.Text = values[0];
                    textBox2.Text = values[1];
                    textBox3.Text = values[0];
                    textBox4.Text = values[1];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            // Nhận dữ liệu từ Arduino
            string data = _serialPort.ReadLine();

            // Phân tích và hiển thị dữ liệu trên hai TextBox
            Invoke(new Action(() => DisplayData(data)));
            Invoke(new Action(() => ProcessSerialData(data)));
            Invoke(new Action(() => ProcessSerialDataTime(data)));

        }

        private void ProcessSerialData(string data)
        {
            string[] parts = data.Split('-');
            // Example data: "Green Light: LED 1, Red Light: LED 2"
            foreach (string part in parts)
            {
                if (part.Contains("LED 1"))
                {
                    string[] colorName = part.Split(':');

                    name_led1_1.Text = colorName[0].Trim();
                    name_led1_2.Text = colorName[0].Trim();
                }

                if (part.Contains("LED 2"))
                {
                    string[] colorName = part.Split(':');

                    name_led2_1.Text = colorName[0].Trim();
                    name_led2_2.Text = colorName[0].Trim();
                }
            }
        }
        private void ProcessSerialDataTime(string data)
        {
            string[] parts = data.Split(';');
            foreach (string part in parts)
            {
                if (part.Contains("Hour"))
                {
                    string[] time = part.Split(':');

                    currentHour.Text = time[1].Trim();
                }

                if (part.Contains("Minute"))
                {
                    string[] time = part.Split(':');

                    currentMinute.Text = time[1].Trim();
                }
            }
        }
        private void btn_stop_light_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop mode
                byte mode = 1;
                byte[] data = new byte[] { mode, colorLed1, colorLed2 };
                _serialPort.Write(data, 0, data.Length);

                selectedMode_0.Checked = false;
                selectedMode_1.Checked = true;
                selectedMode_2.Checked = false;

                MessageBox.Show("Timing values and colors sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btn_night_mode_Click(object sender, EventArgs e)
        {
            try
            {
                byte mode = 2; // Chế độ Night mode

                byte targetHour = byte.Parse(inputHour.Text);
                byte targetMinute = byte.Parse(inputMinute.Text);
                byte targetHourStop = byte.Parse(inputHourStop.Text);
                byte targetMinuteStop = byte.Parse(inputMinuteStop.Text);

                byte[] data = new byte[] { mode, targetHour, targetMinute, targetHourStop, targetMinuteStop };

                // Gửi dữ liệu qua cổng nối tiếp
                _serialPort.Write(data, 0, data.Length);

                selectedMode_0.Checked = false;
                selectedMode_1.Checked = false;
                selectedMode_2.Checked = true;

                MessageBox.Show("Night mode with yellow lights sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_green_light_Click(object sender, EventArgs e)
        {
            try
            {
                byte tDen1 = byte.Parse(txtTden1.Text);
                byte tDen2 = byte.Parse(txtTden2.Text);
                byte mode = 0;

                byte[] data = new byte[] { mode, flag, tDen1, tDen2 };
                _serialPort.Write(data, 0, data.Length);

                selectedMode_0.Checked = true;
                selectedMode_1.Checked = false;
                selectedMode_2.Checked = false;

                MessageBox.Show("Timing values sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void green_light_led1_CheckedChanged(object sender, EventArgs e)
        {
            colorLed1 = 0;
        }

        private void yellow_light_led1_CheckedChanged(object sender, EventArgs e)
        {
            colorLed1 = 1;
        }

        private void red_light_led1_CheckedChanged(object sender, EventArgs e)
        {
            colorLed1 = 2;
        }

        private void green_light_led2_CheckedChanged(object sender, EventArgs e)
        {
            colorLed2 = 0;
        }

        private void yellow_light_led2_CheckedChanged(object sender, EventArgs e)
        {
            colorLed2 = 1;
        }

        private void red_light_led2_CheckedChanged(object sender, EventArgs e)
        {
            colorLed2 = 2;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            flag = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            flag = 1;
        }

        private void reset_Button_Click(object sender, EventArgs e)
        {
            try
            {
                byte tDen1 = 14;
                byte tDen2 = 22;
                byte mode = 0;
                byte flag = 0;

                byte[] data = new byte[] { mode, flag, tDen1, tDen2 };
                _serialPort.Write(data, 0, data.Length);

                MessageBox.Show("Timing values sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
