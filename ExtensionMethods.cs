using System.Collections.Generic;

namespace ClipboardGrabber
{
	static class ExtensionMethods
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			TValue value;
			if (dictionary.TryGetValue(key, out value))
				return value;
			else
				return defaultValue;
		}
	}
}
