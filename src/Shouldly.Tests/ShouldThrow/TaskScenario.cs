namespace Shouldly.Tests.ShouldThrow;

public class TaskScenario
{
    [Fact]
    public void TaskScenarioShouldFail()
    {
        var task = Task.Run(() => { }, TestContext.Current.CancellationToken);

        Verify.ShouldFail(() =>
                task.ShouldThrow<InvalidOperationException>("Some additional context"),

            errorWithSource:
            """
            Task `task`
                should throw
            System.InvalidOperationException
                but did not

            Additional Info:
                Some additional context
            """,

            errorWithoutSource:
            """
            Task
                should throw
            System.InvalidOperationException
                but did not

            Additional Info:
                Some additional context
            """);
    }

    [Fact]
    public void TaskScenarioShouldFail_ExceptionTypePassedIn()
    {
        var task = Task.Run(() => { }, TestContext.Current.CancellationToken);

        Verify.ShouldFail(() =>
                task.ShouldThrow("Some additional context", typeof(InvalidOperationException)),

            errorWithSource:
            """
            Task `task`
                should throw
            System.InvalidOperationException
                but did not

            Additional Info:
                Some additional context
            """,

            errorWithoutSource:
            """
            Task
                should throw
            System.InvalidOperationException
                but did not

            Additional Info:
                Some additional context
            """);
    }

    [Fact]
    public void ShouldPass()
    {
        var task = Task.Run(() => throw new InvalidOperationException(), TestContext.Current.CancellationToken);

        var ex = task.ShouldThrow<InvalidOperationException>();

        ex.ShouldNotBe(null);
        ex.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public void ShouldPass_ExceptionTypePassedIn()
    {
        var task = Task.Run(() => throw new InvalidOperationException(), TestContext.Current.CancellationToken);

        var ex = task.ShouldThrow(typeof(InvalidOperationException));

        ex.ShouldNotBe(null);
        ex.ShouldBeOfType<InvalidOperationException>();
    }
}