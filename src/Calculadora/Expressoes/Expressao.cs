using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculadora.Expressoes
{
    public enum Operador
    {
        Soma = 1,
        Subtracao = 2,
        Multiplicacao = 3,
        Divisao = 4,
        Numerico = 5
    }

    public interface IExpressao
    {
        IExpressao Esquerda { get; }
        IExpressao Direita { get; }
        Operador Operador { get; }
        double Avaliar();
    }

    public class ExpressaoNumerica : IExpressao
    {
        private double _valor;

        public ExpressaoNumerica()
        {
        }

        public ExpressaoNumerica(double valor)
        {
            this._valor = valor;
        }

        public IExpressao Esquerda
        {
            get { return null; }
        }

        public IExpressao Direita
        {
            get { return null; }
        }

        public Operador Operador
        {
            get { return Operador.Numerico; }
        }

        public double Avaliar()
        {
            return _valor;
        }
    }

    public class Expressao:IExpressao
    {
        private IExpressao _esquerda;
        private IExpressao _direita;
        private Operador _operador;

        public Expressao(IExpressao esquerda, Operador operador, IExpressao direita)
        {
            this._esquerda = esquerda;
            this._operador = operador;
            this._direita = direita;
        }

        public IExpressao Esquerda
        {
            get { return _esquerda; }
        }

        public IExpressao Direita
        {
            get { return _direita; }
        }

        public Operador Operador
        {
            get { return _operador; }
        }

        public double Avaliar()
        {
            double valor = 0.0;
                        
            if(this.Operador == Operador.Soma)
            {
                valor = _esquerda.Avaliar() + _direita.Avaliar();
            }
            
            if (this.Operador == Operador.Subtracao)
            {
                valor = _esquerda.Avaliar() - _direita.Avaliar();
            }

            if (this.Operador == Operador.Multiplicacao)
            {
                return _esquerda.Avaliar() * _direita.Avaliar();
            }

            if (this.Operador == Operador.Divisao)
            {
                valor = _esquerda.Avaliar() / _direita.Avaliar();
            }

            return valor;
        }
    }
}


//Clear()
//Peek() -> Retorna o item do topo sem apagá-lo
//Push(Objeto) -> Adiciona no topo
//Pop() -> Pega e retira do topo da pilha
//count