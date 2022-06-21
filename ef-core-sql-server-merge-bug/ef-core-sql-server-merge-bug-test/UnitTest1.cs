using ef_core_sql_server_merge_bug;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ef_core_sql_server_merge_bug_test
{
	public class UnitTest1
	{
		[Fact]
		public void Demonstrate_SQL_Server_Bug()
		{
			// arrange
			MigrateDatabase();
			FillWithInitialData();

			Random r = new Random(DateTime.Now.Millisecond);
			int rInt = r.Next(278, 300);

			var context = new MyContext();
			var someInitialData = Enumerable.Range(1, rInt).Select(i =>
				new ImpactValue(DateTime.Now.Date.AddDays(i)));

			// act
			context.ImpactValues.AddRange(someInitialData);
			var action = () => context.SaveChanges();

			// assert
			action.Should().NotThrow();
		}

		private void MigrateDatabase()
		{
			var context = new MyContext();


			if (context.Database.GetDbConnection().Database != "")
			{
				context.Database.Migrate();
			}
		}

		/// <summary>
		/// just to fill-up with some random data and make sure that history data contains also some data
		/// </summary>
		private void FillWithInitialData()
		{
			FillWithInitialDataBatch();
			FillWithInitialDataBatch();
			FillWithInitialDataBatch();
			FillWithInitialDataBatch();
		}

		private void FillWithInitialDataBatch()
		{
			var context = new MyContext();

			var someInitialData = Enumerable.Range(1, 250).Select(i => new ImpactValue(DateTime.Now.Date.AddDays(i)));
			context.ImpactValues.AddRange(someInitialData);
			context.SaveChanges();

			context.Database.ExecuteSqlRaw(@"
				DELETE [dbo].[ImpactValue] -- just to fill-up history table with some data


				-- just to clear any cached execution plan
				CHECKPOINT
				DBCC DROPCLEANBUFFERS
			");
		}
	}
}