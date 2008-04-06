using NUnit.Framework;

namespace N2.Tests.Content
{
	[TestFixture]
	public class ContentItemTests : ItemTestsBase
	{
		[Test]
		public void SettingValueTypeAddsDetail()
		{
			AnItem item = new AnItem();
			item.IntProperty = 3;
			Assert.AreEqual(1, item.Details.Count);
			Assert.AreEqual(3, item.IntProperty);
			Assert.AreEqual(3, item.Details["IntProperty"].Value);
			Assert.AreEqual(3, item["IntProperty"]);
		}

		[Test]
		public void SettingValueTypeToDefailtRemovesDetail()
		{
			AnItem item = new AnItem();
			item.IntProperty = 3;
			item.IntProperty = 0;
			Assert.AreEqual(0, item.Details.Count);
		}

		[Test]
		public void SettingReferenceTypeAddsDetail()
		{
			AnItem item = new AnItem();
			item.StringProperty = "hello";
			Assert.AreEqual(1, item.Details.Count);
			Assert.AreEqual("hello", item.StringProperty);
			Assert.AreEqual("hello", item.Details["StringProperty"].Value);
			Assert.AreEqual("hello", item["StringProperty"]);
		}

		[Test]
		public void SettingReferenceTypeToDefailtRemovesDetail()
		{
			AnItem item = new AnItem();
			item.StringProperty = "hello";
			item.StringProperty = string.Empty;
			Assert.AreEqual(0, item.Details.Count);
		}

		[Test]
		public void AddTo_UpdatesParentRelation()
		{
			AnItem parent = new AnItem();
			AnItem child = new AnItem();

			child.AddTo(parent);
			Assert.AreEqual(parent, child.Parent);
		}

		[Test]
		public void AddTo_IsAddedToChildren()
		{
			AnItem parent = new AnItem();
			AnItem child = new AnItem();

			child.AddTo(parent);
			EnumerableAssert.Contains(parent.Children, child);
		}

		[Test]
		public void AddTo_IsRemovedFrom_PreviousParentChildren()
		{
			AnItem parent1 = new AnItem();
			AnItem parent2 = new AnItem();
			AnItem child = new AnItem();

			child.AddTo(parent1);
			EnumerableAssert.Contains(parent1.Children, child);
			EnumerableAssert.DoesntContain(parent2.Children, child);

			child.AddTo(parent2);
			EnumerableAssert.DoesntContain(parent1.Children, child);
			EnumerableAssert.Contains(parent2.Children, child);
		}

		[Test]
		public void AddTo_CanBeAddedToNull()
		{
			AnItem parent = new AnItem();
			AnItem child = new AnItem();

			child.AddTo(parent);
			Assert.AreEqual(parent, child.Parent);
			EnumerableAssert.Contains(parent.Children, child);

			child.AddTo(null);
			Assert.IsNull(child.Parent);
			EnumerableAssert.DoesntContain(parent.Children, child);
		}

		[Test]
		public void AddsToChildrenWhenOnlyParentIsSet()
		{
			AnItem parent = new AnItem();
			AnItem child = new AnItem();

			child.Parent = parent;

			child.AddTo(parent);
			EnumerableAssert.Contains(parent.Children, child);
		}

		[Test]
		public void AddTo_IsAppendedLast()
		{
			AnItem parent = new AnItem();
			AnItem child1 = new AnItem();
			AnItem child2 = new AnItem();
			AnItem child3 = new AnItem();
			AnItem child4 = new AnItem();

			child1.AddTo(parent);
			child2.AddTo(parent);
			child3.AddTo(parent);
			child4.AddTo(parent);

			Assert.AreEqual(child1, parent.Children[0]);
			Assert.AreEqual(child2, parent.Children[1]);
			Assert.AreEqual(child3, parent.Children[2]);
			Assert.AreEqual(child4, parent.Children[3]);
		}

		[Test]
		public void AddTo_IsAppendedLast_EvenWhenSortOrder_MayIndicateOtherwise()
		{
			AnItem parent = new AnItem();
			AnItem child1 = new AnItem();
			child1.SortOrder = 4;
			AnItem child2 = new AnItem();
			child2.SortOrder = 3;
			AnItem child3 = new AnItem();
			child3.SortOrder = 2;
			AnItem child4 = new AnItem();
			child4.SortOrder = 1;

			child1.AddTo(parent);
			child2.AddTo(parent);
			child3.AddTo(parent);
			child4.AddTo(parent);

			Assert.AreEqual(child1, parent.Children[0]);
			Assert.AreEqual(child2, parent.Children[1]);
			Assert.AreEqual(child3, parent.Children[2]);
			Assert.AreEqual(child4, parent.Children[3]);
		}

		[Test]
		public void DoesntAddToChildrenTwice()
		{
			AnItem parent = new AnItem();
			AnItem child = new AnItem();

			parent.Children.Add(child);

			child.AddTo(parent);
			Assert.AreEqual(parent, child.Parent);
			Assert.AreEqual(1, parent.Children.Count);
		}

		[Test]
		public void GetChild()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);

