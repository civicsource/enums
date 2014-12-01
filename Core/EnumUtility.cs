using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Archon.Enums
{
	public static class EnumUtility
	{
		static readonly IDictionary<Type, EnumDescriptor[]> reflectionCache = new Dictionary<Type, EnumDescriptor[]>();
		static readonly ReaderWriterLockSlim reflectionCacheLock = new ReaderWriterLockSlim();

		public static string CategoryOf(this Enum enumType)
		{
			var descriptions = Describe(enumType.GetType());
			var descriptor = descriptions.Single(d => d.Value.Equals(enumType));
			return descriptor.Category;
		}

		public static string DescriptionOf(this Enum enumType)
		{
			var descriptions = Describe(enumType.GetType());
			var descriptor = descriptions.Single(d => d.Value.Equals(enumType));
			return descriptor.Description;
		}

		public static IEnumerable<EnumDescriptor> Describe(Type enumType)
		{
			if (enumType == null)
				throw new ArgumentNullException("enumType");

			reflectionCacheLock.EnterReadLock();

			try
			{
				if (reflectionCache.ContainsKey(enumType))
					return reflectionCache[enumType];
			}
			finally
			{
				reflectionCacheLock.ExitReadLock();
			}

			reflectionCacheLock.EnterWriteLock();

			try
			{
				if (reflectionCache.ContainsKey(enumType))
					return reflectionCache[enumType];

				Array values = Enum.GetValues(enumType);
				var descriptions = new EnumDescriptor[values.Length];

				for (int i = 0; i < values.Length; i++)
				{
					descriptions[i] = new EnumDescriptor((Enum)values.GetValue(i));
				}

				reflectionCache.Add(enumType, descriptions);
				return descriptions;
			}
			finally
			{
				reflectionCacheLock.ExitWriteLock();
			}
		}

		public static T GetEnumValueFromEnumDescription<T>(this string description)
		{
			var type = typeof(T);

			if (!type.IsEnum)
				throw new ArgumentException();

			FieldInfo[] fields = type.GetFields();

			var field = fields
				.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Att = a })
				.SingleOrDefault(a => ((DescriptionAttribute)a.Att).Description == description);

			return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
		}
	}
}