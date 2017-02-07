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
        BindingSource bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            labelInfo.Text = "";
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_codBarras.Text.Length == 0) { throw new Exception("Codigo de barras vazio"); }

                Boleto myBoleto = BoletoHandler.ConstructBoleto(txt_codBarras.Text);
                _lstBoleto.Add(myBoleto);

                // Change the DataSource.
                UpdateDataSource();


            }
            catch (Exception ex)
            { MessageBox.Show("Erro: " + ex.Message); }

            if (chk_Limpar.Checked)
            {
                txt_codBarras.Text = "";
            }
        }

        private void InitializeListBox()
        {
            bs.DataSource = _lstBoleto;

            listBox_IPTE.DisplayMember = "IPTE_UI";
            listBox_IPTE.DataSource = bs;
        }

        private void UpdateDataSource()
        {
            listBox_IPTE.DataSource = null;
            listBox_IPTE.DataSource = _lstBoleto;
            listBox_IPTE.DisplayMember = "IPTE_UI";
        }

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
                buttonRead_Click(this, new EventArgs());
            }
        }

        private void listBox_IPTE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_IPTE.SelectedIndex != -1)
            {
                labelInfo.Text = _lstBoleto[listBox_IPTE.SelectedIndex].Informacoes();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox_IPTE.SelectedIndex != -1)
            {
                _lstBoleto.RemoveAt(listBox_IPTE.SelectedIndex);
                UpdateDataSource();
            }
        }
    }
}
