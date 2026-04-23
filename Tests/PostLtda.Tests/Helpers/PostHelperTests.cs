using Business.Helpers;
using Xunit;

namespace PostLtda.Tests.Helpers
{
    public class PostHelperTests
    {
        [Fact]
        public void NormalizeBody_ReturnsUnchanged_WhenAtMost20Characters()
        {
            var shortText = new string('a', 20);
            Assert.Equal(shortText, PostHelper.NormalizeBody(shortText));
        }

        [Fact]
        public void NormalizeBody_TruncatesTo97PlusEllipsis_WhenLongerThan20()
        {
            var longText = new string('b', 100);
            var result = PostHelper.NormalizeBody(longText);
            Assert.Equal(100, result.Length);
            Assert.EndsWith("...", result);
            Assert.StartsWith(new string('b', 97), result);
        }

        [Theory]
        [InlineData(1, "x", "Farándula")]
        [InlineData(2, "x", "Política")]
        [InlineData(3, "x", "Futbol")]
        [InlineData(99, "Manual", "Manual")]
        [InlineData(0, null, "")]
        public void ResolveCategory_MapsTypeOrKeepsRequested(int type, string requested, string expected)
        {
            Assert.Equal(expected, PostHelper.ResolveCategory(type, requested));
        }
    }
}
