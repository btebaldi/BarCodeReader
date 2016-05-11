using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoIPTE
{
    class BoletoBancario : Boleto
    {
        #region Properties

        private string _banco;
        public string Banco { get { return _banco; } }

        private string _moeda;
        public string Moeda { get { return _moeda; } }

        private string _codVerificadorTotal;
        public string CodVerificadorTotal { get { return _codVerificadorTotal; } }

        private string _fatorVencimento;
        public DateTime Vencimento { get { return CalculaVencimento(_fatorVencimento); } }

        private string _valor;
        public decimal Valor { get { return Convert.ToDecimal(_valor) / 100; } }


        private string _campoLivre1;
        public string CampoLivre1 { get { return _campoLivre1; } }

        private string _campoLivre2;
        public string CampoLivre2 { get { return _campoLivre2; } }

        private string _campoLivre3;
        public string CampoLivre3 { get { return _campoLivre3; } }

        private string _campoLivre4;
        public string CampoLivre4 { get { return _campoLivre4; } }

        private string _campoLivre5;
        public string CampoLivre5 { get { return _campoLivre5; } }
        #endregion

        public BoletoBancario(string codigoBarras)
            : base(codigoBarras)
        {

        }


        protected override void InitializeFromCodigoBarras()
        {
            if (CodigoBarras.Length == 44)
            {
                _banco = CodigoBarras.Substring(0, 3);
                _moeda = CodigoBarras.Substring(3, 1);
                _codVerificadorTotal = CodigoBarras.Substring(4, 1);
                _fatorVencimento = CodigoBarras.Substring(5, 4);
                _valor = CodigoBarras.Substring(9, 10);

                _campoLivre1 = CodigoBarras.Substring(19, 5);
                _campoLivre2 = CodigoBarras.Substring(24, 5);
                _campoLivre3 = CodigoBarras.Substring(29, 5);
                _campoLivre4 = CodigoBarras.Substring(34, 5);
                _campoLivre5 = CodigoBarras.Substring(39, 5);

            }
            else
            {
                throw new Exception("tamanho do codigo de barras inconsistente");
            }
        }

        protected override void SetIPTE()
        {
            _IPTE = AddWithVerificator(_banco + _moeda, _campoLivre1) + AddWithVerificator(_campoLivre2, _campoLivre3) + AddWithVerificator(_campoLivre4, _campoLivre5) + _codVerificadorTotal + _fatorVencimento + _valor;
        }

        protected override void SetIPTE_UI()
        {
            _IPTE_UI = IPTE.Insert(5, ".");
            _IPTE_UI = _IPTE_UI.Insert(11, " ");
            _IPTE_UI = _IPTE_UI.Insert(17, ".");
            _IPTE_UI = _IPTE_UI.Insert(24, " ");
            _IPTE_UI = _IPTE_UI.Insert(30, ".");
            _IPTE_UI = _IPTE_UI.Insert(37, " ");
            _IPTE_UI = _IPTE_UI.Insert(39, " ");
        }

        public override string Informacoes()
        {
            string resumo = "";

            resumo += "\r\nBanco: " + Banco;
            resumo += "\r\nVencimento: " + Vencimento.ToString("yyyy-MM-dd");
            resumo += "\r\nValor: " + Valor;
            resumo += "\r\n";
            resumo += "\r\nIPTE: " + IPTE;
            resumo += "\r\nIPTE(UI): " + IPTE_UI;
            resumo += "\r\nCodigoBarras: " + CodigoBarras;

            return resumo;
        }


        #region Metodos Particulares

        DateTime CalculaVencimento(string fatorVencimento)
        {
            //Foi estipulada como o marco zero para o cálculo do Fator de Vencimento a Data Base de 07.10.1997.
            //Para se obter o Fator de Vencimento temos que calcular o número de dias entre a data base e a data do vencimento
            //(a data de vencimento menos a data base será igual ao fator).

            DateTime myDate = new DateTime();
            DateTime dataBase = new DateTime(1997, 10, 07);

            int dias = Convert.ToInt32(_fatorVencimento);


            return dataBase.AddDays(dias);
        }
        #endregion
    }
}
