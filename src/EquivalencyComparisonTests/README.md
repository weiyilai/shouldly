# Equivalency comparison tests

This project runs the same scenarios through Shouldly's `ShouldBeEquivalentTo` and
FluentAssertions' `BeEquivalentTo` (v7.2, the last Apache-2.0 licensed version, default options)
and asserts on *both* outcomes. It is an executable specification of where the two libraries
agree and diverge, modeled on the scenarios in
[FluentAssertions.Equivalency.Specs](https://github.com/fluentassertions/fluentassertions/tree/main/Tests/FluentAssertions.Equivalency.Specs).

Each test either documents parity (`ShouldAgree`) or pins down a divergence (`ShouldDiverge`)
with a `because:` explaining the behavioral difference. When Shouldly's equivalency defaults
change, the affected divergence tests fail, forcing the change to be acknowledged here — the
suite doubles as a progress metric for FluentAssertions convergence.

## Verified divergences (Shouldly 5.0-preview vs FluentAssertions 7.2 defaults)

| Scenario | Shouldly | FluentAssertions |
| --- | --- | --- |
| Different types, identical shape (incl. anonymous-type expectations) | Fail — requires identical runtime types | Pass — structural, by the expectation's members |
| Expectation with a subset of the subject's members | Fail | Pass — extra subject members are ignored |
| Lists/sets with same elements, different order | Fail — ordered element-by-element | Pass — order-insensitive by default (byte arrays excepted) |
| Array vs `List<T>` with identical elements | Fail — container types must match | Pass — container type ignored |
| Dictionaries with same pairs, different insertion order | Fail — compared as ordered pair sequences | Pass — matched by key |
| Dictionaries with reference-typed or collection values (equal, distinct instances) | Fail — `KeyValuePair` struct compared via `Equals`, so values are reference-compared (shouldly#767, shouldly#1077) | Pass — recurses into values |
| Member declared as base type holding derived instances with differing derived-only members | Fail — reflects over runtime type | Pass — member selection uses the declared type (cf. shouldly#1094) |
| Class overriding `Equals`: equal by members, unequal by `Equals` (or vice versa) | Compares members, ignores `Equals` | Honors the `Equals` override (value semantics) |
| Different enum types with the same underlying value | Fail — types must match | Pass — enums compared by value |
| `int 1` vs `long 1` | Fail — types must match | Pass — numeric types compared by value |
| Types with indexers | **Error** — `NotSupportedException` | Pass — indexers skipped |
| `Type`-valued members with different values | **Error** — walks `System.Type`'s reflection properties (shouldly#1050) | Fail cleanly — `Type` treated as a value |
| Cyclic object graphs, identical shape | Pass — tracks visited pairs | Fail — cycles rejected by default |
| Object graphs deeper than 10 levels | Pass — unbounded recursion | Fail — 10-level depth cap by default |
| Multidimensional arrays, same flat content, different shape | Pass — flattens all `IEnumerable`s | Fail — checks rank and dimension lengths |
| Two `new object()`s (no members) | Pass — vacuously equivalent | **Error** — "no members" `InvalidOperationException` |

Everywhere else tested — same-type member comparison, nested graphs, strings (ordinal,
case-sensitive), public fields, private member exclusion, structs, records, `DateTime`
(tick-exact, `Kind`-insensitive), `DateTimeOffset` (instant), `decimal` scale, `Guid`, NaN,
nullable lifting, `Uri` (fragment-insensitive via `Uri.Equals`), null handling, duplicate
multiplicity in collections — the two libraries agree.
