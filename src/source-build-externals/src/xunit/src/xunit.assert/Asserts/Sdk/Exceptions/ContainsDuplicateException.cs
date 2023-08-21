using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when <see cref="Assert.Distinct{T}(System.Collections.Generic.IEnumerable{T})" />
	/// or <see cref="Assert.Distinct{T}(System.Collections.Generic.IEnumerable{T}, System.Collections.Generic.IEqualityComparer{T})" />
	/// finds a duplicate entry in the collection
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class ContainsDuplicateException : XunitException
	{

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsDuplicateException"/> class.
		/// </summary>
		/// <param name="duplicateObject">The object that was present twice in the collection.</param>
		/// <param name="collection">The collection that was checked for duplicate entries.</param>
#if XUNIT_NULLABLE
		public ContainsDuplicateException(object? duplicateObject, IEnumerable collection)
#else
		public ContainsDuplicateException(object duplicateObject, IEnumerable collection)
#endif
			: base("Assert.Distinct() Failure")
		{
			this.DuplicateObject = duplicateObject;
			this.Collection = collection;
		}

		/// <summary>
		/// Gets the collection that was checked for duplicate entries.
		/// </summary>
		public IEnumerable Collection { get; }

		/// <summary>
		/// Gets the object that was present more than once in the collection.
		/// </summary>
#if XUNIT_NULLABLE
		public object? DuplicateObject { get; }
#else
		public object DuplicateObject { get; }
#endif

		/// <inheritdoc/>
		public override string Message
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, "{0}: The item {1} occurs multiple times in {2}.", base.Message, ArgumentFormatter.Format(DuplicateObject), ArgumentFormatter.Format(Collection));
			}
		}
	}
}
