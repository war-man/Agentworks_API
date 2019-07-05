using AwApi.Business.BusinessRules;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using System.Collections.Generic;
using System.Linq;

namespace AwApi.Business
{
    public class ConsumerBusiness : IConsumerBusiness
    {
        protected readonly IAgentConnectIntegration _agentConnectIntegration;
        protected readonly ILookupBusiness _lookupBusiness;

        public ConsumerBusiness(ILookupBusiness lookupBusiness, IAgentConnectIntegration agentConnectIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _agentConnectIntegration = agentConnectIntegration;
            _lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<ConsumerHistoryLookupResponse, ApiData> ConsumerHistoryLookup(ConsumerHistoryLookupRequest req)
        {
            req.ApplyBusinessRules();

            var resp = _agentConnectIntegration.ConsumerHistoryLookup(req);

            var apiResp = new AcApiResponse<ConsumerHistoryLookupResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<GetProfileReceiverResponse, ApiData> GetProfileReceiver(GetProfileReceiverRequest req)
        {
            var resp = _agentConnectIntegration.GetProfileReceiver(req);

            var apiResp = new AcApiResponse<GetProfileReceiverResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<GetProfileSenderResponse, ApiData> GetProfileSender(GetProfileSenderRequest req)
        {
            var resp = _agentConnectIntegration.GetProfileSender(req);

            var apiResp = new AcApiResponse<GetProfileSenderResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<CreateOrUpdateProfileReceiverResponse, ApiData> CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverRequest req)
        {
            var resp = _agentConnectIntegration.CreateOrUpdateProfileReceiver(req);

            var apiResp = new AcApiResponse<CreateOrUpdateProfileReceiverResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<CreateOrUpdateProfileSenderResponse, ApiData> CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderRequest req)
        {
            var resp = _agentConnectIntegration.CreateOrUpdateProfileSender(req);

            var apiResp = new AcApiResponse<CreateOrUpdateProfileSenderResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<GetProfileConsumerResponse, ApiData> GetProfileConsumer(GetProfileConsumerRequest req)
        {
            var resp = _agentConnectIntegration.GetProfileConsumer(req);

            var apiResp = new AcApiResponse<GetProfileConsumerResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }
        public AcApiResponse<CreateOrUpdateProfileConsumerResponse, ApiData> CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerRequest req)
        {
            var resp = _agentConnectIntegration.CreateOrUpdateProfileConsumer(req);

            var apiResp = new AcApiResponse<CreateOrUpdateProfileConsumerResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<SaveConsumerProfileImageResponse, ApiData> SaveConsumerProfileImage(SaveConsumerProfileImageRequest req)
        {
            var resp = _agentConnectIntegration.SaveConsumerProfileImage(req);

            var apiResp = new AcApiResponse<SaveConsumerProfileImageResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<SearchConsumerProfilesResponse, ApiData> SearchConsumerProfiles(SearchConsumerProfilesRequest req)
        {
            var resp = _agentConnectIntegration.SearchConsumerProfiles(req);

            var apiResp = new AcApiResponse<SearchConsumerProfilesResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<GetConsumerProfileTransactionHistoryResponse, ApiData> GetConsumerProfileTransactionHistory(GetConsumerProfileTransactionHistoryRequest req)
        {
            var resp = _agentConnectIntegration.GetConsumerProfileTransactionHistory(req);

            var apiResp = new AcApiResponse<GetConsumerProfileTransactionHistoryResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<SavePersonalIDImageResponse, ApiData> SavePersonalIDImage(ViewModels.Consumer.SavePersonalIDImageRequest vm)
        {
            // Convert from viewmodel to transaction model -- base64 string to bytearray.
            var imageItems = new List<ImageItemType>();
            foreach (var image in vm.PersonalIDImage.ImageItems)
            {
                if (!string.IsNullOrEmpty(image.Image)) //add only non-empty images
                {
                    imageItems.Add(new ImageItemType
                    {
                        Image = System.Convert.FromBase64String(image.Image).ToList(),
                        Label = image.Label
                    });
                }
            }

            if (imageItems.Count == 0) //photocopy flow
            {
                imageItems = null;
            }

            var personalIdType = new PersonalIDImageContentType
            {
                Identifier = vm.PersonalIDImage.Identifier,
                PersonalIDChoice = vm.PersonalIDImage.PersonalIDChoice,
                PersonalIDNumber = vm.PersonalIDImage.PersonalIDNumber,
                PersonalIDVerificationStr = vm.PersonalIDImage.PersonalIDVerificationStr,
                MimeType = vm.PersonalIDImage.MimeType,
                ImageItems = imageItems
            };
            var req = new SavePersonalIDImageRequest
            {
                AgentID = vm.AgentID,
                AgentSequence = vm.AgentSequence,
                ChannelType = vm.ChannelType,
                ClientSoftwareVersion = vm.ClientSoftwareVersion,
                ConsumerProfileID = vm.ConsumerProfileID,
                ConsumerProfileIDType = vm.ConsumerProfileIDType,
                Language = vm.Language,
                MgiSessionID = vm.MgiSessionID,
                OperatorName = vm.OperatorName,
                PoeCapabilities = vm.PoeCapabilities,
                PoeType = vm.PoeType,
                TargetAudience = vm.TargetAudience,
                TimeStamp = vm.TimeStamp,
                UnitProfileID = vm.UnitProfileID,
                PersonalIDImage = personalIdType
            };
            var resp = _agentConnectIntegration.SavePersonalIDImage(req);

            var apiResp = new AcApiResponse<SavePersonalIDImageResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<ViewModels.Consumer.GetPersonalIDImageResponse, ApiData> GetPersonalIDImage(GetPersonalIDImageRequest req)
        {
            var resp = _agentConnectIntegration.GetPersonalIDImage(req);

            var newImageItems = new List<ViewModels.Consumer.ImageItemType>();

            if (resp.Payload != null)
            {
                foreach (var image in resp.Payload.ImageItems)
                {
                    // Convert from byte[] to base64 string
                    newImageItems.Add(new ViewModels.Consumer.ImageItemType
                    {
                        Image = System.Convert.ToBase64String(image.Image.ToArray()),
                        Label = image.Label
                    });
                }
            }

            var GetPersonalIDImageResponsePayload = new ViewModels.Consumer.GetPersonalIDImageResponsePayload
            {
                ImageItems = newImageItems,
                MimeType = resp?.Payload?.MimeType
            };

            var newResp = new ViewModels.Consumer.GetPersonalIDImageResponse
            {
                Payload = GetPersonalIDImageResponsePayload,
                Errors = resp.Errors
            };

            var apiResp = new AcApiResponse<ViewModels.Consumer.GetPersonalIDImageResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = newResp
            };

            return apiResp;
        }

        public AcApiResponse<SaveConsumerProfileDocumentResponse, ApiData> SaveConsumerProfileDocument(ViewModels.Consumer.SaveConsumerProfileDocumentRequest req)
        {
            var imageItems = new List<ImageItemType>();

            foreach (var image in req.ConsumerProfileDocument.ImageItems)
            {
                if (!string.IsNullOrEmpty(image.Image)) //add only non-empty images
                {
                    imageItems.Add(new ImageItemType
                    {
                        Image = System.Convert.FromBase64String(image.Image).ToList(),
                        Label = image.Label
                    });
                }
            }

            if (imageItems.Count == 0) //photocopy flow
            {
                imageItems = null;
            }

            var consumerProfileDocument = new ConsumerProfileDocumentContentType
            {
                Identifier = req.ConsumerProfileDocument.Identifier,
                DocumentIssueDate = req.ConsumerProfileDocument.DocumentIssueDate,
                MimeType = req.ConsumerProfileDocument.MimeType,
                ImageItems = imageItems
            };

            var request = new SaveConsumerProfileDocumentRequest
            {
                AgentID = req.AgentID,
                AgentSequence = req.AgentSequence,
                ChannelType = req.ChannelType,
                ClientSoftwareVersion = req.ClientSoftwareVersion,
                ConsumerProfileID = req.ConsumerProfileID,
                ConsumerProfileIDType = req.ConsumerProfileIDType,
                Language = req.Language,
                MgiSessionID = req.MgiSessionID,
                OperatorName = req.OperatorName,
                PoeCapabilities = req.PoeCapabilities,
                PoeType = req.PoeType,
                TargetAudience = req.TargetAudience,
                TimeStamp = req.TimeStamp,
                UnitProfileID = req.UnitProfileID,
                ConsumerProfileDocument = consumerProfileDocument,
            };

            var response = _agentConnectIntegration.SaveConsumerProfileDocument(request);

            var apiResponse = new AcApiResponse<SaveConsumerProfileDocumentResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(response.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = response
            };

            return apiResponse;
        }

        public AcApiResponse<ViewModels.Consumer.GetConsumerProfileDocumentResponse, ApiData> GetConsumerProfileDocument(GetConsumerProfileDocumentRequest reqVm)
        {
            var response = _agentConnectIntegration.GetConsumerProfileDocument(reqVm);

            var newImageItems = new List<ViewModels.Consumer.ImageItemType>();

            if (response.Payload != null)
            {
                foreach (var image in response.Payload.ImageItems)
                {
                    newImageItems.Add(new ViewModels.Consumer.ImageItemType
                    {
                        Image = System.Convert.ToBase64String(image.Image.ToArray()),
                        Label = image.Label
                    });
                }
            }

            var getConsumerProfileDocumentResponsePayload = new ViewModels.Consumer.GetConsumerProfileDocumentResponsePayload
            {
                ImageItems = newImageItems,
                MimeType = response?.Payload?.MimeType
            };

            var getConsumerProfileDocumentResponse = new ViewModels.Consumer.GetConsumerProfileDocumentResponse()
            {
                Payload = getConsumerProfileDocumentResponsePayload,
                Errors = response.Errors
            };

            var apiResp = new AcApiResponse<ViewModels.Consumer.GetConsumerProfileDocumentResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(response.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = getConsumerProfileDocumentResponse
            };

            return apiResp;
        }
    }
}