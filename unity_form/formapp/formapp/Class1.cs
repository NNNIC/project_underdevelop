using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;
using UnityEditor;


namespace formapp
{
    public class Class1
    {
        public void ShowMessageBox()
        {
            MessageBox.Show("Test");
        }

        public void ShowSelected()
        {
            if (Selection.objects!=null && Selection.objects.Length>0)
            {
                MessageBox.Show(Selection.objects[0].name);
            }
        }

        public void ShowForm()
        {
            System.Diagnostics.Debugger.Break();

            var form = new Form1();
            form.Show();
            
            form.dummy();
        }
    }
}
