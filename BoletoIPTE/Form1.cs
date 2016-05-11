using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoletoIPTE
{
    public partial class Form1 : Form
    {
        List<Boleto> _lstBoleto = new List<Boleto>();

        public Form1()
        {
            InitializeComponent();
            labelInfo.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_codBarras.Text.Length == 0) { throw new Exception("Codigo de barras vazio"); }

                Boleto myBoleto = BoletoHandler.ConstructBoleto(txt_codBarras.Text);
                _lstBoleto.Add(myBoleto);

                // Change the DataSource.
                listBox_IPTE.DataSource = null;
                listBox_IPTE.DisplayMember = "IPTE_UI";
                listBox_IPTE.DataSource = _lstBoleto;
            }
            catch (Exception ex)
            { MessageBox.Show("Erro: " + ex.Message); }

            if (chk_Limpar.Checked)
            {
                txt_codBarras.Text = "";
            }
        }

        //private void listBox_IPTE_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    int index = this.listBox_IPTE.IndexFromPoint(e.Location);
        //    if (index != System.Windows.Forms.ListBox.NoMatches)
        //    {
        //    //    MessageBox.Show(_lstBoleto[index].ToString());
        //        labelInfo.Text = _lstBoleto[index].ToString();
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_lstBoleto[listBox_IPTE.SelectedIndex].IPTE);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_lstBoleto[listBox_IPTE.SelectedIndex].Informacoes());
        }

        private void textBoxTest_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (chk_AutoInsert.Checked))
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void listBox_IPTE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_IPTE.SelectedIndex != -1)
            {
                labelInfo.Text = _lstBoleto[listBox_IPTE.SelectedIndex].Informacoes();
            }
        }
    }
}
