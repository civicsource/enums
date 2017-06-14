using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Archon.Enums
{
	public struct EnumDescriptor
	{
		readonly Enum value;
		readonly string description, category;

		public Enum Value
		{
			get { return value; }
		}

		public string Description
		{
			get { return description ?? ""; }
		}

		public string Category
		{
			get { return category ?? ""; }
		}

		public EnumDescriptor(Enum enumType)
		{
			if (enumType == null)
				throw new ArgumentNullException("enumType");

			this.value = enumType;
			this.description = DescriptionOf(value);
			this.category = CategoryFor(value);
		}

		static string DescriptionOf(Enum value)
		{
			// By default, the result of Spaceify is just ToString with a space in front of each capital letter
			string enumDescription = Spaceify(value.ToString());

			MemberInfo[] memberInfo = value.GetType().GetTypeInfo().GetMember(value.ToString());
			if (memberInfo != null && memberInfo.Length == 1)
			{
				var customAttributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (customAttributes.Any())
				{
					enumDescription = ((DescriptionAttribute)customAttributes.First()).Description;
				}
			}

			return enumDescription;
		}

		static string Spaceify(string value)
		{
			Regex capitalLetterMatch = new Regex("\\B[A-Z]", RegexOptions.Compiled);
			string output = capitalLetterMatch.Replace(value.ToString(), " $&");
			output = output.Replace('_', ',');
			return output;
		}

		static string CategoryFor(Enum value)
		{
			string category = null;

			MemberInfo[] memberInfo = value.GetType().GetTypeInfo().GetMember(value.ToString());
			if (memberInfo != null && memberInfo.Length == 1)
			{
				var customAttributes = memberInfo[0].GetCustomAttributes(typeof(CategoryAttribute), false);
				if (customAttributes.Any())
				{
					category = ((CategoryAttribute)customAttributes.First()).Category;
				}
			}

			return category;
		}

		public override string ToString()
		{
			string value = Value.ToString();

			if (value == Description)
				return value;

			if (String.IsNullOrEmpty(Category))
				return String.Format("{0} ({1})", Description, value);

			return String.Format("{0}: {1} ({2})", Category, Description, value);
		}
	}
}