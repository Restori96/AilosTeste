using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int NumeroConta { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numeroConta, string titular, double saldo = 0)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            Saldo = saldo;
        }

        public void Deposito(double valor)
        {
            Saldo += valor;
        }

        public void Saque(double valor)
        {
            const double taxaSaque = 3.50;
            Saldo -= valor + taxaSaque;
            
          
        }
        public override string ToString()
        {
            return $"Conta {NumeroConta}, Titular: {Titular}, Saldo: $ {Saldo.ToString("0.00")}";
        }

    }

}
