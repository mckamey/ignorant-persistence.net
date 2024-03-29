﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IgnorantPersistence
{
	/// <summary>
	/// A queryable interface to the underlying storage
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ITable<T> :
		IQueryable<T>,
		IEnumerable<T>
		where T : class
	{
		/// <summary>
		/// Adds an item
		/// </summary>
		/// <param name="item"></param>
		void Add(T item);

		/// <summary>
		/// Updates an item
		/// </summary>
		/// <param name="item"></param>
		void Update(T item);

		/// <summary>
		/// Deletes an item
		/// </summary>
		/// <param name="item"></param>
		void Remove(T item);

		/// <summary>
		/// Deletes a set of items
		/// </summary>
		/// <param name="match"></param>
		void RemoveWhere(Expression<Func<T, bool>> match);
	}
}
