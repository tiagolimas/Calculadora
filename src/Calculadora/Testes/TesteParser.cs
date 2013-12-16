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
            var tokens = t.Tokenize("1371/(-3+(-2+3*5+10)*20)");
            Assert.AreEqual("3", tokens);

            //Assert.AreEqual(4, tokens.Count);
            //Assert.AreEqual("a", tokens[0]);
        }

        [Test]
        public void TesteTokenize1()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("(2+3*5+10)");
            Assert.AreEqual("27", tokens);
        }

        [Test]
        public void TesteTokenize2()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("3+(2+3*5+10)"); 
            Assert.AreEqual("30", tokens);
        }


        [Test]
        public void TesteTokenize3()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("15+7+(12+13)");            //OK
            Assert.AreEqual("47", tokens);
        }

        [Test]
        public void TesteTokenize4()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("10+(3+(2+3*5+10)+20)");    //OK
            Assert.AreEqual("60", tokens);
        }

        [Test]
        public void TesteTokenize5()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("10+(3+(2+3*5+10)+20)");    //OK
            Assert.AreEqual("60", tokens);
        }

        [Test]
        public void TesteTokenize6()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("15+7+(12-13)");            //OK
            Assert.AreEqual("21", tokens);
        }


        [Test]
        public void TesteTokenize7()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("10+(3+(2+3*5+10)-20)");    //OK
            Assert.AreEqual("20", tokens);
        }

        [Test]
        public void TesteTokenize8()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("10+(3+(-2+3*5+10)-20)");   //OK
            Assert.AreEqual("16", tokens);
        }

        [Test]
        public void TesteTokenize9()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("10+(-3+(-2+3*5+10)-20)");  //OK
            Assert.AreEqual("10", tokens);
        }

        [Test]
        public void TesteTokenize10()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("-10+(-3+(-2+3*5+10)-20)"); //OK
            Assert.AreEqual("-10", tokens);
        }

        [Test]
        public void TesteTokenize11()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("-10+(-3+(-2+3*5+10)*20)"); //OK
            Assert.AreEqual("447", tokens);
        }

        [Test]
        public void TesteTokenize12()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("1371/(-3+(-2+3*5+10)*20)");//OK
            Assert.AreEqual("3", tokens);
        }

        [Test]
        public void TesteTokenize13()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("1+(-3+(2+3*5+10)*2000)");
            Assert.AreEqual("53998", tokens);
        }

        [Test]
        public void TesteTokenize14()
        {
            var t = new Tokenizer();
            var tokens = t.Tokenize("2+2");
            Assert.AreEqual("4", tokens);
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
