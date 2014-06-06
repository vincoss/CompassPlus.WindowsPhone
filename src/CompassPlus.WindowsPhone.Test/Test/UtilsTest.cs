using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace CompassPlus.Test
{
    [TestFixture]
    public class UtilsTest
    {
        [Test]
        public void DoubleEquals_True_Test()
        {
            // Arrange
            const double diff = 0.100;
            double left =  2.120;
            double right = 2.332;

            // Act
            var result = Utils.DoubleEquals(left, right, diff);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void DoubleEquals_False_Test()
        {
            // Arrange
            const double diff = 0.100;
            double left = 2.120;
            double right = 2.333;

            // Act
            var result = Utils.DoubleEquals(left, right, diff);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void DoubleEquals_NegativeOne_Test()
        {
            // Arrange
            const double diff = 0.100;
            double left = 0.100;
            double right = -0.100;

            // Act
            var result = Utils.DoubleEquals(left, right, diff);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void DoubleEquals_NegativeBoth_Test()
        {
            // Arrange
            const double diff = 0.100;
            double left = -2.120;
            double right = -2.332;

            // Act
            var result = Utils.DoubleEquals(left, right, diff);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void DoubleEquals_ThrowsIfDiffBelowZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utils.DoubleEquals(1, 1, -1));
        }
    }
}
