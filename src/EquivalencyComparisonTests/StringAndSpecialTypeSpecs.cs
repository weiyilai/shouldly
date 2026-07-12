using Xunit;

namespace EquivalencyComparisonTests;

/// <summary>
/// String comparison semantics.
/// </summary>
public class StringSpecs
{
    [Fact]
    public void Identical_strings_are_equivalent()
    {
        Compare.Run("hello", "hello").ShouldAgree(Outcome.Pass);
    }

    [Fact]
    public void Strings_differing_in_case_are_not_equivalent()
    {
        Compare.Run("hello", "HELLO").ShouldAgree(Outcome.Fail);
    }

    [Fact]
    public void Strings_differing_in_trailing_whitespace_are_not_equivalent()
    {
        Compare.Run("hello", "hello ").ShouldAgree(Outcome.Fail);
    }
}

/// <summary>
/// Types both libraries treat as comparison leaves rather than walking their members —
/// or fail to (see shouldly#1050 for Type, shouldly#1205 for Uri).
/// </summary>
public class SpecialTypeSpecs
{
    [Fact]
    public void Equal_uris_are_equivalent()
    {
        Compare.Run(new Uri("https://example.com/a"), new Uri("https://example.com/a")).ShouldAgree(Outcome.Pass);
    }

    [Fact]
    public void Different_uris_are_not_equivalent()
    {
        Compare.Run(new Uri("https://example.com/a"), new Uri("https://example.com/b")).ShouldAgree(Outcome.Fail);
    }

    [Fact]
    public void Uris_differing_only_in_fragment_are_equivalent_in_both()
    {
        // Uri.Equals ignores the fragment; both libraries defer to it.
        Compare.Run(new Uri("https://example.com/a#one"), new Uri("https://example.com/a#two")).ShouldAgree(Outcome.Pass);
    }

    [Fact]
    public void Equal_type_valued_members_are_equivalent()
    {
        var actual = new TypeHolder { Type = typeof(string) };
        var expected = new TypeHolder { Type = typeof(string) };

        Compare.Run(actual, expected).ShouldAgree(Outcome.Pass);
    }

    [Fact]
    public void Different_type_valued_members()
    {
        var actual = new TypeHolder { Type = typeof(string) };
        var expected = new TypeHolder { Type = typeof(int) };

        Compare.Run(actual, expected).ShouldDiverge(
            shouldly: Outcome.Error,
            fluentAssertions: Outcome.Fail,
            because: "Shouldly walks System.Type's reflection properties and blows up on members like DeclaringMethod (shouldly#1050); FluentAssertions compares Type as a value");
    }

    [Fact]
    public void Objects_with_no_members()
    {
        Compare.Run(new object(), new object()).ShouldDiverge(
            shouldly: Outcome.Pass,
            fluentAssertions: Outcome.Error,
            because: "Shouldly treats a memberless object as vacuously equivalent; FluentAssertions throws because comparing objects without members is probably a mistake");
    }
}
