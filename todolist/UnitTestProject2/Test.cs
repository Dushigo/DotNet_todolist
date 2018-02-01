using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2
{
    [TestClass]
    public class Test
    {
        private string ConvertDateToString(DatePicker date)
        {
            return (date.Date.ToString());
        }
        private string ConvertTimeToString(TimePicker time)
        {
            return (time.Time.ToString());
        }
        [TestMethod]
        public void WriteMethod()
        {
        }
    }
}
