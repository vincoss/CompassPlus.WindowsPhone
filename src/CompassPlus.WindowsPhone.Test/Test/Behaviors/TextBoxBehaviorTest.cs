using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompassPlus.Behaviors;
using NUnit.Framework;


namespace CompassPlus.Test.Behaviors
{
    [TestFixture]
    public class TextBoxBehaviorTest
    {
        [Test]
        public void Filter_Test()
        {
            // Arrange
            var text = ".H.e-l,lo.-,";

            // Act
            string actual = string.Empty;

            // Here simulate adding char at a time. Something like user is typing and filter removes wrong characters.
            for (int i = 0; i < text.Length; i++)
            {
                actual += text[i];
                actual = TextBoxBehavior.Filter(actual, ".-,");
            }

            // Assert
            Assert.AreEqual("Hello", actual);
        }

        [Test]
        public void GetSelectionStart_Test()
        {
            // Arrange
            var text = "Hello";

            // Act
            Assert.AreEqual(text.Length, TextBoxBehavior.GetSelectionStart(text));
        }
    }
}
