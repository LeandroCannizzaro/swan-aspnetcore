﻿namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.EntityFrameworkCore;
    using Mocks;
    using NUnit.Framework;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    class AuditTrailTest
    {
        private ProductMock _product;

        private BusinessDbContextMock SetupDatabase(string name)
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
               .UseInMemoryDatabase(name);
            var options = builder.Options;
            return new BusinessDbContextMock(options);
        }

        [SetUp]
        public void SetUp()
        {
            _product = ProductMock.GetProduct();
        }

        [Test]
        public async Task SaveChangesEntityTestAsync()
        {
            using (var context = SetupDatabase(nameof(SaveChangesEntityTestAsync)))
            {
                context.Add(_product);
                await context.SaveChangesAsync();

                var audit = context.AuditTrailEntries.Last();

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(1, audit.Action, "Where 1 means Create");
            }
        }

        [Test]
        public void SaveChangesEntityTest()
        {
            using (var context = SetupDatabase(nameof(SaveChangesEntityTest)))
            {
            
                context.Add(_product);
                context.SaveChanges();

                var audit = context.AuditTrailEntries.Last();

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(1, audit.Action, "Where 1 means Create");
            }
        }

        [Test]
        public void UpdatedChangesEntityTest()
        {
            using (var context = SetupDatabase(nameof(UpdatedChangesEntityTest)))
            {
                context.Add(_product);
                context.SaveChanges();

                context.Update(_product);
                context.SaveChanges();

                var audit = context.AuditTrailEntries.Last();

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(2, audit.Action, "Where 2 means Update");
            }
        }

        [Test]
        public void DeleteChangesEntityTest()
        {
            using (var context = SetupDatabase(nameof(DeleteChangesEntityTest)))
            {
                context.Add(_product);
                context.SaveChanges();

                context.Remove(_product);
                context.SaveChanges();

                var audit = context.AuditTrailEntries.Last();

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(3, audit.Action, "Where 3 means Delete");
            }
        }
    }
}
