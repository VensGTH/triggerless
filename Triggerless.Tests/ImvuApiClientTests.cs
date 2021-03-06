﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggerless.Services.Server;

namespace Triggerless.Tests
{
    [TestFixture]
    public class ImvuApiClientTests
    {
        private const long USER_ID = 70587910; //oP0PPYo
        private const long PRoDUCT_ID = 47935570; //thb timbs
        private ImvuApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new ImvuApiClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public async Task UserSingle()
        {
            var user = await _client.GetUser(USER_ID);
            Assert.IsNotNull(user, "null");
            Assert.That(user.AvatarName.ToLower().Contains("p0ppy"), "not p0ppy");
        } 

        [Test]
        public async Task ProductSingle()
        {
            var product = await _client.GetProduct(PRoDUCT_ID);
            Assert.IsNotNull(product, "null");
            Assert.That(product.Name.ToLower().Contains("timb"), "not timb");

        }

        [Test]
        public async Task ProductMultiple()
        {
            var pids = new long[] { 10599277, 52874386, 52874314, 52797258, 52459235, 52439278, 52439111 };
            var products = await _client.GetProducts(pids);
            Assert.IsNotNull(products);
            Assert.AreEqual(pids.Length, products.Products.Count());
            Assert.AreEqual(pids.Length, products.Products.Count(p => p.Status == "success"));
        }

        [Test]
        public async Task ProductSingleHidden()
        {
            long hiddenProductID = 41967L; // Ankh Collar
            var product = await _client.GetProduct(hiddenProductID);
            Assert.IsNotNull(product);
            Assert.IsNotNull(product.Name);
            Assert.IsNotNull(product.CreatorName);
            Assert.IsNotNull(product.ProductImage);
            Assert.That(product.CreatorId > 0);
        }
    }
}