using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTestsRAS
{
    [TestClass]
    public class ExampleTest
    {
        //method to test a single functionality 
        [TestMethod]
        public void TestCreate()
        {
            //create class to be tested
            TestExample test = new TestExample();
            //use the methods of Assert to test the methods from the class
            Assert.IsTrue(test.create(2), "test create failed");

        }
        [TestMethod]
        public void TestCorrectValue()
        {
            TestExample test = new TestExample();
            Assert.Equals(test.getValue(), 35);
            Assert.IsTrue(test.create(35), "test create failed");

        }
    }
}
//created class for testing
//normally the classes from the RunApproachStatistics will be used
namespace UnitTestsRAS
{
    public class TestExample
    {
        private int value;
        public bool create(int value)
        {
            this.value = value;
            if(value != null)
            {
                return true;
            }
            return false;
        }
        public int getValue()
        {
            return value;
        }
    }
}
