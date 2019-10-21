using UnityEngine;

namespace Folder
{
	public static class StringExtensions
	{
		public static string WithoutFileName(this string str)
		{
			return WithoutLastFolder(str);
		}

		public static string WithoutLastFolder(this string str)
		{
			return str.Substring(0, str.LastIndexOf("/") + 1);
		}

		/// <summary>
		/// Remember that the folder must be located in Resources.
		/// </summary>
		public static T[] LoadFolder<T>(this string str) where T : Object
		{
			string resourcesFolder = "Resources/";

			if (str.IndexOf(resourcesFolder) == -1) // Ensure the path is in Resources.
			{
				Debug.LogError("The folder at path " + str + " must be located in Resources if you want to load it.");
				return null;
			}

			// We remove the part of the path that is before Resources
			str = str.Substring(str.LastIndexOf(resourcesFolder) + resourcesFolder.Length);

			return Resources.LoadAll<T>(str);
		}
	}

	public static class ArrayExtensions
	{
		/// <summary>
		/// Returns a random element within the array, based on its size.
		/// </summary>
		public static T GetRandomElement<T>(this T[] array)
		{
			return array[array.GetRandomIndex()];
		}

		/// <summary>
		/// Returns a random int between 0 and the size of the array.
		/// </summary>
		public static int GetRandomIndex<T>(this T[] array)
		{
			return Random.Range(0, array.Length);
		}
	}
}