using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Calculadora.Expressoes;

namespace Calculadora.Testes
{
    [TestFixture]
    public class TestTokenizer
    {
        [Test]
        public void TesteInicializacao()
        {
            var t = new Tokenizer();
            Assert.IsNotNull(t);
        }

        [Test]
        public void TesteTokenize()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("(2+3*5)");

            Assert.AreEqual("17", tokens);

            //Assert.AreEqual(4, tokens.Count);
            //Assert.AreEqual("a", tokens[0]);
            //Assert.AreEqual("b", tokens[1]);
            //Assert.AreEqual("c", tokens[2]);
            //Assert.AreEqual("d", tokens[3]);
        }

        //[Test]
        //public void TesteValidacaoValido()
        //{
        //    string exp = "2 + 3 + 4";
        //    var t = new Tokenizer();

        //    Assert.IsTrue(t.Validar(exp));
        //}

        //[Test]
        //public void TesteValidacaoValidoComParenteses()
        //{
        //    string exp = "2 + (3 + 4)";
        //    var t = new Tokenizer();

        //    Assert.IsTrue(t.Validar(exp));
        //}
    }

    [TestFixture]
    public class TesteParser
    {
        [Test]
        public void TestInicializacao()
        {
            var p = new Parser();
            Assert.IsNotNull(p);
        }

        [Test]
        public void TesteParserParseia()
        {
            string expressao = "2 + 3";
            var p = new Parser();
            IExpressao exp = p.Parse(expressao);

            Assert.IsNotNull(exp);

            Assert.AreEqual(5.0, exp.Avaliar());
        }
    }
}
