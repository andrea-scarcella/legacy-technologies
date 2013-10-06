using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NUnit.Framework;

namespace Linq2SqlConcepts.Test
{
	[TestFixture]
	public class TestBase
	{
		NorthwindDataContext db = new NorthwindDataContext();
		private TransactionScope scope;

		private IEnumerable<Product> beverages;
		private string newProductName = "new test product";
		[SetUp]
		public void Setup()
		{
			scope = new TransactionScope();
			beverages = from p in db.Products
						where p.Category.CategoryName == "Beverages"
						orderby p.ProductName
						select p;
		}

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
		[Test]
		public void CanInsertNewProduct()
		{
			Product newProduct = new Product { ProductName = newProductName };
			db.Products.InsertOnSubmit(newProduct);
			db.SubmitChanges();

			var productList = from p in db.Products
							  where p.ProductName.Equals((newProductName))
							  select p;
			Assert.AreEqual(1,productList.Count());
			

		}
		[TearDown]
		public void TearDown()
		{
			scope.Dispose();
		}

	}
}
