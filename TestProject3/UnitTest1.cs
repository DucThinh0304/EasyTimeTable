using EasyTimeTable;
namespace TestProject3
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMD5Hash1()
        {
            string expected = "D41D8CD98F00B204E9800998ECF8427E", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestMD5Hash2()
        {
            string expected = "518ED29525738CEBDAC49C49E60EA9D3", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("@");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestMD5Hash3()
        {
            string expected = "ECCBC87E4B5CE2FE28308FD9F2A7BAF3", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("3");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestMD5Hash4()
        {
            string expected = "E8BABFBB9799BADD295B08666B32A4D3", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("admin 1$");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestMD5Hash5()
        {
            string expected = "900150983CD24FB0D6963F7D28E17F72", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("abc");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestMD5Hash6()
        {
            string expected = "2711DD730059E9AF47D977B547AD49D0", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("31&");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestMD5Hash7()
        {
            string expected = "131F19734A84E3E2C6613FB5F4BB9A48", actual;
            actual = EasyTimeTable.Converter.Converter.CreateMD5("ab#");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestStartTime1()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime2()
        {
            TimeOnly expected = new TimeOnly(7, 30, 0), actual;
            actual = EasyTimeTable.Converter.Converter.StartTime("1");
            Assert.AreEqual(expected, actual);
            
        }
        [Test]
        public void TestStartTime3()
        {
            TimeOnly expected = new TimeOnly(8, 15, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("2");
                Assert.AreEqual(expected, actual);

            }
            catch (Exception e)
            {
                Assert.Fail();

            }
        }
        [Test]
        public void TestStartTime4()
        {
            TimeOnly expected = new TimeOnly(9, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("3");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime5()
        {
            TimeOnly expected = new TimeOnly(10, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("4");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime6()
        {
            TimeOnly expected = new TimeOnly(10, 45, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("5");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime7()
        {
            TimeOnly expected = new TimeOnly(13, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("6");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime8()
        {
            TimeOnly expected = new TimeOnly(13, 45, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("7");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime9()
        {
            TimeOnly expected = new TimeOnly(14, 30, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("8");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime10()
        {
            TimeOnly expected = new TimeOnly(15, 30, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("9");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime11()
        {
            TimeOnly expected = new TimeOnly(16, 15, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("0");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime12()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("a");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime13()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("1a");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime14()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("abc1");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestStartTime15()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.StartTime("abc#123");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        public void TestEndTime1()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime2()
        {
            TimeOnly expected = new TimeOnly(8, 15, 0), actual;
            actual = EasyTimeTable.Converter.Converter.EndTime("1");
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestEndTime3()
        {
            TimeOnly expected = new TimeOnly(9, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("2");
                Assert.AreEqual(expected, actual);

            }
            catch (Exception e)
            {
                Assert.Fail();

            }
        }
        [Test]
        public void TestEndTime4()
        {
            TimeOnly expected = new TimeOnly(9, 45, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("3");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime5()
        {
            TimeOnly expected = new TimeOnly(10, 45, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("4");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime6()
        {
            TimeOnly expected = new TimeOnly(11, 30, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("5");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime7()
        {
            TimeOnly expected = new TimeOnly(13, 45, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("6");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime8()
        {
            TimeOnly expected = new TimeOnly(14, 30, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("7");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime9()
        {
            TimeOnly expected = new TimeOnly(15, 15, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("8");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime10()
        {
            TimeOnly expected = new TimeOnly(16, 15, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("9");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime11()
        {
            TimeOnly expected = new TimeOnly(17, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("0");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime12()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("a");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime13()
        {
            TimeOnly expected = new TimeOnly(11, 30, 0), actual;
                actual = EasyTimeTable.Converter.Converter.EndTime("345");
                Assert.AreEqual(expected, actual);
           
        }
        [Test]
        public void TestEndTime14()
        {
            TimeOnly expected = new TimeOnly(10, 45, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("1234");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestEndTime15()
        {
            TimeOnly expected = new TimeOnly(0, 0, 0), actual;
            try
            {
                actual = EasyTimeTable.Converter.Converter.EndTime("abc#123");
                Assert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [Test]
        public void TestCompare1()
        {
            bool expected = false, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("123", "123", 2, 2);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare2()
        {
            bool expected = false, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("123", "12345", 2, 2);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare3()
        {
            bool expected = true, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("456", "123", 2, 2);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare4()
        {
            bool expected = true, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("123", "123", 2, 4);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare5()
        {
            bool expected = true, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("", "", 2, 4);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare6()
        {
            bool expected = true, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("", "123", 2, 4);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare7()
        {
            bool expected = true, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("", "", 2, 2);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestCompare8()
        {
            bool expected = true, actual;
            actual = EasyTimeTable.Converter.Converter.Compare("890", "12345", 2, 4);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestComonChars1()
        {
            int expected = 2, actual;
            actual = EasyTimeTable.Converter.Converter.CommonChars("abcd", "aabb");
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestComonChars2()
        {
            int expected = 4, actual;
            actual = EasyTimeTable.Converter.Converter.CommonChars("abcd", "aaaa");
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestComonChars3()
        {
            int expected = 4, actual;
            actual = EasyTimeTable.Converter.Converter.CommonChars("abcd", "abcd");
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestComonChars4()
        {
            int expected = 1, actual;
            actual = EasyTimeTable.Converter.Converter.CommonChars("a", "abcd");
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestComonChars5()
        {
            int expected = 0, actual;
            actual = EasyTimeTable.Converter.Converter.CommonChars("", "aaaa");
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void TestComonChars6()
        {
            int expected = 0, actual;
            actual = EasyTimeTable.Converter.Converter.CommonChars("", "");
            Assert.AreEqual(expected, actual);

        }


    }
}