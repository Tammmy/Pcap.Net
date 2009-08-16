using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;

namespace PcapDotNet.Core.Test
{
    /// <summary>
    /// Summary description for PacketDumpFileTests
    /// </summary>
    [TestClass]
    public class PacketDumpFileTests
    {
        public PacketDumpFileTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DumpWithoutDeviceTest()
        {
            string filename = Path.GetTempPath() + @"dump.pcap";

            Packet expectedPacket = PacketBuilder.Ethernet(DateTime.Now, new MacAddress(1), new MacAddress(2), EthernetType.QInQ,
                                                           new Datagram(new byte[] {1, 2, 3}));
            PacketDumpFile.Dump(filename, new PcapDataLink(DataLinkKind.Ethernet), PacketDevice.DefaultSnapshotLength,
                                new[] {expectedPacket});

            using (PacketCommunicator communicator = new OfflinePacketDevice(filename).Open())
            {
                Packet actualPacket;
                PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out actualPacket);
                Assert.AreEqual(PacketCommunicatorReceiveResult.Ok, result);
                Assert.AreEqual(expectedPacket, actualPacket);
            }
        }
    }
}