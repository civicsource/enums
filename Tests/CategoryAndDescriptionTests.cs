using System;
using System.Linq;
using System.ComponentModel;
using Xunit;

namespace Archon.Enums.Tests
{
	public class CategoryAndDescriptionTests
	{
		[Fact]
		public void can_calculate_enum_description_by_finding_words_by_case()
		{
			Assert.Equal("Ghost White", Color.GhostWhite.DescriptionOf());
			Assert.Equal("Navy", Color.Navy.DescriptionOf());
		}

		[Fact]
		public void can_get_enum_description_from_specified_description()
		{
			Assert.Equal("Alice's Wonderland Blue", Color.AliceBlue.DescriptionOf());
		}

		[Fact]
		public void can_get_enum_category_from_specified_category()
		{
			Assert.Equal("Blues", Color.AliceBlue.CategoryOf());
			Assert.Equal("Blues", Color.Navy.CategoryOf());
			Assert.Equal("", Color.GhostWhite.CategoryOf());
		}

		[Fact]
		public void can_list_enum_descriptions()
		{
			Array expectedValues = Enum.GetValues(typeof(Color));

			EnumDescriptor[] values = EnumUtility.Describe(typeof(Color)).ToArray();

			for (int i = 0; i < expectedValues.Length; i++)
			{
				Color color = (Color)expectedValues.GetValue(i);
				Assert.Equal(color, values[i].Value);
				Assert.Equal(color.DescriptionOf(), values[i].Description);
				Assert.Equal(color.CategoryOf(), values[i].Category);
			}
		}
	}
}