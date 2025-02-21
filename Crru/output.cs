using System;
using System.Windows.Forms;

namespace Crru
{
    internal class Output
    {
        public static void Show(RichTextBox richTextBox, params object[] values)
        {
            foreach (var value in values)
            {
                richTextBox.AppendText(value.ToString());
            }
            richTextBox.AppendText(Environment.NewLine);
        }
    }
}
