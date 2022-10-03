using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USBManagement;

namespace Usb_Hid
{
    public partial class Form1 : Form
    {

        UsbHidDevice my_hid = new UsbHidDevice();
        short VendorID = short.Parse("1111", NumberStyles.HexNumber);
        short ProductID = short.Parse("1111", NumberStyles.HexNumber);

        byte[] my_buffer = new byte[64];
        byte[] gelen_buffer = new byte[64];
        public Form1()
        {
            InitializeComponent();
            my_hid.DeviceStateChanged += new DeviceStateChangeEventHandler(OnChangeDeviceState);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

        }

        //Sure Methodu
        void sure(int x)
        {
            int a;
            long b;

            for (a = 0; a <= 10000 * x; a++)
            {
                for (b = 0; b <= 100 * x; b++)
                {

                }
            }

        }

        //BAGLANTI KUR
        private void button1_Click(object sender, EventArgs e)
        {
            if (!my_hid.IsOpen)
            {

                if (my_hid.OpenPipe(VendorID, ProductID))
                {
                 

                    my_hid.DeviceArrived += new HidDeviceArrivedEventHandler(OnDeviceAttached);
                    my_hid.DeviceRemoved += new HidDeviceRemovedEventHandler(OnDeviceDetached);

                }
                else
                {
                    toolStripStatusLabel1.Text = "Cihaz bulunamadi...!";
                }

            }

        }


        //BAGLANTI KES
        private void button2_Click(object sender, EventArgs e)
        {
            if (my_hid.IsOpen)
            {
                my_hid.ClosePipe();
            }

        }

        //Cihazin Durumunda Degisiklikler oldugunda Devreye giren Method
        private void OnChangeDeviceState(object sender, DeviceStateChangeEventArgs e)
        {
            if (e.CurrentState == DeviceState.Opened)
            {
                toolStripStatusLabel1.Text = "Cihaz Durumu : Baglanti kuruldu !";
                //Debug.WriteLine("Device State Changed!! Curren State : Opened");
            }
            else if (e.CurrentState == DeviceState.Closed)
            {
                toolStripStatusLabel1.Text = "Cihaz Durumu : Baglanti kesildi !";
                //Debug.WriteLine("Device State Changed!! Curren State : Closed");
            }
            else if (e.CurrentState == DeviceState.Waiting)
            {
                toolStripStatusLabel1.Text = "Cihaz Durumu : Baglanti saglaniyor...";
                //Debug.WriteLine("Device State Changed!! Curren State : Waiting");
            }
            else if (e.CurrentState == DeviceState.Ready)
            {
                toolStripStatusLabel1.Text = "Cihaz Durumu : Okuma yapiliyor !";
                //Debug.WriteLine("Device State Changed!! Curren State : Ready");
            }
            else
            {
                toolStripStatusLabel1.Text = "Device State Changed!! Curren State : None";
                //Debug.WriteLine("Device State Changed!! Curren State : None");
            }
        }


        private void OnDeviceAttached(object sender, EventArgs e)
        {
            bool Success;

            toolStripStatusLabel1.Text = "Atached Device";
            Success = my_hid.OpenPipe(VendorID, ProductID);
            if (Success)
            {

                toolStripStatusLabel1.Text = "Bağlantı kuruldu";

            }
        }

        private void OnDeviceDetached(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Detached Device";
            if (my_hid.IsOpen) my_hid.ClosePipe();

            toolStripStatusLabel1.Text = "Cihaz ile bağlantı kesildi";
            this.Text = "Cihaz bekleniyor..";
        }

