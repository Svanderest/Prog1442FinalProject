
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prog1442_FinalProject.Utils;
using Windows.ApplicationModel.Activation;

namespace CardTest.cs
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UnicityTest()
        {
            //Arrange
            var Target = new Round();

            //Act
            bool Result = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                    Result = Result || Target.Hands[i].Any(ic => Target.Hands[j].Any(jc => jc.Equals(ic)));
            }

            //Assert
            Assert.IsFalse(Result);
        }

        [TestMethod]
        public void HandSizeTest()
        {
            //Arrange
            var Target = new Round();

            //Act
            bool Result = true;
            foreach (List<Card> hand in Target.Hands)
                Result = Result && hand.Count == 13;

            //Assert
            Assert.IsTrue(Result);
        }

        [TestMethod]
        public void DefaultWinnerTest()
        {
            //Arrange
            var Target = new Trick();

            //Act
            int expected = -1;
            int actual = Target.WinningPlayer;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SameSuitWinnerTest()
        {
            //Arrange
            var Target = new Trick();
            Target.Cards = new Card[]
            {
                new Card(Suit.Clubs, CardValue.Two),
                new Card(Suit.Clubs, CardValue.Ace),
                new Card(Suit.Clubs, CardValue.Eight),
                new Card(Suit.Clubs, CardValue.Five)
            };

            //Act
            int expected = 1;
            int actual = Target.WinningPlayer;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DiffferentSuitWinnerTest()
        {
            //Arrange
            var Target = new Trick();
            Target.Cards = new Card[]
            {
                new Card(Suit.Clubs, CardValue.Two),
                new Card(Suit.Diamonds, CardValue.Ace),
                new Card(Suit.Clubs, CardValue.Eight),
                new Card(Suit.Clubs, CardValue.Five)
            };

            //Act
            int expected = 2;
            int actual = Target.WinningPlayer;

            //Assert
            Assert.AreEqual(expected, actual);
        }        
    }
}
