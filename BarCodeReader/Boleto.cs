using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoIPTE
{
    abstract class Boleto
    {
        #region Properties

        public string CodigoBarras { get; set; }

        protected string _IPTE;
        public string IPTE { get { return _IPTE; } }

        protected string _IPTE_UI;
        public string IPTE_UI { get { return _IPTE_UI; } }

        #endregion

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="p"></param>
        protected Boleto(string codigoBarras)
        {
            this.CodigoBarras = codigoBarras;
            Validate();
            InitializeFromCodigoBarras();
            SetIPTE();
            SetIPTE_UI();
        }

        #region Abstract Methods
        protected abstract void InitializeFromCodigoBarras();
        protected abstract void SetIPTE();
        protected abstract void SetIPTE_UI();
        public abstract string Informacoes();
        #endregion

        private void Validate()
        {
            // valida se o codigo contem apenas numeros
            if (!System.Text.RegularExpressions.Regex.IsMatch(CodigoBarras, @"^\d+$"))
            {
                throw new Exception("Codigo de barras invalido.");
            }
        }

        private int CalculaDigitoVerificador(string number)
        {

            int nDigitoAtual;
            int nContador = 0;
            int multiplicador;
            int soma = 0;
            int resto = 0;
            int digitoverificador = 0;

            // Calculo da soma de digitos multiplicadores
            for (nContador = 1; nContador <= number.Length; nContador++)
            {
                bool isNumeric = int.TryParse(number.Substring(number.Length - nContador, 1), out nDigitoAtual);
                if (isNumeric)
                {
                    if (IsOdd(nContador))
                    { multiplicador = 2; }
                    else
                    { multiplicador = 1; }

                    soma = soma + SomaAlgarismos((multiplicador * nDigitoAtual));
                }
                else
                {
                    throw new Exception("Digito Verificador nao pode ser calculado");
                }
            }

            // Calculo do resto da divisao.
            resto = soma % 10;

            if (resto == 0)
            { digitoverificador = 0; }
            else
            {
                digitoverificador = 10 - resto;
            }

            return digitoverificador;

            //        Cálculo do Dígito Verificador para os três primeiros campos

            //Para se calcular o Dígito Verificador de cada um dos três primeiros campos, deve-se utilizar o módulo 10, da seguinte maneira:

            //Multiplique cada dígito de cada campo, iniciando-se da direita para a esquerda, pela seqüência de 2, 1, 2, 1, 2,...;
            //Some individualmente os algarismos dos resultados, obtendo-se um total “X”;
            //Divida o valor “X” por 10 (Y = X/10) e determine o resto da divisão (R);
            //Calcule o DV (Dígito Verificador) através da expressão: DV = 10 - R.
            //Observação: Utilizar o dígito "0" para o resto 0 (zero). Exemplo:

            //    1º CAMPO
            //  +---+---+---+---+---+---+---+---+---+---+---+
            //  | 9 | 9 | 9 | 9 | 7 | . | 7 | 7 | 2 | 1 |    |  Dados do código
            //  +---+---+---+---+---+---+---+---+---+---+---+
            //     |   |    |    |    |        |     |    |    |
            //    x2  x1   x2  x1  x2       x1  x2   x1  x2        Fatores de multiplicação
            //     |   |    |    |    |        |     |    |    |    
            //  =18  =9 =18 =9 =14      =7  =14  =2  =2

            //    Soma dos produtos:   X = (1+8) + 9 + (1+8) + 9 + (1+4) + 7 + (1+4) + 2 + 2 = 57
            //    Divisão:             Y = 57/10 = 5,7 = 50 + 7/10
            //    Resto:               R = 7 
            //    Dígito verificador:  DV = 10 - R = 10 - 7 = 3

            //Logo, o dígito verificador para o primeiro campo do exemplo acima é 3.

        }

        private bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        private int SomaAlgarismos(int num)
        {
            int soma = 0;
            while (num > 0)
            {
                soma += num % 10;
                num /= 10;
            }
            return soma;
        }

        protected string AddWithVerificator(string str1, string str2)
        {
            string retorno = "";

            retorno = str1 + str2 + CalculaDigitoVerificador(str1 + str2).ToString();

            return retorno;
        }


    }
}
