using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Data.Json;

namespace StreetFoo.Client.Tests
{
    [TestClass()]
    public class TestJsonMapper
    {
        [TestMethod()]
        public void TestLoad()
        {
            // data...
            string json = "{\"ownerUserId\":\"4fb28003e7044a90803a3168\",\"title\":\"Remove damaged light\",\"description\":\"In malesuada vulputate ipsum sed posuere. Cras pretium venenatis tellus, quis vehicula est auctor vel. In posuere adipiscing urna, ac tempor enim mollis ac. Cras vehicula pulvinar tellus quis blandit. Suspendisse potenti. Phasellus sodales imperdiet venenatis. Quisque pulvinar facilisis orci, nec venenatis arcu faucibus non. Aliquam vehicula ante id nisl facilisis cursus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Suspendisse tellus neque, imperdiet a rhoncus vitae, vestibulum ac sapien. Donec vitae sapien non mauris fermentum vulputate non ac dui. Duis hendrerit purus id justo volutpat tristique. Cras ut turpis tellus, vel dapibus diam.\",\"latitude\":0,\"longitude\":0,\"apiKey\":\"4f41463a-dfc7-45dd-8d95-bf339f040933\",\"_id\":\"4fb2a0e1e7044a92bc693ac5\"}";

            // use the mapper to create an instance...
            var mapper = JsonMapperFactory.GetMapper<ReportItem>();
            ReportItem item = mapper.Load(json);

            // check...
            Assert.AreEqual("Remove damaged light", item.Title);
        }

        [TestMethod()]
        public void TestLoadArray()
        {
            // data...
            string json = "[{\"ownerUserId\":\"4fb28003e7044a90803a3168\",\"title\":\"Remove damaged light 1\",\"description\":\"In malesuada vulputate ipsum sed posuere. Cras pretium venenatis tellus, quis vehicula est auctor vel. In posuere adipiscing urna, ac tempor enim mollis ac. Cras vehicula pulvinar tellus quis blandit. Suspendisse potenti. Phasellus sodales imperdiet venenatis. Quisque pulvinar facilisis orci, nec venenatis arcu faucibus non. Aliquam vehicula ante id nisl facilisis cursus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Suspendisse tellus neque, imperdiet a rhoncus vitae, vestibulum ac sapien. Donec vitae sapien non mauris fermentum vulputate non ac dui. Duis hendrerit purus id justo volutpat tristique. Cras ut turpis tellus, vel dapibus diam.\",\"latitude\":0,\"longitude\":0,\"apiKey\":\"4f41463a-dfc7-45dd-8d95-bf339f040933\",\"_id\":\"4fb2a0e1e7044a92bc693ac5\"},{\"ownerUserId\":\"4fb28003e7044a90803a3168\",\"title\":\"Remove damaged light 2\",\"description\":\"In malesuada vulputate ipsum sed posuere. Cras pretium venenatis tellus, quis vehicula est auctor vel. In posuere adipiscing urna, ac tempor enim mollis ac. Cras vehicula pulvinar tellus quis blandit. Suspendisse potenti. Phasellus sodales imperdiet venenatis. Quisque pulvinar facilisis orci, nec venenatis arcu faucibus non. Aliquam vehicula ante id nisl facilisis cursus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Suspendisse tellus neque, imperdiet a rhoncus vitae, vestibulum ac sapien. Donec vitae sapien non mauris fermentum vulputate non ac dui. Duis hendrerit purus id justo volutpat tristique. Cras ut turpis tellus, vel dapibus diam.\",\"latitude\":0,\"longitude\":0,\"apiKey\":\"4f41463a-dfc7-45dd-8d95-bf339f040933\",\"_id\":\"4fb2a0e1e7044a92bc693ac6\"}]";

            // use the mapper to create an instance...
            var mapper = JsonMapperFactory.GetMapper<ReportItem>();
            List<ReportItem> items = mapper.LoadArray(json);

            // check...
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("Remove damaged light 1", items[0].Title);
            Assert.AreEqual("Remove damaged light 2", items[1].Title);
        }
    }
}
