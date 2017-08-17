using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeHemming
{
    public partial class Form1 : Form
    {

        List<Code> codes;
        public Form1()
        {
            codes = new List<Code>();
           
            InitializeComponent();
        }
        byte[] GetCodeString(string str)
        {
            byte[] b = new byte[str.Length];
            for (int j = 0; j < str.Length; ++j)
            {
                b[j] = (byte)str[j];
            }
            return b;
        }
        public void GetStringCode(string str,int error,int n,int k)
        {
            
            byte [] b;
            foreach (char c in str.ToCharArray())
            {
                b=GetCodeString(Convert.ToString((int)c, 2));
                if (b.Length < k)
                {
                    byte[] buffer = new byte[k];
                    b.CopyTo(buffer,k-b.Length);
                    b = buffer;
                }
                codes.Add(new Code(b,b.Length,n-k,richTextBox1,error));
            }
            Show2Mas();
        }
        public void Show2Mas()
        {
            richTextBox1.Text = "Текст представленный в двоичной системе исчисления";
            foreach (Code c in codes)
            {
                c.ShowCode(richTextBox1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            codes.Clear();
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Укажите ошибки");
                return;
            }
            int error = int.Parse(comboBox1.Text);
            int r = int.Parse(comboBox2.Text.Split()[0]);
            int k = int.Parse(comboBox2.Text.Split()[1]);
            GetStringCode(textBox1.Text, error,r,k);
            foreach (Code c in codes)
            {
                c.GetControl();
               richTextBox1.Text+="   "+ c.GetHemmingCode()+"\r\n";
            }
         
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Code c in codes)
            {
                c.Decode();
            }
            string str = "";
            foreach (Code c in codes)
            {
                str += c.Getvalue();
            }
            richTextBox1.Text += str;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "1" || comboBox1.Text == "2")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("11 7");
                comboBox2.Items.Add("15 11");
                comboBox2.Items.Add("31 26");
                comboBox2.Items.Add("63 57");
            }
            if (comboBox1.Text == "2")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("32 26");
                comboBox2.Items.Add("64 57");
                comboBox2.Items.Add("128 120");
            }
                if( comboBox1.Text == "3")
            {
                comboBox2.Items.Clear();
                
                comboBox2.Items.Add("31 26");
                comboBox2.Items.Add("63 45");
                comboBox2.Items.Add("127 106");
            }
            comboBox2.SelectedIndex=0;
        }

        
    }
}
