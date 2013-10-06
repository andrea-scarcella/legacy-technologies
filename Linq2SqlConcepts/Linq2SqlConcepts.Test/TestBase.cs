using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Linq2SqlConcepts.Test
{
	[TestFixture]
	public class TestBase
	{
		NorthwindDataContext db = new NorthwindDataContext();

		[Test, Sequential]
		public void CanRetrieveProductsByCategoryName([Values("Beverages", "Condiments")]string categoryName, [Values(12, 12)] int expectedCount)
		{
			IEnumerable<Product> productsByCategory = from p in db.Products
													  //#AS:2013/10/06: Equals(string, StringComparison) not supported!
													  where p.Category.CategoryName.Equals(categoryName)
													  orderby p.ProductName
													  select p;
			Assert.AreEqual(expectedCount, productsByCategory.Count());

		}

		public TestBase()
		{

		}

	}
}
