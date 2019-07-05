using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MoneyGram.Common.Models;

namespace MoneyGram.Common.Cache
{
    public static class CacheDataExtensions
    {
        public static List<Claim> ToClaims(this List<KeyValueItem<string>> keyValueList)
        {
            return keyValueList.Select(keyValue => new Claim(keyValue.Key, keyValue.Val)).ToList();
        }

        public static List<Claim> ToClaims(this List<Tuple<string, string>> serializedClaims)
        {
            return serializedClaims.Select(claim => new Claim(claim.Item1, claim.Item2)).ToList();
        }

        public static List<KeyValueItem<string>> ToKeyValueItems(this List<Claim> claims)
        {
            return claims.Select(claim => new KeyValueItem<string> { Key = claim.Type, Val = claim.Value }).ToList();
        }

        public static List<Tuple<string, string>> ToTupleList(this List<Claim> claims)
        {
            return claims.Select(claim => new Tuple<string, string>(claim.Type, claim.Value)).ToList();
        }
    }
}