			Assert.AreEqual(item1, root.GetChild("item1"));
		}

		[Test]
		public void GetChild_NoItemYeldsNull()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);

			Assert.IsNull(root.GetChild("item2"));
		}

		[Test]
		public void GetChild_WithNullDoesntThrowException()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);

			Assert.IsNull(root.GetChild(null));
		}

		[Test]
		public void GetChild_WithEmptyStringDoesntThrowException()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);

			Assert.IsNull(root.GetChild(string.Empty));
		}

		[Test]
		public void GetChild_WithManyChildren()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(3, "item2", root);
			AnItem item3 = CreateOneItem<AnItem>(4, "item3", root);

			Assert.AreEqual(item1, root.GetChild("item1"));
			Assert.AreEqual(item2, root.GetChild("item2"));
			Assert.AreEqual(item3, root.GetChild("item3"));
		}

		[Test]
		public void GetChild_NameIncludingAspxYeldsNull()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);

			Assert.IsNull(root.GetChild("item1.aspx"));
		}

		[Test]
		public void GetChild_NameIncludingDot()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item.1", root);

			Assert.AreEqual(item1, root.GetChild("item.1"));
		}

		[Test]
		public void GetChild_NameIncludingDotAndAspxYeldsNull()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item.1", root);

			Assert.IsNull(root.GetChild("item.1.aspx"));
		}

		[Test]
		public void GetChild_NameIncludingUnicodeCharacter()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "�nnu en �ngande �", root);

			Assert.AreEqual(item1, root.GetChild("�nnu en �ngande �"));
		}

		[Test]
		public void GetChildsChild()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(2, "item2", item1);

			Assert.AreEqual(item2, root.GetChild("item1/item2"));
		}

		[Test]
		public void DoesntFindChildWhenTrailingAspx()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(2, "item2", item1);

			Assert.IsNull(root.GetChild("item1/item2.aspx"));
		}

		[Test]
		public void GetChildsChildWithTrailContainingDots()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item.1", root);
			AnItem item2 = CreateOneItem<AnItem>(2, "item2", item1);

			Assert.AreEqual(item2, root.GetChild("item.1/item2"));
		}

		[Test]
		public void GetAncestorWayDown()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item", root);
			AnItem item2 = CreateOneItem<AnItem>(3, "item", item1);
			AnItem item3 = CreateOneItem<AnItem>(4, "item", item2);
			AnItem item4 = CreateOneItem<AnItem>(5, "item", item3);
			AnItem item5 = CreateOneItem<AnItem>(6, "item", item4);
			AnItem item6 = CreateOneItem<AnItem>(7, "item", item5);
			AnItem item7 = CreateOneItem<AnItem>(8, "item", item6);
			AnItem item8 = CreateOneItem<AnItem>(9, "item", item7);

			Assert.AreEqual(item2, root.GetChild("item/item"));
			Assert.AreEqual(item2, root.GetChild("item").GetChild("item"));
			Assert.AreEqual(item4, root.GetChild("item/item/item/item"));
			Assert.AreEqual(item4, root.GetChild("item/item").GetChild("item/item"));
			Assert.AreEqual(item8, root.GetChild("item/item/item/item").GetChild("item/item/item/item"));
			Assert.AreEqual(item8, root.GetChild("item/item/item/item/item/item/item/item"));
		}

		[Test]
		public void GetChild_HolesInPathYeldsNull()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(3, "item2", item1);
			AnItem item3 = CreateOneItem<AnItem>(4, "item3", item2);

			Assert.IsNull(root.GetChild("item1/itemX/item3"));
		}

		[Test]
		public void GetChild_CanFindCurrentItem()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(3, "item2", item1);

			Assert.AreEqual(root.GetChild("/"), root);
		}

		[Test]
		public void GetChild_CanFindItem_WhenTrailingSlash()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(3, "item2", item1);

			Assert.AreEqual(root.GetChild("/item1/"), item1);
		}

		[Test]
		public void GetChild_FindGrandChild_WhenTrailingSlash()
		{
			AnItem root = CreateOneItem<AnItem>(1, "root", null);
			AnItem item1 = CreateOneItem<AnItem>(2, "item1", root);
			AnItem item2 = CreateOneItem<AnItem>(3, "item2", item1);

			Assert.AreEqual(root.GetChild("item1/item2/"), item2);
		}

		[Test]
		public void CanCloneItem()
		{
			ContentItem root = CreateOneItem<AnItem>(1, "root", null);
			ContentItem clonedRoot = root.Clone(false);
			
			Assert.AreEqual(0, clonedRoot.ID);
			Assert.AreEqual(root.Name, clonedRoot.Name);
			Assert.AreEqual(root.Title, clonedRoot.Title);
		}

		[Test]
		public void CanCloneItemWithDetail()
		{
			ContentItem root = CreateOneItem<AnItem>(1, "root", null);
			root["TheDetail"] = "TheValue";
			ContentItem clonedRoot = root.Clone(false);

			Assert.AreEqual("TheValue", clonedRoot["TheDetail"]);
		}

		[Test]
		public void CanCloneItemWithDetailCollection()
		{
			ContentItem root = CreateOneItem<AnItem>(1, "root", null);
			root.GetDetailCollection("TheDetailCollection", true).Add("TheValue");
			ContentItem clonedRoot = root.Clone(false);

			Assert.AreEqual("TheValue", clonedRoot.GetDetailCollection("TheDetailCollection", false)[0]);
		}
	}
}