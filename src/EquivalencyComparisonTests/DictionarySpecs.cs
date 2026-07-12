using Xunit;

namespace EquivalencyComparisonTests;

/// <summary>
/// Dictionary comparison semantics, mirroring FluentAssertions.Equivalency.Specs/DictionarySpecs.
/// FluentAssertions matches dictionaries by key; Shouldly enumerates them as sequences of
/// KeyValuePair structs, which makes it sensitive to insertion order and — because KeyValuePair
/// is a value type compared via Equals — unable to see into reference-typed values (shouldly#767,
/// shouldly#1077).
/// </summary>
public class DictionarySpecs
{
    [Fact]
    public void Dictionaries_with_same_pairs_inserted_in_same_order_are_equivalent()
    {
        var actual = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var expected = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Compare.Run(actual, expected).ShouldAgree(Outcome.Pass);
    }

    [Fact]
    public void Dictionaries_with_same_pairs_inserted_in_different_order()
    {
        var actual = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var expected = new Dictionary<string, int> { ["b"] = 2, ["a"] = 1 };

        Compare.Run(actual, expected).ShouldDiverge(
            shouldly: Outcome.Fail,
            fluentAssertions: Outcome.Pass,
            because: "FluentAssertions matches dictionaries by key; Shouldly compares them as ordered sequences of pairs");
    }

    [Fact]
    public void Dictionaries_with_a_differing_value_are_not_equivalent()
    {
        var actual = new Dictionary<string, int> { ["a"] = 1 };
        var expected = new Dictionary<string, int> { ["a"] = 2 };

        Compare.Run(actual, expected).ShouldAgree(Outcome.Fail);
    }

    [Fact]
    public void Dictionaries_with_a_missing_key_are_not_equivalent()
    {
        var actual = new Dictionary<string, int> { ["a"] = 1 };
        var expected = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Compare.Run(actual, expected).ShouldAgree(Outcome.Fail);
    }

    [Fact]
    public void Dictionaries_with_reference_typed_values_that_are_equal_but_not_the_same_instance()
    {
        var actual = new Dictionary<string, Person> { ["a"] = new() { Name = "John", Age = 30 } };
        var expected = new Dictionary<string, Person> { ["a"] = new() { Name = "John", Age = 30 } };

        Compare.Run(actual, expected).ShouldDiverge(
            shouldly: Outcome.Fail,
            fluentAssertions: Outcome.Pass,
            because: "Shouldly compares KeyValuePair structs with Equals, which reference-compares the Person values instead of recursing into them (shouldly#1077)");
    }

    [Fact]
    public void Dictionaries_with_collection_values_that_are_equal_but_not_the_same_instance()
    {
        var actual = new Dictionary<string, List<int>> { ["a"] = [1, 2] };
        var expected = new Dictionary<string, List<int>> { ["a"] = [1, 2] };

        Compare.Run(actual, expected).ShouldDiverge(
            shouldly: Outcome.Fail,
            fluentAssertions: Outcome.Pass,
            because: "Shouldly compares KeyValuePair structs with Equals, which reference-compares the List values instead of recursing into them (shouldly#767)");
    }
}
