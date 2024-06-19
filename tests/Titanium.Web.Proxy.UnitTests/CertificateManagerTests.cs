﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Web.Proxy.Network;

namespace Titanium.Web.Proxy.UnitTests
{
    [TestClass]
    public class CertificateManagerTests
    {
        private static readonly string[] hostNames
            = { "facebook.com", "youtube.com", "google.com", "bing.com", "yahoo.com" };


        [TestMethod]
        public async Task CertificateManager_EngineBouncyCastle_CreatesCertificates ()
        {
            var tasks = new List<Task>();

            using var mgr = new CertificateManager(null, null, false, false, false, null)
            {
                CertificateEngine = CertificateEngine.BouncyCastle
            };
            mgr.ClearIdleCertificates();
            for (var i = 0; i < 5; i++)
                tasks.AddRange(hostNames.Select(host => Task.Run(() =>
                {
                    // get the connection
                    var certificate = mgr.CreateCertificate(host, false);
                    Assert.IsNotNull(certificate);
                })));

            await Task.WhenAll(tasks.ToArray());

            mgr.StopClearIdleCertificates();
        }

        [TestMethod]
        public async Task CertificateManager_EnginePure_CreatesCertificates ()
        {
            var tasks = new List<Task>();

            using var mgr = new CertificateManager(null, null, false, false, false, null)
            {
                CertificateEngine = CertificateEngine.Pure
            };
            mgr.ClearIdleCertificates();
            for (var i = 0; i < 5; i++)
                tasks.AddRange(hostNames.Select(host => Task.Run(() =>
                {
                    // get the connection
                    var certificate = mgr.CreateCertificate(host, false);
                    Assert.IsNotNull(certificate);
                })));

            await Task.WhenAll(tasks.ToArray());

            mgr.StopClearIdleCertificates();
        }

        // uncomment this to compare WinCert maker performance with BC (BC takes more time for same test above)
        //[TestMethod]
        public async Task Simple_Create_Win_Certificate_Test ()
        {
            var tasks = new List<Task>();

            using var mgr = new CertificateManager(null, null, false, false, false, null)
            { CertificateEngine = CertificateEngine.DefaultWindows };

            mgr.CreateRootCertificate();
            mgr.TrustRootCertificate(true);
            mgr.ClearIdleCertificates();

            for (var i = 0; i < 5; i++)
                tasks.AddRange(hostNames.Select(host => Task.Run(() =>
                {
                    // get the connection
                    var certificate = mgr.CreateCertificate(host, false);
                    Assert.IsNotNull(certificate);
                })));

            await Task.WhenAll(tasks.ToArray());
            mgr.RemoveTrustedRootCertificate(true);
            mgr.StopClearIdleCertificates();
        }

        [TestMethod]
        public async Task CertificateManager_EngineBouncyCastleFast_Creates500Certificates ()
        {
            var tasks = new List<Task>();

            using var mgr = new CertificateManager(null, null, false, false, false, null)
            { CertificateEngine = CertificateEngine.BouncyCastleFast };

            mgr.SaveFakeCertificates = true;

            for (var i = 0; i < 500; i++)
                tasks.AddRange(hostNames.Select(host => Task.Run(() =>
                {
                    var certificate = mgr.CreateServerCertificate(host);
                    Assert.IsNotNull(certificate);
                })));

            await Task.WhenAll(tasks.ToArray());
        }

        [TestMethod]
        public async Task CertificateManager_EnginePure_Creates500Certificates ()
        {
            var tasks = new List<Task>();

            using var mgr = new CertificateManager(null, null, false, false, false, null)
            { CertificateEngine = CertificateEngine.Pure };

            mgr.SaveFakeCertificates = true;

            for (var i = 0; i < 500; i++)
                tasks.AddRange(hostNames.Select(host => Task.Run(() =>
                {
                    var certificate = mgr.CreateServerCertificate(host);
                    Assert.IsNotNull(certificate);
                })));

            await Task.WhenAll(tasks.ToArray());
        }
    }
}
