using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoIPTE
{
    static class BoletoHandler
    {

        static public Boleto ConstructBoleto(string codigoBarras)
        {
            Boleto retorno;
            if (codigoBarras.Substring(0, 1) == "8" && codigoBarras.Length == 44)
            {
                retorno = new BoletoConcessionaria(codigoBarras);
            }
            else if (codigoBarras.Length == 44)
            {
                retorno = new BoletoBancario(codigoBarras);
            }
            else
            {
                throw new Exception("O codigo de barras inserido nao é valido");
            }
            return retorno;
        }
    }
}
