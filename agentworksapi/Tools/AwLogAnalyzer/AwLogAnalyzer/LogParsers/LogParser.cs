using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AwLogAnalyzer.Models;

namespace AwLogAnalyzer.LogParsers
{
    public class LogParser : ILogParser
    {
        private readonly Regex bracketRegex = new Regex(RegexStringConstants.Brackets, RegexOptions.Compiled);
        private readonly Regex tranRegex = new Regex(RegexStringConstants.Transaction, RegexOptions.Compiled);
        private readonly Regex serverIpRegex = new Regex(RegexStringConstants.ServerIP, RegexOptions.Compiled);
        private readonly Regex usernameRegex = new Regex(RegexStringConstants.UserName, RegexOptions.Compiled);
        private readonly Regex apiRegex = new Regex(RegexStringConstants.Api, RegexOptions.Compiled);
        private readonly Regex elapsedTimeRegex = new Regex(RegexStringConstants.ElapsedTime, RegexOptions.Compiled);
        private readonly Regex soapCallRegex = new Regex(RegexStringConstants.Soap, RegexOptions.Compiled);
        private readonly Regex cacheActionRegex = new Regex(RegexStringConstants.Cache, RegexOptions.Compiled);
        private readonly Regex actionRegex = new Regex(RegexStringConstants.Action, RegexOptions.Compiled);

        public Task<Dictionary<string, List<Record>>> ParseLogFile(string fileNames, Action<string> onUpdate,
            DateTime? startTime = null, DateTime? endTime = null)
        {
            // 1 - DateTime
            // 2 - AgentWorks.nxt
            // 3 - Server Ip
            // 4 - UserName
            // 5 - Method
            // 6 - INFO
            // 7 - TransactionId
            return Task.Run(() =>
            {
                var requests = new Dictionary<string, List<Record>>();
                var files = fileNames.Split(';')
                    .Select(fileName => File.ReadAllText(fileName))
                    .Select(file => file.Split(new[] {Environment.NewLine, "\r", "\n"}, StringSplitOptions.None))
                    .ToList();

                var totalLines = files.Select(x => x.Length).Sum();
                var linesRead = 0;

                foreach (var file in files)
                {
                    foreach (var line in file)
                    {
                        linesRead++;

                        var percentComplete = ((decimal) linesRead / totalLines) * 100;
                        onUpdate($"Parsing Files: {(int) percentComplete}% Complete");

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        var record = new Record();

                        var bracketedEntries = bracketRegex.Matches(line);

                        ProcessTimestamp(record, bracketedEntries[0]);
                        ProcessApplicationName(record, bracketedEntries[1]);
                        ProcessServerIp(record, bracketedEntries[2]);
                        ProcessUserName(record, bracketedEntries[3]);
                        ProcessMethod(record, bracketedEntries[4]);
                        ProcessTransactionId(record, bracketedEntries[6]);
                        
                        if (ShouldIgnoreRecord(record.DateStamp, startTime, endTime))
                        {
                            continue;
                        }

                        // Check for begin and end transaction regex and or api.
                        if (apiRegex.IsMatch(line))
                        {
                            record.ApiName = apiRegex.Match(line).Groups[1].Value;
                        }

                        if (line.Contains("Begin Request"))
                        {
                            record.IsBeginRequest = true;
                        }

                        if (line.Contains("End Request"))
                        {
                            record.IsEndRequest = true;
                        }

                        if (line.Contains("urn:AgentConnect"))
                        {
                            var soapCall = soapCallRegex.Match(line).Groups[1].Value;
                            // Soap request
                            record.IsSoap = true;
                            record.SoapCall = soapCall;
                        }

                        if (record.IsEndRequest && elapsedTimeRegex.IsMatch(line))
                        {
                            var elapsedTime = elapsedTimeRegex.Match(line).Groups[1].Value;
                            record.TotalElapsedMilliseconds = int.Parse(elapsedTime);
                        }
                        else if (elapsedTimeRegex.IsMatch(line))
                        {
                            var elapsedTime = elapsedTimeRegex.Match(line).Groups[1].Value;
                            record.ElapsedMilliseconds = int.Parse(elapsedTime);
                        }

                        if (!record.IsBeginRequest && actionRegex.IsMatch(line))
                        {
                            var action = actionRegex.Match(line).Groups[2].Value;
                            record.MethodAction = action;
                        }

                        if (requests.ContainsKey(record.transactionId))
                        {
                            requests[record.transactionId].Add(record);
                        }
                        else
                        {
                            var ListOfRecords = new List<Record>();
                            ListOfRecords.Add(record);
                            requests.Add(record.transactionId, ListOfRecords);
                        }
                    }
                }

                return requests;
            });
        }

        private void ProcessTimestamp(Record record, Match match)
        {
            var value = GetBracketValue(match);

            if (value == null)
            {
                return;
            }

            record.DateStamp = DateTime.Parse(value);
        }

        private void ProcessApplicationName(Record record, Match match)
        {
            var value = GetBracketValue(match);

            record.ApplicationName = value ?? string.Empty;
        }

        private void ProcessServerIp(Record record, Match match)
        {
            var value = GetBracketValue(match, serverIpRegex);

            record.ServerIp = value ?? string.Empty;
        }

        private void ProcessUserName(Record record, Match match)
        {
            var value = GetBracketValue(match, usernameRegex);

            record.UserName = value ?? string.Empty;
        }

        private void ProcessMethod(Record record, Match match)
        {
            var value = GetBracketValue(match);

            record.Method = value ?? string.Empty;
        }

        private void ProcessTransactionId(Record record, Match match)
        {
            var value = GetBracketValue(match, tranRegex);

            record.transactionId = value ?? string.Empty;
        }

        private string GetBracketValue(Match match, Regex valueExtractionRegex = null)
        {
            var value = match.Groups[1].Value;

            if (string.IsNullOrWhiteSpace(value) || value.ToLower().Trim() == "(null)")
            {
                return null;
            }

            if (valueExtractionRegex != null)
            {
                var valueMatch = valueExtractionRegex.Match(value);
                return GetBracketValue(valueMatch);
            }

            return value;
        }

        private bool ShouldIgnoreRecord(DateTime dateTime, DateTime? startTime, DateTime? endTime)
        {
            var beforeStartTime =
                startTime != null &&
                DateTime.Compare(dateTime, (DateTime) startTime) < 0;

            var afterEndTime =
                endTime != null &&
                DateTime.Compare(dateTime, (DateTime) endTime) > 0;

            return beforeStartTime || afterEndTime;
        }
    }
}