public class EnumerableShouldBeEmptyExamples
{
    ITestOutputHelper _testOutputHelper;

    public EnumerableShouldBeEmptyExamples(ITestOutputHelper testOutputHelper) =>
        _testOutputHelper = testOutputHelper;

    [Fact]
    public void ShouldBeEmpty()
    {
        DocExampleWriter.Document(
            () =>
            {
                var homer = new Person { Name = "Homer" };
                var powerPlantOnTheWeekend = new List<Person> { homer };
                powerPlantOnTheWeekend.ShouldBeEmpty();
            },
            _testOutputHelper);
    }

    [Fact]
    public void ShouldNotBeEmpty()
    {
        DocExampleWriter.Document(
            () =>
            {
                var moesTavernOnTheWeekend = new List<Person>();
                moesTavernOnTheWeekend.ShouldNotBeEmpty();
            }
            , _testOutputHelper);
    }
}