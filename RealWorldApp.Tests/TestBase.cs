using System.Threading.Tasks;
using NUnit.Framework;

namespace RealWorldApp.IntegrationTests
{
    [TestFixture]
    public class TestBase
    {
        [OneTimeSetUp]
        public async Task BeforeAllTest()
        {
            await SliceFixture.ResetCheckpoint();
        }
    }
}