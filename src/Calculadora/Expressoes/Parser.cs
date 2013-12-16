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
                    for (int j = i; j >= 0; j--)
                    {
                        expInterna += Convert.ToString(expressao[j]);

                        if (expressao[j] == '(')
                        {
                            string expressaoOrdenada = reordena(expInterna);
                            expInterna = ResolveExpInterna(expressaoOrdenada.Trim());
                            expressao = expressao.Replace(expressaoOrdenada.Trim(), expInterna.Trim());
                            expressao = expressao.Replace("+-", "-").Replace("-+","-");
                            expInterna = expressao;
                            j = -1;
                        }
                    }
                }
            }

            if (VerificaOperacoeMultDivisao(expInterna))
            {
                
                expInterna = ResolveParenteses(expInterna);
                expInterna = ResolveExpInterna(expInterna);
                if (expInterna == string.Empty)
                {
                    expInterna = ResolveExpInterna(expressao);      
                }
      
            }
            else
            {
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

            expInterna = RemoveParentes(expInterna);

            for (int i = 0; i < expInterna.Trim().Length; i++)
            {
                if (expInterna[i] == mult)
                {
                    string operacao = PegaOperacao(expInterna);

                    int extensao = operacao.Trim().Length - 1;
                    i = operacao.IndexOf(mult);
                    extensao = extensao - i;
                    int direita = Convert.ToInt32(operacao.Substring(i + 1, extensao));
                    int esquerda = Convert.ToInt32(operacao.Substring(0, i));
                    resultado =  Convert.ToString(esquerda * direita);

                    expInterna = expInterna.Replace(operacao.Trim(), resultado.Trim());

                    expInterna = ResolveExpInterna(expInterna);
                } 
                else if (expInterna[i] == div)
                {
                    string operacao = PegaOperacao(expInterna);
                    int extensao = operacao.Trim().Length - 1;
                    i = operacao.IndexOf(div);
                    extensao = extensao - i;
                    int direita = Convert.ToInt32(operacao.Substring(i + 1, extensao)); ;
                    int esquerda = Convert.ToInt32(expInterna.Substring(0, i));
                    resultado = Convert.ToString(esquerda / direita);

                    expInterna = expInterna.Replace(operacao.Trim(), resultado.Trim());
                    expInterna = ResolveExpInterna(expInterna);
                }
            }

            expInterna =  VerificaParenteses(expInterna);

            if (VerificaOperacoeMultDivisao(expInterna))
            {
                ResolveExpInterna(expInterna);
            }
            else
            {
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

            expInterna = VerificaParenteses(expInterna);
            expInterna = RemoveParentes(expInterna);

            for (int i = 0; i < expInterna.Length; i++)
            {
                if (expInterna[i] == soma)
                {
                    string operacao = PegaOperacao(expInterna);
                    int extensao = operacao.Trim().Length - 1;
                    i = operacao.IndexOf(soma);
                    extensao = extensao - i;
                    int direita = Convert.ToInt32(operacao.Substring(i + 1, extensao));
                    int esquerda = Convert.ToInt32(operacao.Substring(0, i));
                    resultado = Convert.ToString(esquerda + direita);
                    
                    expInterna = expInterna.Replace(operacao.Trim(), resultado.Trim());
                    expInterna = ResolveSomaSubtracao(expInterna);
                }
                else if (expInterna[i] == sub)
                {
                    if (i > 1)
                    {
                        string operacao = PegaOperacao(expInterna);
                        int extensao = operacao.Trim().Length - 1;
                        i = operacao.IndexOf(sub);
                        extensao = extensao - i;
                        int direita = Convert.ToInt32(operacao.Substring(i + 1, extensao));
                        int esquerda = Convert.ToInt32(operacao.Substring(0, i));
                        resultado = Convert.ToString(esquerda - direita);

                        expInterna = expInterna.Replace(operacao.Trim(), resultado.Trim());
                        expInterna = ResolveSomaSubtracao(expInterna);
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
                if (expInterna[i] == soma || expInterna[i] == sub)
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
                if (expInterna[i] == mult || expInterna[i] == div)
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
            for (int i = 0; i < expInterna.Length; i++)
            {
                if (expInterna[i] == '(')
                {
                    //expInterna = expInterna.Remove(0, 1);
                    int indice = expInterna.IndexOf('(');
                    expInterna = expInterna.Remove(indice,1);
                    
                }
                else if (expInterna[i] == ')')
                {
                    //expInterna = expInterna.Remove(expInterna.Length - 1, 1);
                    int indice = expInterna.IndexOf(')');
                    expInterna = expInterna.Remove(indice,1);
                }
            }



            return expInterna;
        }

        /// <summary>
        /// Reordena a expressão
        /// </summary>
        /// <param name="expressao"></param>
        /// <returns></returns>
        public string reordena(string expressao)
        {
            string expressaoTemp = string.Empty;

            for (int i = expressao.Length - 1; i >= 0 ; i--)
            {
                expressaoTemp += expressao[i];
            }

            return expressaoTemp;
        }
        
        /// <summary>
        /// Retornará uma operação simples
        /// </summary>
        /// <param name="expInterna"></param>
        /// <returns></returns>
        public string PegaOperacao(string operacao)
        {
            for (int t = 1; t < operacao.Length; t++)
            {
                if (operacao[t] == mult)
                {
                    int indiceMult = operacao.IndexOf(mult);
                    //apaga -->
                    for (int j = t+1; j < operacao.Length; j++)
                    {
                        if (operacao[j] == soma || operacao[j] == sub || operacao[j] == mult || operacao[j] == div)
                        {
                            operacao = operacao.Remove(j, operacao.Length - j);
                        }
                    }
                    //apaga <--
                    for (int x = indiceMult; x >= 0; x--)
                    {
                        if (operacao[x] == soma || operacao[x] == sub || operacao[x] == div)
                        {
                            operacao = operacao.Remove(0, x+1);
                        }
                    }
                }
            }

            for (int t = 1; t < operacao.Length; t++)
            {
                if (operacao[t] == soma)
                {
                    int indiceSoma = operacao.IndexOf(soma);
                    //apaga -->
                    if (indiceSoma < operacao.Length - 3)
                    {
                        for (int j = t + 1; j < operacao.Length; j++)
                        {
                            if (operacao[j] == soma || operacao[j] == sub || operacao[j] == mult || operacao[j] == div)
                            {
                                operacao = operacao.Remove(j, operacao.Length - j);
                            }
                        }
                    }
                    //apaga <--
                    if (indiceSoma > 2)
                    {
                        int indiceSub = operacao.IndexOf(sub);
                        if (indiceSub != 0)
                        {
                            for (int x = indiceSoma; x > 1; x--)
                            {
                                if (operacao[x] == soma || operacao[x] == sub || operacao[x] == div)
                                {
                                    operacao = operacao.Remove(0, x + 1);
                                }
                            }
                        }
                    }
                }
            }
            for (int t = 1; t < operacao.Length; t++)
            {
                if (operacao[t] == sub)
                {
                    //int indiceSub = operacao.IndexOf(sub);
                    //apaga -->
                    if (t < operacao.Length - 3)
                    {
                        for (int j = t + 1; j < operacao.Length; j++)
                        {
                            if (operacao[j] == soma || operacao[j] == sub || operacao[j] == mult || operacao[j] == div)
                            {
                                operacao = operacao.Remove(j, operacao.Length - j);
                            }
                        }
                    }
                    //apaga <--
                    if (t > 1)
                    {
                        for (int x = t-1; x > 1; x--)
                        {
                            if (operacao[x] == soma || operacao[x] == sub || operacao[x] == div || operacao[x] == mult)
                            {
                                operacao = operacao.Remove(0, x + 1);
                            }
                        }
                    }
                }
            }
            
            return operacao;
        }


        /// <summary>
        /// Verifica se ainda restam parêntes na expressão
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public string VerificaParenteses(string exp)
        {
            for (int j = 0; j < exp.Length; j++)
            {
                if (exp[j] == '(')
                {
                   exp = ResolveParenteses(exp);
                }
            }

            return exp;
        }
    }
}














