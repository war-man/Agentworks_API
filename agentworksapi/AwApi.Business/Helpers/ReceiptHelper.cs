using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwApi.Business.Helpers
{
    public static class ReceiptHelper
    {
        public static ReceiptsApiData GenerateAdditionalDataReceipts(CompletionReceiptType receipts,
            IReceiptIntegration receiptIntegration)
        {
            var textReceipt = string.Empty;
            var isAdditionalData = false;
            if (receipts != null)
            {
                var mimeType = IdentifyReceiptMimeType(receipts.ReceiptMimeType);

                // Dont process Images or PDF's
                if (mimeType == MimeTypeEnum.ImageJPEG || mimeType == MimeTypeEnum.Pdf)
                {
                    return null;
                }

                var receiptSegments = GetReceiptSegments(receipts.AgentReceiptMimeData,
                    receipts.ConsumerReceipt1MimeData, receipts.ConsumerReceipt2MimeData);
                var receipt = ProcessReceipt(receiptSegments, receipts.ReceiptMimeType, receiptIntegration);

                if (mimeType == MimeTypeEnum.Thermal)
                {
                    textReceipt = ProcessThermalReceipt(receipt, receipts.CharsetEncoding);
                }
                else if (mimeType == MimeTypeEnum.Text)
                {
                    textReceipt = ProcessTextReceipt(receipt, receipts.CharsetEncoding);
                }
                isAdditionalData = !string.IsNullOrWhiteSpace(textReceipt);
            }
            return isAdditionalData
                ? new ReceiptsApiData
                {
                    TextReceipt = textReceipt
                }
                : null;
        }

        public static ReceiptsApiData GenerateAdditionalDataReceipts(PreCompletionReceiptType receipts,
            IReceiptIntegration receiptIntegration)
        {
            var textReceipt = string.Empty;
            var isAdditionalData = false;
            if (receipts != null)
            {
                var mimeType = IdentifyReceiptMimeType(receipts.ReceiptMimeType);

                // Dont process Images or PDF's
                if (mimeType == MimeTypeEnum.ImageJPEG || mimeType == MimeTypeEnum.Pdf)
                {
                    return null;
                }

                var receiptSegments = GetReceiptSegments(receipts.Disclosure1MimeData, receipts.Disclosure2MimeData);
                var receipt = ProcessReceipt(receiptSegments, receipts.ReceiptMimeType, receiptIntegration);

                if (mimeType == MimeTypeEnum.Thermal)
                {
                    textReceipt = ProcessThermalReceipt(receipt, receipts.CharsetEncoding);
                }
                else if (mimeType == MimeTypeEnum.Text)
                {
                    textReceipt = ProcessTextReceipt(receipt, receipts.CharsetEncoding ?? "UTF-8");
                }
                isAdditionalData = !string.IsNullOrWhiteSpace(textReceipt);
            }
            return isAdditionalData
                ? new ReceiptsApiData
                {
                    TextReceipt = textReceipt
                }
                : null;
        }

        public static string ProcessTextReceipt(List<byte[]> receiptData, string charsetEncoding = "UTF-8")
        {
            var encoding = Encoding.GetEncoding(charsetEncoding);
            var receiptText = string.Empty;
            var sb = new StringBuilder();
            foreach (var byteArray in receiptData)
            {
                if (byteArray.Length > 0)
                {
                    var decodedString = encoding.GetString(byteArray);
                    sb.Append(decodedString);
                }
            }
            if (sb.Length > 0)
            {
                receiptText = sb.ToString();
            }
            return receiptText;
        }

        public static string ProcessThermalReceipt(List<byte[]> receiptData, string charsetEncoding)
        {
            var encoding = Encoding.GetEncoding(charsetEncoding);
            var receiptText = string.Empty;
            var sb = new StringBuilder();
            foreach (var byteArray in receiptData)
            {
                if (byteArray.Length > 0)
                {
                    var receiptDataString = encoding.GetString(byteArray);
                    sb.Append(receiptDataString);
                }
            }
            if (sb.Length > 0)
            {
                var receiptBytes = encoding.GetBytes(sb.ToString());
                receiptText = Convert.ToBase64String(receiptBytes);
            }
            return receiptText;
        }

        public static List<byte[]> ProcessReceipt(List<List<ReceiptSegmentType>> receiptSegments, string mimeType,
            IReceiptIntegration receiptIntegration)
        {
            var receiptMimeType = IdentifyReceiptMimeType(mimeType);
            List<byte[]> receipt = new List<byte[]>();
            if (receiptSegments.Any())
            {
                var combinedSegments = receiptSegments.SelectMany(i => i).ToArray();
                for (var seg = 0; seg < combinedSegments.Length; seg++)
                {
                    if(combinedSegments[seg].MimeData != null && combinedSegments[seg].MimeData.Count > 0)
                    {
                        receipt.Add(combinedSegments[seg].MimeData.ToArray());
                    }
                }
            }

            // return segregated byte[] data for images.
            return receipt;
        }

        public static MimeTypeEnum IdentifyReceiptMimeType(string mimeType)
        {
            if (mimeType.ToLower().Contains("gif"))
            {
                return MimeTypeEnum.ImageGIF;
            }
            else if (mimeType.ToLower().Contains("jpeg"))
            {
                return MimeTypeEnum.ImageJPEG;
            }
            else if (mimeType.ToLower().Contains("thermal"))
            {
                return MimeTypeEnum.Thermal;
            }
            else if (mimeType.ToLower().Contains("text"))
            {
                return MimeTypeEnum.Text;
            }
            else
            {
                return MimeTypeEnum.Pdf;
            }
        }

        public static List<List<ReceiptSegmentType>> GetReceiptSegments(
            params List<ReceiptSegmentType>[] receiptSegments)
        {
            var segmentList = receiptSegments.Where(x => x != null && x.Any()).ToList();
            return segmentList;
        }
    }
}