        public void lcdYaz(string metin, int satır)//Metin LCD ye yazılacak metindir. Satır İse LCD satrıdır.
        {
            if (satır == 0)
            {
                if (my_hid.IsOpen)
                {
                    if (metin == "")
                    {
                        MessageBox.Show("LCD ye göndermek istediginiz Yaziyi giriniz");
                    }
                    else
                    {
                        int adet;
                        string s =
                        s = Convert.ToString(" " + " " + metin).TrimEnd() + " ";

                        //my_buffer = Encoding.UTF8.GetBytes(s);   // s stringini byte ceviriyoruz
                        my_buffer = UnicodeEncoding.UTF8.GetBytes(s);   // s stringini byte ceviriyoruz
                        adet = s.Length - 2;           // s stringinin basina atadigimiz bos karekter sayisini cikartiyoruz


                        my_buffer[0] = (byte)adet;   // Gönderecegimiz stringin sayisi
                        my_buffer[1] = (byte)'W';      // Isaretci harf
                        if (my_hid.IsWindowsXpOrLater)
                        {
                            my_hid.WritePipe(my_buffer, TransactionType.Interrupt);
                            my_hid.WritePipe(my_buffer, TransactionType.Control);
                        }
                        else

                            my_hid.WritePipe(my_buffer, TransactionType.Control);
                    }
                }
                else
                {
                    label1.Text = "Cihaz ile bağlantı yok!";
                }
            }



            if (satır == 1)
            {
                if (my_hid.IsOpen)
                {
                    if (metin == "")
                    {
                        MessageBox.Show("LCD ye göndermek istediginiz Yaziyi giriniz");
                    }
                    else
                    {
                        int adet;
                        string s =
                        s = Convert.ToString(" " + " " + metin).TrimEnd() + " ";

                        //my_buffer = Encoding.UTF8.GetBytes(s);   // s stringini byte ceviriyoruz
                        my_buffer = UnicodeEncoding.UTF8.GetBytes(s);   // s stringini byte ceviriyoruz
                        adet = s.Length - 2;           // s stringinin basina atadigimiz bos karekter sayisini cikartiyoruz


                        my_buffer[0] = (byte)adet;   // Gönderecegimiz stringin sayisi
                        my_buffer[1] = (byte)'Y';      // Isaretci harf
                        if (my_hid.IsWindowsXpOrLater)
                        {
                            my_hid.WritePipe(my_buffer, TransactionType.Interrupt);
                            my_hid.WritePipe(my_buffer, TransactionType.Control);
                        }
                        else

                            my_hid.WritePipe(my_buffer, TransactionType.Control);
                    }
                }
                else
                {
                    label1.Text = "Cihaz ile bağlantı yok!";
                }
            }
        }
        public void lcdTemizle()//Lcd Yi Komple Siler
        {
            if (my_hid.IsOpen)
            {
                my_buffer[0] = (byte)0;   // Gönderecegimiz stringin sayisi
                my_buffer[1] = (byte)'Z';
                if (my_hid.IsWindowsXpOrLater)
                {
                    my_hid.WritePipe(my_buffer, TransactionType.Interrupt);
                    my_hid.WritePipe(my_buffer, TransactionType.Control);
                }
                else

                    my_hid.WritePipe(my_buffer, TransactionType.Control);
            }

            else
            {
                label1.Text = "Cihaz ile bağlantı yok!";
            }
        }

        public void ledControl(char led)
        {
            my_buffer[0] = (byte)led;

            if (my_hid.IsOpen)
            {
                if (my_hid.IsWindowsXpOrLater)
                {
                    my_hid.WritePipe(my_buffer, TransactionType.Interrupt);
                    my_hid.WritePipe(my_buffer, TransactionType.Control);
                }
                else
                    my_hid.WritePipe(my_buffer, TransactionType.Control);
            }
            else
            {
                toolStripStatusLabel1.Text = "Cihaz ile bağlantı yok!";
            }
        }
        public void motorControl(char motorno, byte sure)//Kullanımı  motorControl('i',Byte.Parse(trackBar1.Value.ToString()));Servo Motorlara PWM Göndermek İçin Kullanılır 
        {
            if (my_hid.IsOpen)
            {
                my_buffer[1] = (byte)'Q';
                my_buffer[2] = (byte) motorno;
                my_buffer[5] = (byte)(sure / 2); 

                if (my_hid.IsWindowsXpOrLater)
                    my_hid.WritePipe(my_buffer, TransactionType.Interrupt);
            }
            else
            {
                toolStripStatusLabel1.Text = "Cihaz ile bağlantı yok!";
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ledControl('y');
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ledControl('s');
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (my_hid.IsOpen)
            {
                ledControl('s');
                my_hid.ClosePipe();
            }

        }


        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {

            motorControl('i',Byte.Parse(trackBar1.Value.ToString()));
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            motorControl('j', Byte.Parse(trackBar2.Value.ToString()));
        }

        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            motorControl('k', Byte.Parse(trackBar3.Value.ToString()));
        }

        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            motorControl('l', Byte.Parse(trackBar4.Value.ToString()));
        }

        private void trackBar5_MouseUp(object sender, MouseEventArgs e)
        {
            motorControl('m', Byte.Parse(trackBar5.Value.ToString()));
        }
       
        
        
        
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label6.Text = trackBar3.Value.ToString();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label8.Text = trackBar4.Value.ToString();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label10.Text = trackBar5.Value.ToString();
        }
     


        public void ledPinControl(char led)
        {
            if (my_hid.IsOpen)
            {
                my_buffer[6] = (byte)led;
                if (my_hid.IsWindowsXpOrLater)
                    my_hid.WritePipe(my_buffer, TransactionType.Interrupt);
            }
            else
            {
                toolStripStatusLabel1.Text = "Cihaz ile bağlantı yok!";
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            ledPinControl('I');
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ledPinControl('D');
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ledPinControl('G');
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ledPinControl('a');
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ledPinControl('s');
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ledPinControl('a');
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ledPinControl('s');
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ledPinControl('d');
        }

        private void button16_Click(object sender, EventArgs e)
        {
            ledPinControl('f');
        }

        private void button18_Click(object sender, EventArgs e)
        {
            ledPinControl('g');
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ledPinControl('h');
        }

        private void button22_Click(object sender, EventArgs e)
        {
            ledPinControl('j');
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ledPinControl('z');
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ledPinControl('x');
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ledPinControl('c');
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ledPinControl('v');
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ledPinControl('b');
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ledPinControl('n');
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ledPinControl('m');
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            lcdYaz(textBox1.Text, 0);
            lcdYaz(textBox2.Text, 1);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            lcdTemizle();
        }

}
}
