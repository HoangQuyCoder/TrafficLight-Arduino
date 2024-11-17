using System;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Windows.Forms;
using static TrafficLight.Login;


namespace TrafficLight
{
    public partial class Form1 : Form
    {
        private SerialPort _serialPort;
        byte led = 1;
        byte yellowLedOff = 0;
        byte colorLed2 = 0;
        byte colorLed1 = 0;
        byte dateTimeFlag = 0;

        public Form1(string comPort)
        {
            InitializeComponent();
            _serialPort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            _serialPort.Open();
        }

        int timeOfRreen1 = 0;
        int timeOfRed1 = 0;
        int timeOfYellow1 = 0;
        int timeOfRreen2 = 0;
        int timeOfRed2 = 0;
        int timeOfYellow2 = 0;

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

            foreach (string part in parts)
            {
                if (part.Contains("LED 1"))
                {
                    string[] colorName = part.Split(':');
                    if (colorName.Length > 1)
                    {
                        string currentNameLED1 = colorName[0].Trim();

                        name_led1_1.Text = currentNameLED1;
                        name_led1_2.Text = currentNameLED1;
                    }
                }
                else if (part.Contains("LED 2"))
                {
                    string[] colorName = part.Split(':');
                    if (colorName.Length > 1)
                    {
                        string currentNameLED2 = colorName[0].Trim();

                        name_led2_1.Text = currentNameLED2;
                        name_led2_2.Text = currentNameLED2;
                    }
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
                byte fillIndex = 0; 
                byte[] data = new byte[] { mode, colorLed1, colorLed2, fillIndex, fillIndex, fillIndex };
                _serialPort.Write(data, 0, data.Length);

                selectedMode_0.Checked = false;
                selectedMode_1.Checked = true;
                selectedMode_2.Checked = false;

                indexDatabase("Stop", timeOfRreen1, timeOfYellow1, timeOfRed1, timeOfRreen2, timeOfYellow2, timeOfRed2);

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

                if (!checkDateTime(targetHour, targetMinute, targetHourStop, targetMinuteStop))
                {
                    MessageBox.Show("Error: Input not valid!");
                    return;
                }

                DateTime date1 = dateTimePicker1.Value.Date;
                DateTime date2 = dateTimePicker2.Value.Date;

                byte startDay = (byte)date1.Day;
                byte stopDay = (byte)date2.Day;

                byte[] data = new byte[] { mode, targetHour, targetMinute, targetHourStop, targetMinuteStop, startDay, stopDay };

                // Gửi dữ liệu qua cổng nối tiếp
                _serialPort.Write(data, 0, data.Length);

                selectedMode_0.Checked = false;
                selectedMode_1.Checked = false;
                selectedMode_2.Checked = true;

                indexDatabase("Night", timeOfRreen1, timeOfYellow1, timeOfRed1, timeOfRreen2, timeOfYellow2, timeOfRed2);
                MessageBox.Show("Night mode with yellow lights sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private bool checkDateTime(byte targetHour, byte targetMinute, byte targetHourStop, byte targetMinuteStop)
        {
            DateTime date1 = dateTimePicker1.Value;
            DateTime date2 = dateTimePicker2.Value;
            if (date2.Date > date1.Date)
            {
                if (targetHour >= 0 && targetHour <= 23 && targetMinute >= 0 && targetMinute <= 59)
                {
                    if (targetHourStop >= 0 && targetHourStop <= 23 && targetMinuteStop >= 0 && targetMinuteStop <= 59)
                    {
                        return true;
                    }
                }
            }
            else if (date2.Date == date1.Date)
            {
                if (targetHour >= 0 && targetHour <= 23 && targetMinute >= 0 && targetMinute <= 59)
                {
                    if (targetHourStop >= 0 && targetHourStop <= 23 && targetMinuteStop >= 0 && targetMinuteStop <= 59)
                    {
                        if (targetHourStop >= targetHour)
                        {
                            return true;
                        }
                        else if (targetHour == targetHourStop && targetMinuteStop > targetMinute)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void btn_green_light_Click(object sender, EventArgs e)
        {
            try
            {
                byte tDen1 = byte.Parse(txtTden1.Text);
                byte tDen2 = byte.Parse(txtTden2.Text);
                byte mode = 0;

                if (!checkBox1.Checked)
                {
                    yellowLedOff = 0;
                }
                else
                {
                    yellowLedOff = 1;
                    textBox5.Text = "0";
                }
                byte tDen3 = byte.Parse(textBox5.Text);

                byte[] data = new byte[] { mode, led, yellowLedOff, tDen1, tDen2, tDen3 };
                _serialPort.Write(data, 0, data.Length);

                selectedMode_0.Checked = true;
                selectedMode_1.Checked = false;
                selectedMode_2.Checked = false;

                if (led == 1)
                {
                    indexDatabase("Normal", tDen1, tDen2, tDen3, tDen2, tDen1 + tDen3, tDen1 + tDen3 - tDen1);
                }
                else
                {
                    indexDatabase("Normal", tDen2, tDen1 + tDen3, tDen1 + tDen3 - tDen1, tDen1, tDen2, tDen3);
                }

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
            led = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            led = 2;
        }

        private void reset_Button_Click(object sender, EventArgs e)
        {
            try
            {
                byte tDen1 = 14;
                byte tDen2 = 22;
                byte tDen3 = 3;
                byte mode = 0;
                byte led = 1;
                byte yellowOff = 0;

                byte[] data = new byte[] { mode, led, yellowOff, tDen1, tDen2, tDen3 };
                _serialPort.Write(data, 0, data.Length);

                MessageBox.Show("Timing values sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void indexDatabase(string mode, int timeOfGreen1,int timeOfRed1,  int timeOfYellow1, int timeOfGreen2, int timeOfRed2, int timeOfYellow2)
        {
            if (mode == "Stop")
            {

                switch (colorLed1)
                {
                    case 0:
                        timeOfRreen1 = 999;
                        break;
                    case 1:
                        timeOfYellow1 = 999;
                        break;
                    case 2:
                        timeOfRed1 = 999;
                        break;
                    default:
                        break;
                }

                switch (colorLed2)
                {
                    case 0:
                        timeOfRreen2 = 999;
                        break;
                    case 1:
                        timeOfYellow2 = 999;
                        break;
                    case 2:
                        timeOfRed2 = 999;
                        break;
                    default:
                        break;
                }
            }

            SaveToDatabase(mode, DateTime.Now, timeOfGreen1, timeOfRed1, timeOfYellow1, timeOfGreen2, timeOfRed2, timeOfYellow2);
        }

        private void button_history_Click(object sender, EventArgs e)
        {
            Database databaseForm = new Database();

            databaseForm.Show();
            this.Hide();
        }

        private void SaveToDatabase(string mode, DateTime timestamp, int timeOfGreen1, int timeOfRed1, int timeOfYellow1, int timeOfGreen2, int timeOfRed2, int timeOfYellow2)
        {
            try
            {
                string connectionString = "Server=MSI\\MYDATABASE;Database=TRAFFIC_LIGHT;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO UserModeHistory (UserName, ModeName, ModeStartTime, GreenLightDuration1, YellowLightDuration1, RedLightDuration1, GreenLightDuration2, YellowLightDuration2, RedLightDuration2) " +
                        "           VALUES (@UserName, @ModeNumber, @ModeStartTime, @GreenLightDuration1, @YellowLightDuration1, @RedLightDuration1, @GreenLightDuration2, @YellowLightDuration2, @RedLightDuration2)";
                    SqlCommand command = new SqlCommand(query, connection);
                    string currentUser = UserSession.Username;
                    command.Parameters.AddWithValue("@UserName", currentUser);
                    command.Parameters.AddWithValue("@ModeNumber", mode);
                    command.Parameters.AddWithValue("@ModeStartTime", timestamp);
                    command.Parameters.AddWithValue("@GreenLightDuration1", timeOfGreen1);
                    command.Parameters.AddWithValue("@YellowLightDuration1", timeOfYellow1);
                    command.Parameters.AddWithValue("@RedLightDuration1", timeOfRed1);
                    command.Parameters.AddWithValue("@GreenLightDuration2", timeOfGreen2);
                    command.Parameters.AddWithValue("@YellowLightDuration2", timeOfYellow2);
                    command.Parameters.AddWithValue("@RedLightDuration2", timeOfRed2);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data to the database: " + ex.Message);
            }
        }
    }
}