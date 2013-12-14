using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace Calculadora.Expressoes
{
    public class Parser
    {
        public IExpressao Parse(string exp)
        {
            return new ExpressaoNumerica(2);
        }

    }

    public enum TokenType
    { 
        Numerico,
        Operador,
        Agrupamento
    }


    public class Tokenizer
    {
        private char soma = '+';
        private char sub = '-';
        private char mult = '*';
        private char div = '/';
        private char abertura = '(';
        private char fechamento = ')';

        public bool Validar(string expressao)
        {
            var tokens = this.Tokenize(expressao);
            var saida = true;

            foreach (char t in tokens)
            {
                double resultado = -1;
                var resultadoConversao = Double.TryParse(Convert.ToString(t), out resultado);
                if (!resultadoConversao && (t != soma && t != sub && t != mult && t != div && t != abertura && t != fechamento))
                {
                    saida = false;
                    break;
                }
            }

            return saida;
        }

        
        public string Tokenize(string expressao)
        {
            //var lista = expressao.Split(' ').ToList();
            string resultado = string.Empty;
           
            if(expressao.Length != 0)
            {

                resultado = ResolveParenteses(expressao);


            }
            
            return resultado;
        }

        /// <summary>
        /// Busca a expressão dentro dos parênteses
        /// </summary>
        /// <param name="expressao"></param>
        /// <returns></returns>
        public string ResolveParenteses(string expressao)
        {
            string expInterna = string.Empty;
            int i;

            for (i = 0; i < expressao.Length; i++)
            {
                if (expressao[i] == ')')
                {
                    //empilha voltando a lista até a chave de abertura
                    //for (int j = 0; j >= i; j++)
                    for (int j = i; j >= 0; j--)
                    {
                        expInterna += Convert.ToString(expressao[j]);

                        if (expressao[j] == '(')
                        {
                            
                            expInterna = ResolveExpInterna(expInterna);
                            //expressao = expressao.Remove(j, 3).Trim();
                            //expressao = expressao.Insert(j, expInterna).Trim();
                            //expInterna = expressao;
                            j = -1;
                        }

                        //expressao.Remove(j);
                    }
                }
            }


            if (VerificaOperacoeMultDivisao(expInterna))
            {
                //recursividade
                ResolveExpInterna(expInterna);
            }
            else
            {
                //recursividade
                expInterna = ResolveSomaSubtracao(expInterna);
            }


            return expInterna;
        }

        /// <summary>
        /// Resolve a expressão Interna
        /// </summary>
        /// <param name="expInterna"></param>
        /// <returns></returns>
        public string ResolveExpInterna(string expInterna)
        {
            string resultado = string.Empty;

            for (int i = 0; i < expInterna.Length; i++)
            {
                if (expInterna[i] == '*')
                {
                    string teste2 = expInterna.Substring(i, 1).Trim();
                    string teste3 = expInterna.Substring(i - 1, 1).Trim();

                    int esquerda = Convert.ToInt32(expInterna.Substring(i - 1, 1).Trim());
                    int direita  = Convert.ToInt32(expInterna.Substring(i + 1, 1).Trim());
                    resultado =  Convert.ToString(esquerda * direita);
                    expInterna = expInterna.Remove(i - 1, 3).Trim();
                    expInterna = expInterna.Insert(i - 1, resultado).Trim();
                    expInterna = RemoveParentes(expInterna);
                    //recursividade
                    expInterna = ResolveExpInterna(expInterna);
                }
            }

            if (VerificaOperacoeMultDivisao(expInterna))
            {
                //recursividade
                ResolveExpInterna(expInterna);
            }
            else
            {
                //recursividade
                expInterna = ResolveSomaSubtracao(expInterna);
            }

            return expInterna;
        }


        /// <summary>
        /// Retorna resultados de Soma e Subtração da expressão interna
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string ResolveSomaSubtracao(string expInterna)
        {
            string resultado = string.Empty;

            for (int i = 0; i < expInterna.Length; i++)
            {
                if (expInterna[i] == '+')
                {
                    int esquerda = Convert.ToInt32(expInterna.Substring(i - 1, 1).Trim());
                    int direita = Convert.ToInt32(expInterna.Substring(i + 1, 1).Trim());
                    resultado = Convert.ToString(esquerda + direita);
                    expInterna = expInterna.Remove(i - 1, 3);
                    expInterna = expInterna.Insert(i - 1, resultado);
                    
                    //recursividade
                    ResolveSomaSubtracao(expInterna);
                }
                else if (expInterna[i] == '-')
                {
                    int esquerda = Convert.ToInt32(expInterna.Substring(i - 1, 1).Trim());
                    int direita = Convert.ToInt32(expInterna.Substring(i + 1, 1).Trim());
                    resultado = Convert.ToString(esquerda - direita);
                    expInterna = expInterna.Remove(i - 1, 3);
                    expInterna = expInterna.Insert(i - 1, resultado);
                   
                    if (VerificaOperacoeSomaSubtracao(expInterna))
                    {
                        //recursividade
                        ResolveSomaSubtracao(expInterna);
                    }
                }
            }

            return expInterna;
        }


        /// <summary>
        /// //Verifica se ainda resta operções a serem feitas
        /// </summary>
        /// <param name="expInterna"></param>
        /// <returns></returns>
        public bool VerificaOperacoeSomaSubtracao(string expInterna)
        {
            for (int i = 0; i < expInterna.Length; i++)
            {
                if (expInterna[i] == '+' || expInterna[i] == '-')
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// //Verifica se ainda resta operções a serem feitas em Multiplicação ou Divisao
        /// </summary>
        /// <param name="expInterna"></param>
        /// <returns></returns>
        public bool VerificaOperacoeMultDivisao(string expInterna)
        {
            for (int i = 0; i < expInterna.Length; i++)
            {
                if (expInterna[i] == '*' || expInterna[i] == '/')
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Retorna expressão sem parenteses
        /// </summary>
        /// <param name="expInterna"></param>
        /// <returns></returns>
        public string RemoveParentes(string expInterna)
        {
            expInterna = expInterna.Remove(0,1);
            expInterna = expInterna.Remove(expInterna.Length - 1, 1);

            return expInterna;
        }
    }
}














