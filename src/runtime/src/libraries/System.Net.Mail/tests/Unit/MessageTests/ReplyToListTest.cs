// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Net.Mail.Tests
{
    public class ReplyToListTest
    {
        private Message _message = new Message();

        [Fact]
        public void ReplyToList_WithNoReplyToSet_ShouldReturnEmptyList()
        {
            Assert.Equal(0, _message.ReplyToList.Count);
        }

        [Fact]
        public void ReplyTo_WithReplyToListEmpty_ShouldReturnNull()
        {
            Assert.Null(_message.ReplyTo);
        }

        [Fact]
        public void PrepareHeaders_WithReplyToSet_ShouldIgnoreReplyToList()
        {
            MailAddress m = new MailAddress("test@example.com");
            MailAddress m2 = new MailAddress("test2@example.com");
            MailAddress m3 = new MailAddress("test3@example.com");

            _message.ReplyToList.Add(m);
            _message.ReplyToList.Add(m2);

            _message.ReplyTo = m3;

            _message.From = new MailAddress("from@example.com");

            _message.PrepareHeaders(false);

            Assert.Equal(2, _message.ReplyToList.Count);

            string[] s = _message.Headers.GetValues("Reply-To");
            Assert.Equal(1, s.Length);

            Assert.DoesNotContain("test@example.com", s[0]);
            Assert.DoesNotContain("test2@example.com", s[0]);
            Assert.Contains("test3@example.com", s[0]);
        }

        [Fact]
        public void PrepareHeaders_WithReplyToNull_AndReplyToListSet_ShouldUseReplyToList()
        {
            MailAddress m = new MailAddress("test@example.com");
            MailAddress m2 = new MailAddress("test2@example.com");
            MailAddress m3 = new MailAddress("test3@example.com");

            _message.ReplyToList.Add(m);
            _message.ReplyToList.Add(m2);
            _message.ReplyToList.Add(m3);

            _message.From = new MailAddress("from@example.com");

            _message.PrepareHeaders(false);

            Assert.Equal(3, _message.ReplyToList.Count);

            string[] s = _message.Headers.GetValues("Reply-To");
            Assert.Equal(1, s.Length);

            Assert.Contains("test@example.com", s[0]);
            Assert.Contains("test2@example.com", s[0]);
            Assert.Contains("test3@example.com", s[0]);

            Assert.Null(_message.ReplyTo);
        }

        [Fact]
        public void PrepareHeaders_WithReplyToListSet_AndReplyToHeaderSetManually_ShouldEnforceReplyToListIsSingleton()
        {
            MailAddress m = new MailAddress("test@example.com");
            MailAddress m2 = new MailAddress("test2@example.com");
            MailAddress m3 = new MailAddress("test3@example.com");

            _message.ReplyToList.Add(m);
            _message.ReplyToList.Add(m2);
            _message.ReplyToList.Add(m3);

            _message.Headers.Add("Reply-To", "shouldnotbeset@example.com");

            _message.From = new MailAddress("from@example.com");

            _message.PrepareHeaders(false);

            Assert.True(_message.ReplyToList.Count == 3, "ReplyToList did not contain all email addresses");

            string[] s = _message.Headers.GetValues("Reply-To");
            Assert.Equal(1, s.Length);

            Assert.Contains("test@example.com", s[0]);
            Assert.Contains("test2@example.com", s[0]);
            Assert.Contains("test3@example.com", s[0]);
            Assert.DoesNotContain("shouldnotbeset@example.com", s[0]);

            Assert.Null(_message.ReplyTo);
        }
    }
}
