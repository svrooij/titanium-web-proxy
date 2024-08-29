﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unobtanium.Web.Proxy.IntegrationTests;

[TestClass]
public class StressTests
{
    [TestMethod, Timeout(30000)]
    public async Task Stress_Test_With_One_Server_And_Many_Clients()
    {
        var testSuite = new TestSuite();

        var server = testSuite.GetServer();
        server.HandleRequest(context =>
        {
            return context.Response.WriteAsync("I am server. I received your greetings.");
        });

        using var proxy = testSuite.GetProxy();

        await Task.Delay(1000);

        var tasks = new List<Task>();

        //send 1000 requests to server
        for (var j = 0; j < 1000; j++)
        {
            var task = Task.Run(async () =>
            {
                using var client = testSuite.GetClient(proxy);

                await client.PostAsync(new Uri(server.ListeningHttpsUrl),
                    new StringContent("hello server. I am a client."));
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}
