using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using NLog.Config;

namespace NLog.Targets.SNS
{
	[Target("SNS")]
	public class SnsTarget : TargetWithLayout
	{
		private AmazonSimpleNotificationServiceClient _amazonSnsClient;

		[RequiredParameter]
		public string Region { get; set; }
		[RequiredParameter]
		public string TopicArn { get; set; }

		public string AwsCredentialsType { get; set; }
		public string AccessKey { get; set; }
		public string SecretKey { get; set; }
		public string Format { get; set; }
		public string Subject { get; set; }

		protected override void InitializeTarget()
		{
			base.InitializeTarget();

			_amazonSnsClient =
				new AmazonSimpleNotificationServiceClient(GetAwsCredentials(), RegionEndpoint.GetBySystemName(Region));
		}

		private AWSCredentials GetAwsCredentials()
		{
			var credentialType = (string.IsNullOrWhiteSpace(AwsCredentialsType)
									 ? null
									 : Type.GetType(AwsCredentialsType))
								 ?? typeof(BasicAWSCredentials);

			if (!string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(SecretKey) &&
				credentialType == typeof(BasicAWSCredentials))
				return new BasicAWSCredentials(AccessKey, SecretKey);

			return (AWSCredentials)Activator.CreateInstance(credentialType);
		}

		protected override void Write(LogEventInfo logEvent)
		{
			if (_amazonSnsClient == null)
				throw new Exception("The AWS SNS Target is not initialized");

			var message = "json".Equals(Format, StringComparison.InvariantCultureIgnoreCase)
				? RenderJsonBody(logEvent)
				: Layout.Render(logEvent);

			Task.Run(() => PublishAsync(message)).Wait();
		}

		private string RenderJsonBody(LogEventInfo logEvent)
		{
			logEvent.Properties["logLevel"] = logEvent.Level;
			logEvent.Properties["loggerName"] = logEvent.LoggerName;
			logEvent.Properties["message"] = Layout.Render(logEvent);

			return JsonConvert.SerializeObject(logEvent.Properties);
		}

		private async Task<PublishResponse> PublishAsync(string message)
		{
			if (string.IsNullOrEmpty(message))
				return new PublishResponse();

			return await _amazonSnsClient.PublishAsync(new PublishRequest(TopicArn, message, Subject));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_amazonSnsClient?.Dispose();

			base.Dispose(disposing);
		}
	}
}