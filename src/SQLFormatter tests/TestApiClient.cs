using Klaasie.Sf;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLFormatter_tests
{
    [TestClass]
    public class TestApiClient
    {
        [TestMethod]
        public void TestApi()
        {
            string sql = "select * from foo";

            Task<Response> response = ApiClient.Format(sql);

            Assert.AreEqual("select *\nfrom foo", response.Result.Result);
        }
    }
}