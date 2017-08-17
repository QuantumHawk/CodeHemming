using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeHemming
{
   public class Code
    {
        byte[] InputCode;//входящий код
        byte?[] Hemmingcode;//код хемминга
        Control c;
        byte[] Control;
        int k;
        int r;
        int errors;
        public Code(byte [] c,int k,int r,Control control,int eror)
        {
            this.errors = eror;
            this.c= control;
                this.k = k;
                this.r = r;//random4egggg
                InputCode = c;
                Hemmingcode=new byte?[r+k];
                Control=new byte[r];
        }

        #region Получаем входящую строку
        static public byte[] GetCodeString(string str)
        {
            byte[] b = new byte[str.Length];
            for (int j = 0; j < str.Length; ++j)
            {
                b[j] = (byte)str[j];
            }
            return b;
        }
        #endregion

        #region переворачиваем строку для кода хемминга
        void SetHemming()
        {
            InputCode=InputCode.Reverse().ToArray();
            for (int j = 0,i=0; j < Hemmingcode.Length; ++j)
            {
                if (!IsPowOf2(j+1) )
                {
                    
                    Hemmingcode[j] = InputCode[i];
                ++i;
                }
            }
        }
        #endregion

        #region
        bool IsPowOf2(int j)
        {
            bool b = false;
            for (int i = 0; Math.Pow(2.0,(double)i)<=( r + k); ++i)
            {
                if (j == Math.Pow(2.0, (double)i))
                {
                    b = true;
                }
            }
            return b;
        }
        #endregion

        #region получаем индексы
        List<byte[]> GetIndexes()
        {
            List<byte[]> index = new List<byte[]>();
            byte[] b;
            for (int j = 0; j < Hemmingcode.Length; ++j)
            {
                if (Hemmingcode[j] == 1)
                {
                    b = Code.GetCodeString(Convert.ToString(j + 1, 2));
                    if (b.Length < r)
                    {
                        byte[] c = new byte[r];
                        b.CopyTo(c, r - b.Length);
                        b = c;
                    }
                    index.Add(b);
                }
            }
            return index;
        }
        #endregion

        #region сумма
        byte Allsum()
         {
             byte b = 0;
             for (int i = 0; i < Hemmingcode.Length; ++i)
             {
                 if(Hemmingcode[i]!=null)
                 b ^= Hemmingcode[i].Value;
             }
             return b;
         }
        #endregion

        #region получаем контрольные значения
        public void GetControl()
        {
            SetHemming();
            List<byte[]> index = GetIndexes();
            
            for (int j = 0; j < r; ++j)
            {
                for (int i = 0; i < index.Count; ++i)
                {
                    Control[j] ^= index[i][j];
                }
            }
            
            Control = Control.Reverse().ToArray();
            c.Text += "Контрольные значания   ";
            for (int j = 0,i=0; j < Hemmingcode.Length; ++j)
            {
                if (Hemmingcode[j] == null )
                {
                    c.Text += Control[i];
                    Hemmingcode[j] = Control[i];
                    ++i;
                }
            }
            c.Text += "\r\n";
        }
        #endregion

        #region делаем рандомную ошибку
        public void SetError(int count)
        {
            c.Text += "Закодированное значание " + GetHemmingCode()+"\r\n";
            Random r = new Random();
            for (int j = 0; j < count; ++j)
            {
                int k = r.Next(Hemmingcode.Length);
                Hemmingcode[k] = Hemmingcode[k] == 0 ? (byte)1 : (byte)0;
            }
            c.Text += "Закодированное значание при внесении " + errors + " ошибок" + GetHemmingCode()+"\r\n";
        }
        #endregion

        #region получаем код хемминга
        public string GetHemmingCode()
        {
            string str = "";
            foreach (byte b in Hemmingcode)
                str += b.ToString();
            return str;
        }
        #endregion

        #region
        public void CorrectError(int error)
        {
            Hemmingcode[error] = Hemmingcode[error] == 0 ? (byte)1 : (byte)0; //Условие ? истиина:ложь
        }
        #endregion

        #region кажем код
        public void ShowCode(Control c)
        {
            foreach (byte b in InputCode)
                c.Text += b;
            c.Text += "\r\n";
        }
        #endregion

        #region декодируем
        public void Decode()
        {
            SetError(errors);
            CheckErrors();
            
        }
        #endregion

        #region getsum
        byte[] GetSum(List<byte[]> index)
        {
            byte[] b = new byte[r];
            for (int j = 0; j < r; ++j)
            {
                for (int i = 0; i < index.Count; ++i)
                {
                    b[j] ^= index[i][j];
                }
            }
            return b;
        }
        #endregion

        #region проверка ошибок
        void CheckErrors()
        {
            List<byte[]> index = GetIndexes();
            byte[] b = GetSum(index);
            int k=CheckAnswer(b);
            if (k != 0)
            {
                c.Text += "Индекс ошибки " + k + "\r\n";
                CorrectError(k-1);
                CheckErrors();
            }
                
        }
        #endregion

        #region checkAnswer
        int CheckAnswer(byte[] b)
        {
            string str="";
            foreach (byte bit in b)
            {
                str += bit.ToString();
            }
            return Convert.ToInt16(str, 2);
        }
        #endregion

        #region getvalue
        public char Getvalue()
         {
            
             byte [] b=new byte[k];
             for (int j = 0,i=0; j < Hemmingcode.Length; ++j)
             {
                 if (IsPowOf2(j + 1) == false)
                 {
                     b[i] = Hemmingcode[j].Value;
                     ++i;
                    
                 }
             }
             b = b.Reverse().ToArray();
             string str = "";
             foreach (byte bit in b)
             {
                 str += bit.ToString();
             }
             return (char)Convert.ToInt32(str, 2);
         }
        #endregion
    }
}
