
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests
{
    public class PunchTests
    {
        private readonly IPunchUtilities _punchUtilities;

        public PunchTests()
        {
            _punchUtilities = new PunchUtilities();
        }

        [Fact]
        public void isValidStatus_pending_true()
        {
            Assert.True(_punchUtilities.IsValidStatus("pENdINg"));
        }

        [Fact]
        public void isValidStatus_approved_true()
        {
            Assert.True(_punchUtilities.IsValidStatus("APPROved"));
        }

        [Fact]
        public void isValidStatus_rejected_true()
        {
            Assert.True(_punchUtilities.IsValidStatus("REjEctED"));
        }

        [Fact]
        public void isValidStatus_completed_false()
        {
            Assert.False(_punchUtilities.IsValidStatus("coMPLeted"));
        }
    }
}