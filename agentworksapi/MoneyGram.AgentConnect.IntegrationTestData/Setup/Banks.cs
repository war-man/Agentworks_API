using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class Banks
    {
        // Dictionary of country AND banks
        private static Dictionary<string, Dictionary<string, List<BankDetail>>> BankData => 
            new Dictionary<string, Dictionary<string, List<BankDetail>>>
        {
            {
                AcEnv.D2,
                new Dictionary<string, List<BankDetail>>
                {
                    {
                        Country.India,
                        // BankDetail for 301567
                        new List<BankDetail>
                        {
                            {
                                new BankDetail
                                {
                                    BankInfo = new HierarchyLevelInfo
                                    {
                                        HierarchyLevelElementNumber = 301567,
                                        HierarchyLevelValue = "ABHYUDAYA CO-OPERATIVE BANK"
                                    },
                                    BankStates = new List<BankState>
                                    {
                                        new BankState
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 302836,
                                                HierarchyLevelValue = "GUJARAT"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 366240,
                                                        HierarchyLevelValue = "AHMEDABAD"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber =  387104,
                                                            HierarchyLevelValue = "MANEKCHOWK",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "DAHINI KHADKI, MANEKCHOWK, AHMEDABAD- 380 221."
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065203"
                                                                }
                                                            }
                                                        },
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber =  366241,
                                                            HierarchyLevelValue = "RAIPUR",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "BLOCK NO.E/1, GHANTAKARNA MAHAVIR CLOTH MARKET, RAIPUR, AHMEDABAD- 380 002."
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065204"
                                                                }
                                                            }
                                                        }
                                                    }
                                                },
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 492137,
                                                        HierarchyLevelValue = "MANDVI / VADODARA"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 492138,
                                                            HierarchyLevelValue = "MANDVI VADODARA",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "SHREE KRISHNA BHAVAN, NEAR CHAMPANER GATE,  3259 BANK ROAD, VADODARA 390006,"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065201"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new BankState
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 301568,
                                                HierarchyLevelValue = "MAHARASHTRA"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 309557,
                                                        HierarchyLevelValue = "MUMBAI"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 497289,
                                                            HierarchyLevelValue = "ANDHERI",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "SHRADHA SHOPPING CENTRE, OLD NAGARDAS ROAD, ANDHERI (E), MUMBAI-400069"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065034"
                                                                }
                                                            }
                                                        },
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 452449,
                                                            HierarchyLevelValue = "WORLI",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "LANDMARK,NEXT TO MAHINDRA TOWERS, PLOT NO.1, J M BHOSLE MARG, WORLI, MUMBAI-400018"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065013"
                                                                }
                                                            }
                                                        }
                                                    }
                                                },
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 322334,
                                                        HierarchyLevelValue = "PUNE"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 331571,
                                                            HierarchyLevelValue = "LAXMI ROAD, PUNE",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "867, BUDHWAR PETH, LAXMI ROAD, PUNE-411002"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065103"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            },
                            // BankDetail for 331116
                            {
                                new BankDetail
                                {
                                    BankInfo = new HierarchyLevelInfo
                                    {
                                        HierarchyLevelElementNumber = 331116,
                                        HierarchyLevelValue = "ABU DHABI COMMERCIAL BANK"
                                    },
                                    BankStates = new List<BankState>
                                    {
                                        new BankState
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 376335,
                                                HierarchyLevelValue = "KARNATAKA"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 376336,
                                                        HierarchyLevelValue = "BENGALURU (BANGALORE)"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 376337,
                                                            HierarchyLevelValue = "BANGALORE BRANCH",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "CITI CENTRE, NO.28, CHURCH STREET, BANGALORE-560001"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ADCB0000002"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new BankState()
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 331117,
                                                HierarchyLevelValue = "MAHARASHTRA"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 331118,
                                                        HierarchyLevelValue = "MUMBAI"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 331119,
                                                            HierarchyLevelValue = "MUMBAI BRANCH",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "75, REHMAT MANZIL, VEER NARIMAN ROAD, CHURCHGATE, MUMBAI - 400020"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ADCB0000001"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            {
                AcEnv.Q3,
                new Dictionary<string, List<BankDetail>>
                {
                    {
                        Country.India,
                        // BankDetail for 301567
                        new List<BankDetail>
                        {
                            {
                                new BankDetail
                                {
                                    BankInfo = new HierarchyLevelInfo
                                    {
                                        HierarchyLevelElementNumber = 301567,
                                        HierarchyLevelValue = "ABHYUDAYA CO-OPERATIVE BANK"
                                    },
                                    BankStates = new List<BankState>
                                    {
                                        new BankState
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 302836,
                                                HierarchyLevelValue = "GUJARAT"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 366240,
                                                        HierarchyLevelValue = "AHMEDABAD"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber =  387104,
                                                            HierarchyLevelValue = "MANEKCHOWK",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "DAHINI KHADKI, MANEKCHOWK, AHMEDABAD- 380 221."
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065203"
                                                                }
                                                            }
                                                        },
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber =  366241,
                                                            HierarchyLevelValue = "RAIPUR",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "BLOCK NO.E/1, GHANTAKARNA MAHAVIR CLOTH MARKET, RAIPUR, AHMEDABAD- 380 002."
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065204"
                                                                }
                                                            }
                                                        }
                                                    }
                                                },
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 492137,
                                                        HierarchyLevelValue = "MANDVI / VADODARA"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 492138,
                                                            HierarchyLevelValue = "MANDVI VADODARA",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "SHREE KRISHNA BHAVAN, NEAR CHAMPANER GATE,  3259 BANK ROAD, VADODARA 390006,"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065201"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new BankState
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 301568,
                                                HierarchyLevelValue = "MAHARASHTRA"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 309557,
                                                        HierarchyLevelValue = "MUMBAI"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 497289,
                                                            HierarchyLevelValue = "ANDHERI",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "SHRADHA SHOPPING CENTRE, OLD NAGARDAS ROAD, ANDHERI (E), MUMBAI-400069"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065034"
                                                                }
                                                            }
                                                        },
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 452449,
                                                            HierarchyLevelValue = "WORLI",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "LANDMARK,NEXT TO MAHINDRA TOWERS, PLOT NO.1, J M BHOSLE MARG, WORLI, MUMBAI-400018"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065013"
                                                                }
                                                            }
                                                        }
                                                    }
                                                },
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 322334,
                                                        HierarchyLevelValue = "PUNE"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 331571,
                                                            HierarchyLevelValue = "LAXMI ROAD, PUNE",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "867, BUDHWAR PETH, LAXMI ROAD, PUNE-411002"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ABHY0065103"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            },
                            // BankDetail for 331116
                            {
                                new BankDetail
                                {
                                    BankInfo = new HierarchyLevelInfo
                                    {
                                        HierarchyLevelElementNumber = 331116,
                                        HierarchyLevelValue = "ABU DHABI COMMERCIAL BANK"
                                    },
                                    BankStates = new List<BankState>
                                    {
                                        new BankState
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 376335,
                                                HierarchyLevelValue = "KARNATAKA"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 376336,
                                                        HierarchyLevelValue = "BENGALURU (BANGALORE)"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 376337,
                                                            HierarchyLevelValue = "BANGALORE BRANCH",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "CITI CENTRE, NO.28, CHURCH STREET, BANGALORE-560001"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ADCB0000002"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new BankState()
                                        {
                                            StateInfo = new HierarchyLevelInfo
                                            {
                                                HierarchyLevelElementNumber = 331117,
                                                HierarchyLevelValue = "MAHARASHTRA"
                                            },
                                            BankCities = new List<BankCity>
                                            {
                                                new BankCity
                                                {
                                                    CityInfo = new HierarchyLevelInfo
                                                    {
                                                        HierarchyLevelElementNumber = 331118,
                                                        HierarchyLevelValue = "MUMBAI"
                                                    },
                                                    BankBranches = new List<HierarchyLevelInfo>
                                                    {
                                                        new HierarchyLevelInfo
                                                        {
                                                            HierarchyLevelElementNumber = 331119,
                                                            HierarchyLevelValue = "MUMBAI BRANCH",
                                                            Attributes = new List<AttributeType>
                                                            {
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "ADDRESS",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "75, REHMAT MANZIL, VEER NARIMAN ROAD, CHURCHGATE, MUMBAI - 400020"
                                                                },
                                                                new AttributeType
                                                                {
                                                                    AttributeLabel = "IFSC",
                                                                    AttributeTag = null,
                                                                    AttributeValue = "ADCB0000001"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private static long GetBankNumber(BankIdentifier bankIdentifier)
        {
            switch (bankIdentifier)
            {
                case BankIdentifier.ABHY:
                    return 301567;

                case BankIdentifier.ADCB:
                    return 331116;

                default:
                    return 0;
            }
        }

        public static BankDetail GetBank(string countryCode, BankIdentifier bankId)
        {
            var bankNumber = GetBankNumber(bankId);
            var testSettings = TestConfig.TestSettings;
            return BankData.ContainsKey(testSettings.AcEnvironment) && 
                BankData[testSettings.AcEnvironment].ContainsKey(countryCode) && BankData[testSettings.AcEnvironment][countryCode].Exists(x => x.BankInfo.HierarchyLevelElementNumber == bankNumber) ?
                    BankData[testSettings.AcEnvironment][countryCode].Find(x => x.BankInfo.HierarchyLevelElementNumber == bankNumber): null;
        }
    }
}