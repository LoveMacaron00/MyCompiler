using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crru
{
    public partial class exam : Form
    {
        public exam()
        {
            InitializeComponent();
            exam1("a in(1, 9..11, 15..20, 77, 80..90)");
        }

        /*
         parametor
	        "a in(1,9,15..20,77,80..90)"
        retrun
	        a=1 or a=9 or (a>=15 and a<=20) or a=77 or (a>=80 and a<=90)
        */

        private void exam1(string jo)
        {
            if (jo.Contains("in"))
            {
                string[] sp = jo.Split(new string[] { "in" }, StringSplitOptions.RemoveEmptyEntries);
                string varNa = "a";
                string condi = sp[1].Trim('(', ')');
                string[] sp2 = condi.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder result = new StringBuilder();
                for (int i = 0; i < sp2.Length; i++)
                {
                    string sp2s = sp2[i];
                    if (sp2s.Contains(".."))
                    {
                        string[] ran = sp2s.Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
                        result.Append($"({varNa}>={ran[0]} and {varNa}<={ran[1]}) or ");
                    }
                    else
                    {
                        result.Append($"{varNa}={sp2s} or ");
                    }
                }
                result.Remove(result.Length - 4, 4);

                Console.WriteLine(result);
            }
        }


    }
}
