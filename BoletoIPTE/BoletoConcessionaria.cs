using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoIPTE
{
    class BoletoConcessionaria : Boleto
    {

        #region Properties
        private string _codVerificadorTotal;
        public string CodVerificadorTotal { get { return _codVerificadorTotal; } }

        private string _valor;
        public decimal Valor { get { return Convert.ToDecimal(_valor) / 100; } }


        private string _produto;
        public string Produto { get { return _produto; } }

        private string _segmento;
        public string Segmento { get { return InterpretaSegmento(_segmento); } }

        private string _identificacaoValor;
        public string IdentificacaoValor { get { return _identificacaoValor; } }

        private string _empresa;
        public string Empresa { get { return _empresa; } }

        private string _campoLivre1;
        public string CampoLivre1 { get { return _campoLivre1; } }

        #endregion

        public BoletoConcessionaria(string codigoBarras)
            : base(codigoBarras)
        {

        }

        protected override void InitializeFromCodigoBarras()
        {
            if (CodigoBarras.Length == 44)
            {
                _produto = CodigoBarras.Substring(0, 1);
                _segmento = CodigoBarras.Substring(1, 1);
                _identificacaoValor = CodigoBarras.Substring(2, 1);
                _codVerificadorTotal = CodigoBarras.Substring(3, 1);
                _valor = CodigoBarras.Substring(4, 11);
                _empresa = CodigoBarras.Substring(15, 4);
                _campoLivre1 = CodigoBarras.Substring(19, 25);
            }
            else
            {
                throw new Exception("tamanho do codigo de barras inconsistente");
            }
        }

        protected override void SetIPTE()
        {
            _IPTE = AddWithVerificator(_produto + _segmento + _identificacaoValor + _codVerificadorTotal, _valor.Substring(0, 7));
            _IPTE = _IPTE + AddWithVerificator(_valor.Substring(7, 4), _empresa + _campoLivre1.Substring(0, 3));
            _IPTE = _IPTE + AddWithVerificator(_campoLivre1.Substring(3, 10), _campoLivre1.Substring(13, 1));
            _IPTE = _IPTE + AddWithVerificator(_campoLivre1.Substring(14, 1), _campoLivre1.Substring(15, 10));
        }

        protected override void SetIPTE_UI()
        {
            _IPTE_UI = IPTE.Insert(11, "-");
            _IPTE_UI = _IPTE_UI.Insert(13, " ");
            _IPTE_UI = _IPTE_UI.Insert(25, "-");
            _IPTE_UI = _IPTE_UI.Insert(27, " ");
            _IPTE_UI = _IPTE_UI.Insert(39, "-");
            _IPTE_UI = _IPTE_UI.Insert(41, " ");
            _IPTE_UI = _IPTE_UI.Insert(53, "-");
        }


        public override string Informacoes()
        {
            string resumo = "";

            resumo += "\r\nProduto: " + Produto;
            resumo += "\r\nSegmento: " + Segmento;
            resumo += "\r\nValor: " + Valor.ToString();
            resumo += "\r\n";
            resumo += "\r\nIPTE: " + IPTE;
            resumo += "\r\nIPTE(UI): " + IPTE_UI;
            resumo += "\r\nCodigoBarras: " + CodigoBarras;

            return resumo;
        }

        string InterpretaSegmento(string segmento)
        {
            string retorno = "";
            switch (segmento)
            {
                case "1":
                    retorno = "Prefeituras";
                    break;
                case "2":
                    retorno = "Saneamento";
                    break;
                case "3":
                    retorno = "Energia Elétrica e Gás";
                    break;
                case "4":
                    retorno = "Telecomunicações";
                    break;
                case "5":
                    retorno = "Órgãos Governamentais";
                    break;
                case "6":
                    retorno = "Demais Empresas";
                    break;
                case "7":
                    retorno = "Multas de trânsito";
                    break;
                case "9":
                    retorno = "Uso exclusivo do banco";
                    break;
                default:
                    retorno = "Não identificado";
                    break;
            }
            return retorno;
        }
    }
}
