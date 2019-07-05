using AwApi.Business.Helpers;
using AwApi.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwApi.ViewModels;

namespace AwApi.Tests
{
    [TestClass]
    public class ReceiptHelperTests
    {
        [TestMethod]
        public void ProcessTextReceiptTest()
        {
            // Arrange
            var receiptData = new List<byte[]>();
            var unitTestString = "Test receipt data string";
            // Convert to base64
            var base64EncodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(unitTestString));
            // Convert base64 string to byte array
            var base64ByteArray = Encoding.UTF8.GetBytes(base64EncodedString);
            receiptData.Add(base64ByteArray);

            // Act
            var result = ReceiptHelper.ProcessTextReceipt(receiptData, "UTF-8");
            var base64EncodedBytes = Convert.FromBase64String(result);
            var plainText = Encoding.UTF8.GetString(base64EncodedBytes);

            // Assert
            Assert.AreEqual(unitTestString, plainText);
        }

        [TestMethod]
        public void ProcessReceiptTest()
        {
            // Arrange
            var byteList = new List<byte>();
            byteList.Add(Convert.ToByte(22));
            var receiptSegments = new List<List<ReceiptSegmentType>>();
            var segmentList = new List<ReceiptSegmentType>
            {
                new ReceiptSegmentType {MimeData = byteList, Sequence = -1}
            };
            receiptSegments.Add(segmentList);
            var mimeType = "application/pdf";
            var mockReceiptIntegration = new Mock<IReceiptIntegration>();
            mockReceiptIntegration.Setup(x => x.MergePdf(It.IsAny<List<byte[]>>())).Returns(() => new byte[1]);

            // Act
            var result = ReceiptHelper.ProcessReceipt(receiptSegments, mimeType, mockReceiptIntegration.Object);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GenerateAdditionalDataReceiptsPrecompletion_ForJpeg_DoesNotGenerateAdditionalData()
        {
            //Arrange & Act
            var additionalData = GenerateAdditionalDataPrecompletionTest("image/jpeg");

            // Assert
            Assert.IsNull(additionalData);
        }

        [TestMethod]
        public void GenerateAdditionalDataReceiptsCompletion_ForJpeg_DoesNotGenerateAdditionalData()
        {
            //Arrange & Act
            var additionalData = GenerateAdditionalDataCompletionTest("image/jpeg");

            // Assert
            Assert.IsNull(additionalData);
        }

        private static ReceiptsApiData GenerateAdditionalDataPrecompletionTest(string mimeType)
        {
            var byteList = new List<byte>
            {
                Convert.ToByte(22)
            };
            var segmentList = new List<ReceiptSegmentType> { new ReceiptSegmentType { MimeData = byteList, Sequence = -1 } };

            var preCompletionReceiptType = new PreCompletionReceiptType
            {
                ReceiptMimeType = mimeType,
                Disclosure1MimeData = segmentList,
                Disclosure2MimeData = segmentList
            };
            var mockReceiptIntegration = new Mock<IReceiptIntegration>();
            mockReceiptIntegration.Setup(x => x.MergePdf(It.IsAny<List<byte[]>>())).Returns(() => new byte[1]);

            return ReceiptHelper.GenerateAdditionalDataReceipts(preCompletionReceiptType, mockReceiptIntegration.Object);
        }

        private static ReceiptsApiData GenerateAdditionalDataCompletionTest(string mimeType)
        {
            var byteList = new List<byte>
            {
                Convert.ToByte(22)
            };
            var segmentList = new List<ReceiptSegmentType> { new ReceiptSegmentType { MimeData = byteList, Sequence = -1 } };

            var preCompletionReceiptType = new CompletionReceiptType
            {
                ReceiptMimeType = mimeType,
                ConsumerReceipt1MimeData = segmentList,
                ConsumerReceipt2MimeData = segmentList
            };
            var mockReceiptIntegration = new Mock<IReceiptIntegration>();
            mockReceiptIntegration.Setup(x => x.MergePdf(It.IsAny<List<byte[]>>())).Returns(() => new byte[1]);

            return ReceiptHelper.GenerateAdditionalDataReceipts(preCompletionReceiptType, mockReceiptIntegration.Object);
        }
    }
}
