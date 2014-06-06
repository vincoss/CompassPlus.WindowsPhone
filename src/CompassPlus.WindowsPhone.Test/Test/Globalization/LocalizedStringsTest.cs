using CompassPlus.Globalization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CompassPlus.Test.Globalization
{
    [TestFixture]
    public class LocalizedStringsTest
    {
        [Test]
        public void WithLanguate_Test()
        {
            // Arrange
            var strings = new LocalizedStrings();

            // Act
            strings.WithLanguate("ko-KR");

            // Assert
            Assert.AreEqual(Thread.CurrentThread.CurrentUICulture.Name, "ko-KR");
        }
    }
}