using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crru
{

        class input
        {
            string data = "";
            public input(string variableName)
            {
                inputbox Form = new inputbox();
                Form.label1.Text = variableName;
                Form.ShowDialog();
                data = Form.data;
            }
            public string getData()
            {
                return data;
            }

        }
    }
