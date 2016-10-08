using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace 彩票
{
    public partial class Form1 : Form
    {
        public List<String> a_v = new List<String>(32);
        public List<String> b_v = new List<String>(32);
        //AC值
        public List<String> ac_v = new List<String>(32);
        //奇偶比
        public List<String> jo_v = new List<String>(32);
        //大小比
        public List<String> dx_v = new List<String>(32);
        //连号
        public List<String> lh_v = new List<String>(32);
        public List<List<int>> result = new List<List<int>>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void a_checked(object sender, EventArgs e)
        {
            String this_text = ((CheckBox)sender).Text;
            bool this_checked = ((CheckBox)sender).Checked;
            if (this_checked)
                a_v.Add(this_text);
            else
                a_v.Remove(this_text);
            a_v.Sort();
            a1_v.Text = String.Join(",", a_v);
        }


        private void b_checked(object sender, EventArgs e)
        {
            String this_text = ((CheckBox)sender).Text;
            bool this_checked = ((CheckBox)sender).Checked;
            if (this_checked)
                b_v.Add(this_text);
            else
                b_v.Remove(this_text);
            b_v.Sort();
            b1_v.Text =String.Join(",",b_v);
        }


        private void ac_checked(object sender, EventArgs e)
        {
            String this_text = ((CheckBox)sender).Text;
            bool this_checked = ((CheckBox)sender).Checked;
            if (this_checked)
                ac_v.Add(this_text);
            else
                ac_v.Remove(this_text);
         }

        private void jo_checked(object sender, EventArgs e)
        {
            String this_text = ((CheckBox)sender).Text;
            bool this_checked = ((CheckBox)sender).Checked;
            if (this_checked)
                jo_v.Add(this_text);
            else
                jo_v.Remove(this_text);
        }

        private void dx_checked(object sender, EventArgs e)
        {
            String this_text = ((CheckBox)sender).Text;
            bool this_checked = ((CheckBox)sender).Checked;
            if (this_checked)
                dx_v.Add(this_text);
            else
                dx_v.Remove(this_text);
        }

        private void lh_checked(object sender, EventArgs e)
        {
            String this_text = ((CheckBox)sender).Text;
            bool this_checked = ((CheckBox)sender).Checked;
            if (this_checked)
                lh_v.Add(this_text);
            else
                lh_v.Remove(this_text);
        }


        private void button1_Click(object sender, EventArgs e)

        {
            bool check = true;
            String checkStr = "";
            foreach(String a in a_v)
            {
                foreach(String b in b_v)
                {
                    if (a.Equals(b))
                    {
                        checkStr = a;
                        check = false;
                        break;
                    }
                }
            }

            if (!check)
            {
                MessageBox.Show("胆号和拖号不能重复:"+ checkStr);
                return;
            }


            result.Clear();
            log_textbox.Text = "";
            result_textbox.Text = "";
            String[] bv= b_v.ToArray();
            String[] av = a_v.ToArray();
            var res=av.Select(x => new string[] { x });
            for (int i = 0; i < 5 - bv.Length; i++)
            {
                res = res.SelectMany(x => av.Where(y => y.CompareTo(x.First()) < 0).Select(y => new string[] { y }.Concat(x).ToArray()));
            }
           
            foreach (var item in res)
            {
                List<int> r1 = new List<int>(6);
                for (int i = 0; i < bv.Length; i++)
                {
                    r1.Add(Int32.Parse(bv[i]));
                }
                foreach (String v in item)
                {
                    r1.Add(Int32.Parse(v));
                    r1.Sort();
                   
                }
                result.Add(r1);

            }

           
           //ac值
            foreach (List<int> item in Clone(result))
            {
                int acval = acVal(item);
                log_textbox.AppendText(System.Environment.NewLine + String.Join(",", item) + "当前AC值" + acval);
                bool isok = false;
                foreach (String acv in ac_v)
                {
                    if (acval == int.Parse(acv))
                    {

                        isok = true;
                        break;
                    }
                }
                if (!isok && ac_v.Count != 0)
                    result.Remove(item);

            }

            //和值
            foreach (List<int> item in Clone(result))
            {
                int sum = item.Sum();
                log_textbox.AppendText(System.Environment.NewLine + String.Join(",", item) + "当前和值" + sum);
                if (sum <= int.Parse(sum_min.Text) || sum >= int.Parse(sum_max.Text))
                {
                    Console.WriteLine("s:" + result.Count);
                    result.Remove(item);
                    Console.WriteLine("e:" + result.Count);
                }

            }

            //奇偶比
            foreach (List<int> item in Clone(result))
            {
                int j = 0;
                int o = 0;
                foreach(int k in item)
                {
                    if (k % 2 == 0)
                        o++;
                    else
                        j++;
                }
                String jo_key = j + ":" + o;

                log_textbox.AppendText(System.Environment.NewLine + String.Join(",", item) + "当前奇偶比" + jo_key);
                bool isok = false;
                foreach (String jov in jo_v)
                {
                    if (jo_key.Equals(jov))
                    {
                        isok = true;
                        break;
                    }
                }
                if (!isok && jo_v.Count != 0)
                    result.Remove(item);

            }


            //大小比
            foreach (List<int> item in Clone(result))
            {
                int d= 0;
                int x = 0;
                foreach (int k in item)
                {
                    if (k % 10 >=5)
                        d++;
                    else
                        x++;
                }
                String dx_key = d+ ":" + x;

                log_textbox.AppendText(System.Environment.NewLine + String.Join(",", item)+"当前大小比" + dx_key);
                bool isok = false;
                
                foreach (String dxv in dx_v)
                {
                    if (dx_key.Equals(dxv))
                    {
                        isok = true;
                        break;
                    }
                }
                if (!isok&& dx_v.Count!=0)
                    result.Remove(item);

            }


            //连号
            foreach (List<int> item in Clone(result))
            {
                int k = 1;
                int l = 1;
               
                int lastn = 0;
               for(int i = 0; i < item.Count; i++)
                {
                    if(i==0)
                        lastn= item.ElementAt(i);
                    else if (item.ElementAt(i) == lastn + 1) 
                    {
                        l++;
                    }else
                    {
                        if(k<l)
                            k = l;
                        l = 1;
                    }
                    lastn = item.ElementAt(i);
                }

                int v = k > l ? k : l;
                String lh_key = v + "连号";
                bool isok = false;
                log_textbox.AppendText(System.Environment.NewLine + String.Join(",", item) + "当前连号" + lh_key);
                foreach (String lhv in lh_v)
                {
                    if (lh_key.Equals(lhv))
                    {
                        isok = true;
                        break;
                    }
                }

                if (!isok && lh_v.Count != 0)
                    result.Remove(item);
            }



            Console.WriteLine("最终号码:");

            for( int i = 0; i < result.Count; i++)
            {
                if (i > 0 && i % 7 == 0)
                {
                    result_textbox.AppendText(System.Environment.NewLine);
                }
                result_textbox.AppendText(String.Join(",", result.ElementAt(i))+"  ");

            }
           

        }



        public int acVal(List<int> item)
        {
            HashSet<int> acvals = new HashSet<int>();
           int[] isv= item.ToArray();
            for(int i = 0; i< isv.Length - 1; i++)
            {
                for(int j=i+1; j< isv.Length; j++)
                {
                    acvals.Add(isv[j] - isv[i]);
                }
            }

            return acvals.Count - 5;
        }


        public static List<List<int>> Clone(List<List<int>> RealObject)

        {
            return  new List<List<int>>(RealObject);
        }

    }
}


