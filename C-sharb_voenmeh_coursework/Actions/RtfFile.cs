﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharb_voenmeh_coursework.Actions
{
    public static class RtfFile
    {
        public static string RTFToPlainText(this string s)
        {
            // for information : default Xceed.Wpf.Toolkit.RichTextBox formatter is RtfFormatter 
            Xceed.Wpf.Toolkit.RichTextBox rtBox = new Xceed.Wpf.Toolkit.RichTextBox(new System.Windows.Documents.FlowDocument());
            rtBox.Text = s;
            rtBox.TextFormatter = new Xceed.Wpf.Toolkit.PlainTextFormatter();
            return rtBox.Text;

        }
    }
}
