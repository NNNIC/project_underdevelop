//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChartViewer = chart.ChartViewer;
using Detail=DrawStateBox.Detail;
using LineType=DrawUtil.LineType;
using D=Define;
//>>>


    // 自動作成されて使わなくなったAPI
namespace chart
{
    public partial class ChartViewer 
    {

        private void pictureBox_main_Click(object sender,EventArgs e)
        {
            //if (m_pictureBox_select_show)
            //{

            //    m_pictureBox_select_show = false;
            //    pictureBox_select.Hide();
            //}
            //else
            //{
            //    m_pictureBox_select_show = true;
            //    pictureBox_select.Show();
            //}
        }

        private void ChartViewer_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("cv Mouse Down\n");
        }

        private void ChartViewer_DragDrop(object sender,DragEventArgs e)
        {
            DBG.LogWrite("DragDrop\n");
        }

        private void ChartViewer_DragEnter(object sender,DragEventArgs e)
        {
            DBG.LogWrite("DragEnter\n");
        }

        private void ChartViewer_DragLeave(object sender,EventArgs e)
        {
            DBG.LogWrite("DragLeave\n");
        }

        private void ChartViewer_DragOver(object sender,DragEventArgs e)
        {
            DBG.LogWrite("DragOver\n");
        }

        private void pictureBox_main_DragDrop(object sender,DragEventArgs e)
        {

        }

        private void pictureBox_main_DragEnter(object sender,DragEventArgs e)
        {

        }

        private void pictureBox_main_DragLeave(object sender,EventArgs e)
        {

        }

        private void pictureBox_main_DragOver(object sender,DragEventArgs e)
        {

        }

        private void pictureBox_main_GiveFeedback(object sender,GiveFeedbackEventArgs e)
        {

        }

        private void pictureBox_main_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("main Mouse Down\n");
        }

        private void pictureBox_select_Click(object sender,EventArgs e)
        {
            DBG.LogWrite("select Mouse Click\n");

        }

        private void pictureBox_select_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("select Mouse Down\n");
        }

        private void pictureBox_highlite_Click(object sender,EventArgs e)
        {

        }

        private void pictureBox_highlite_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("heighlight Mouse Down\n");
        }

        private void pictureBox_collider_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite(" colider Mouse Down\n");
        }

    }
}