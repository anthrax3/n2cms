﻿using N2.Engine;
using N2.Security;
using N2.Tests.Fakes;
using NUnit.Framework;
using N2.Collections;

namespace N2.Extensions.Tests.Extensions
{
	[TestFixture]
	public class FilterExtensionsTests : ExtensionTests
	{
		Function<ISecurityManager> backup;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			backup = AccessFilter.CurrentSecurityManager;
			FakeSecurityManager securityManager = new FakeSecurityManager();
			AccessFilter.CurrentSecurityManager = () => securityManager;
		}
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			AccessFilter.CurrentSecurityManager = backup;
		}



		[Test]
		public void DoenstFilter_AccessibleItem()
		{
			var item = CreateOneItem<MyItem>(3, "item", root);

			var filteredItem = item.FilterByAccess();

			Assert.That(filteredItem, Is.SameAs(item));
		}

		[Test]
		public void Filters_UnaccessibleItem()
		{
			var item = CreateOneItem<MyItem>(3, "item", root);
			item["Unaccessible"] = true;

			var filteredItem = item.FilterByAccess();

			Assert.That(filteredItem, Is.Null);
		}



		[Test]
		public void DoesntFilter_VisibleItem()
		{
			var item = CreateOneItem<MyItem>(3, "item", root);

			var filteredItem = item.FilterByNavigation();

			Assert.That(filteredItem, Is.SameAs(item));
		}

		[Test]
		public void Filters_HiddenItem()
		{
			var item = CreateOneItem<MyItem>(3, "item", root);
			item.Visible = false;

			var filteredItem = item.FilterByNavigation();

			Assert.That(filteredItem, Is.Null);
		}
	}
}
