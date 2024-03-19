namespace RexStudios.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="StringExtensions" />.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The IsNullOrEmpty.
        /// </summary>
        /// <param name="instance">The instance<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNullOrEmpty(this string instance)
        {
            return string.IsNullOrEmpty(instance);
        }

        /// <summary>
        /// The HasValue.
        /// </summary>
        /// <param name="instance">The instance<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool HasValue(this string instance)
        {
            return !string.IsNullOrEmpty(instance);
        }

        /// <summary>
        /// The IsNumber.
        /// </summary>
        /// <param name="instance">The instance<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNumber(this string instance)
        {
            return Enumerable.All<char>((IEnumerable<char>)instance.ToCharArray(), new Func<char, bool>(char.IsNumber));
        }

        /// <summary>
        /// The IsEmailAddess.
        /// </summary>
        /// <param name="instance">The instance<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsEmailAddess(this string instance)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(instance);
        }

        /// <summary>
        /// The Append.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <param name="chunk">The chunk<see cref="string"/>.</param>
        /// <param name="condition">The condition<see cref="Func{string, bool}"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Append(this string target, string chunk, Func<string, bool> condition)
        {
            return target + (condition(target) ? chunk : "");
        }

        /// <summary>
        /// Append a string of text to an existing string.
        /// </summary>
        /// <param name="target">.</param>
        /// <param name="chunk">.</param>
        /// <returns>.</returns>
        public static string Append(this string target, string chunk)
        {
            return StringExtensions.Append(target, chunk, (Func<string, bool>)(s => true));
        }

        /// <summary>
        /// Remove '/' from at end of a string if it exists.
        /// </summary>
        /// <param name="target">.</param>
        /// <returns>.</returns>
        public static string RemoveTrailingSlash(this string target)
        {
            return IsNullOrEmpty(target) ? target : (ReplaceSlash(target));
        }

        /// <summary>
        /// Replace Slash with blank.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string ReplaceSlash(this string target)
        {
            return target.EndsWith("/") ? target?.Substring(0, target.Length - 1) : target;
        }

        internal static bool IsNullOrEmpty(object notificationTypeText)
        {
            throw new NotImplementedException();
        }
    }
}
