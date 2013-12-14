using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Calculadora.Expressoes;
using System.Collections;


namespace Calculadora.Testes
{
    [TestFixture]
    public class TesteExpressao
    {
        [Test]
        public void TesteInicializacao()
        {
            var expressao = new Expressao(null, Operador.Soma, null);
            Assert.IsNotNull(expressao);
        }

        [Test]
        public void TestSoma()
        {
            var expressao = new Expressao(new ExpressaoNumerica(2d), Operador.Soma, new ExpressaoNumerica(2d));
            Assert.AreEqual(4, expressao.Avaliar());
        }

        [Test]
        public void TestSoma2()
        {
            var e1 = new ExpressaoNumerica(2d);
            var e2 = new ExpressaoNumerica(3d);

            var expressao = new Expressao(e1, Operador.Soma, e2);
            Assert.AreEqual(5, expressao.Avaliar());
        }

        [Test]
        public void TestSubtracao()
        {
            var expressao = new Expressao(new ExpressaoNumerica(5d), Operador.Subtracao, new ExpressaoNumerica(2d));
            Assert.AreEqual(3, expressao.Avaliar());
        }
        
        [Test]
        public void TestSubtracao2()
        {
            var expressao = new Expressao(new ExpressaoNumerica(2d), Operador.Subtracao, new ExpressaoNumerica(2d));
            Assert.AreEqual(0, expressao.Avaliar());
        }
        [Test]
        public void TestMultiplicacao()
        {
            var expressao = new Expressao(new ExpressaoNumerica(2d), Operador.Multiplicacao, new ExpressaoNumerica(2d));
            Assert.AreEqual(4, expressao.Avaliar());
        }
        
        [Test]
        public void TestDivisao()
        {
            var expressao = new Expressao(new ExpressaoNumerica(2d), Operador.Divisao, new ExpressaoNumerica(2d));
            Assert.AreEqual(1, expressao.Avaliar());
        }

        [Test]
        public void TestMultiplasSomas()
        {
            // 2 + 3 + 2
            var expNo1 = new ExpressaoNumerica(2);
            var expNo2 = new ExpressaoNumerica(3);
            IExpressao expressaoEsquerda = new Expressao(expNo1, Operador.Soma, expNo2);
            var expNo3 = new ExpressaoNumerica(2);

            var expRaiz = new Expressao(expressaoEsquerda, Operador.Soma, expNo3);
            var resultado = expRaiz.Avaliar();
            Assert.AreEqual(7, resultado);
        }

        [Test]
        public void TestMultiplasExpressoes()
        { 
            // -2 + 3 * (10 -1 + (2x1))
            IExpressao doisVezesUm = new Expressao(new ExpressaoNumerica(2), Operador.Multiplicacao, new ExpressaoNumerica(1));
            IExpressao dezMenosUm = new Expressao(new ExpressaoNumerica(10), Operador.Subtracao, new ExpressaoNumerica(1));
            IExpressao expParenteses = new Expressao(dezMenosUm, Operador.Soma, doisVezesUm);
            IExpressao tresVezesParentes = new Expressao(new ExpressaoNumerica(3), Operador.Multiplicacao, expParenteses);
            IExpressao menosDoisMaisTresParenteses = new Expressao(new ExpressaoNumerica(-2), Operador.Soma, tresVezesParentes);
            var resultado = menosDoisMaisTresParenteses.Avaliar();

            Assert.AreEqual(31, resultado);
        
        }
    }
}
