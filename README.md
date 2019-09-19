# nlog-aws-sns-target

NLog AWS SNS Target for .Net Standard 2.0

Could be used both int .Net Framework and .Net Core applications.

Configuration example:

```
<targets>
    <target xsi:type="SNS"
        name="SnsTarget"
        region ="us-west-1"
        topicArn="arn:aws:sns:***"
        accessKey="***"
        secretKey="***"
        format="json"
        subject="Log message subject" />
</tatrgets>
<rules>
    <logger name="sns" minlevel="Info" writeTo="SnsTarget" final="true" />
</rules>
```

| Property name  | Description | Mandatory |
| ------------- | ------------- | --------- |
| region        | AWS region  | Yes |
| topicArn  | arn of SNS topic | Yes |
| AwsCredentialsType | "BasicAWSCredentials" (default) or "Amazon.Runtime.StoredProfileAWSCredentials, AWSSDK.Core" | No |
| accessKey  | Access key for BasicAWSCredentials | No |
| secretKey  | Secret key for BasicAWSCredentials | No |
| format  | "plainText" (default) or "json" | No |
| subject | Subject for SNS message | No |
| layout | string layout for "plainText" format | No |

Simple usage example:

```
var snsLogger = LogManager.GetLogger("sns");
snsLogger.Info("Hello world!");
```
