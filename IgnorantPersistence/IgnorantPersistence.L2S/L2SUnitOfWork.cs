﻿using System;
using System.Data.Linq;

namespace IgnorantPersistence.L2S
{
	/// <summary>
	/// A unit-of-work adapter for LINQ-to-SQL DataContexts.
	/// </summary>
	public class L2SUnitOfWork : IUnitOfWork
	{
		#region Fields

		private readonly DataContext DB;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="db"></param>
		public L2SUnitOfWork(DataContext db)
		{
			this.DB = db;
		}

		#endregion Init

		#region Methods

		/// <summary>
		/// Checks if the configured database exists at the location specified in the connection string.
		/// </summary>
		/// <returns></returns>
		public bool CanConnect()
		{
			return this.DB.DatabaseExists();
		}

		/// <summary>
		/// Creates a database at the location specified in the connection string.
		/// </summary>
		public void InitializeDatabase()
		{
			this.DB.CreateDatabase();
		}

		#endregion Methods

		#region Events

		/// <summary>
		/// A callback which notifies any listeners of pending changes
		/// </summary>
		public event Action<L2SUnitOfWork, ChangeSet> OnCommit;

		#endregion Events

		#region IUnitOfWork Members

		/// <summary>
		/// Stores any pending changes
		/// </summary>
		public void Save()
		{
			if (this.OnCommit != null)
			{
				ChangeSet changes = this.DB.GetChangeSet();

				if ((changes.Inserts.Count > 0) ||
					(changes.Updates.Count > 0) ||
					(changes.Deletes.Count > 0))
				{
					this.OnCommit(this, changes);
				}
			}

			this.DB.SubmitChanges(ConflictMode.ContinueOnConflict);
		}

		/// <summary>
		/// Returns a collection of objects of a particular type,
		/// where the type is defined by the T parameter.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be returned.</typeparam>
		/// <returns>A collection of objects.</returns>
		public ITable<T> GetTable<T>() where T : class
		{
			if (typeof(ISoftDeleteEntity).IsAssignableFrom(typeof(T)))
			{
				return new L2SSoftDeleteTable<T>(this.DB);
			}

			return new L2STable<T>(this.DB);
		}

		#endregion IUnitOfWork Members
	}
}
