namespace Shouldly.Tests.ShouldBeSameAs;

public class BoxedIntScenario
{
    private readonly object _boxedInt = 1;
    private readonly object _differentBoxedInt = 1;

    [Fact]
    public void BoxedIntScenarioShouldFail()
    {
        Verify.ShouldFail(() =>
                _boxedInt.ShouldBeSameAs(_differentBoxedInt),

            errorWithSource:
            """
            _boxedInt
                should be same as
            1
                but was
            1
            """,

            errorWithoutSource:
            """
            1
                should be same as
            1
                but was not
            """);
    }

    [Fact]
    public void ShouldPass()
    {
        _boxedInt.ShouldBeSameAs(_boxedInt);
    }
}