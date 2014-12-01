# Archon Enums Utility

> Provides convenient utility methods for working with Enums and their user-friendly descriptions.

## How to Use

Install via [nuget](https://www.nuget.org/packages/Archon.Enums/)

```
install-package Archon.Enums
```

Make sure to add `using Archon.Enums;` to the top of your files to get access to the following extension methods.

### Categories & Descriptions

Given an `enum` that looks like this:

```c#
enum Color
{
	[Category("Blues")]
	[Description("Alice's Wonderland Blue")]
	AliceBlue,

	GhostWhite,

	[Category("Blues")]
	Navy
}
```

You can now easily retrieve the descriptions and categories for an enum:

```c#
Assert.Equal("Alice's Wonderland Blue", Color.AliceBlue.DescriptionOf());
Assert.Equal("Blues", Color.AliceBlue.CategoryOf());
```

as well as get sensible defaults for non-defined descriptions:

```c#
Assert.Equal("Ghost White", Color.GhostWhite.DescriptionOf());
```

You can also list out the descriptors for the `enum`:

```c#
IEnumerable<EnumDescriptor> values = EnumUtility.Describe(typeof(Color));
foreach (var val in values)
{
	Console.WriteLine(String.Format("{0}: {1} ({2})", val.Category, val.Description, val.Value));
	//this will output "Blues: Alice's Wonderland Blue (AliceBlue)" for Color.AliceBlue
}
```

### States/Provinces & Countries

Install the [geography nuget package](https://www.nuget.org/packages/Archon.Enums.Geography/)

```
install-package Archon.Enums.Geography
```

This includes two enum types, `StateProvince` and `Country` which enumerate US states, Canadian provinces, and countries utilizing the aforementioned framework with user friendly descriptions & categories.