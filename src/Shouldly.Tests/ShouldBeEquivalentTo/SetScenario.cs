namespace Shouldly.Tests.ShouldBeEquivalentTo;

public class SetScenario
{
    [Fact]
    public void ShouldPassWhenSetsMatch()
    {
        var subject = new HashSet<int> { 1, 2, 3 };

        subject.ShouldBeEquivalentTo(new HashSet<int> { 1, 2, 3 });
    }

    [Fact]
    public void ShouldPassWhenSetsMatchInDifferentOrder()
    {
        var subject = new HashSet<int> { 1, 2, 3 };

        subject.ShouldBeEquivalentTo(new HashSet<int> { 3, 2, 1 });
    }

    [Fact]
    public void ShouldFailWhenSetIsTooShort()
    {
        var subject = new HashSet<int> { 1, 2, 3, 4 };

        Verify.ShouldFail(() =>
            subject.ShouldBeEquivalentTo(new HashSet<int> { 1, 2, 3, 4, 5 }, "Some additional context"));
    }

    [Fact]
    public void ShouldFailWhenSetIsTooLong()
    {
        var subject = new HashSet<int> { 1, 2, 3, 4, 5 };

        Verify.ShouldFail(() =>
            subject.ShouldBeEquivalentTo(new HashSet<int> { 1, 2, 3, 4 }, "Some additional context"));
    }

    [Fact]
    public void ShouldFailWhenSetsDoNotMatch()
    {
        var subject = new HashSet<int> { 1, 2, 6, 4, 3 };

        Verify.ShouldFail(() =>
            subject.ShouldBeEquivalentTo(new HashSet<int> { 1, 2, 3, 4, 5 }, "Some additional context"));
    }
}
