# AWS SNS Target for NLog

Target Framework: .Net Standard 2.0

License: [MIT](https://raw.githubusercontent.com/RRomaNN/nlog-aws-sns-target/master/LICENSE).

Nuget: [Latest version](https://www.nuget.org/packages/NLog.Targets.AWS.SNS)

Could be used both in .Net Framework and .Net Core applications.

Configuration example:

```xml
<targets>
    <target xsi:type="SNS"
        name="SnsTarget"
        region ="us-west-1"
        topicArn="arn:aws:sns:***"
        accessKey="***"
        secretKey="***"
        format="json"
        subject="Log message subject"
        layout="${message}"/>
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
| layout | layout for the "message" property | No |

Simple usage example:

```cs 
var snsLogger = LogManager.GetLogger("sns");
snsLogger.Info("Hello world!");
```

!This is the draft version of documentation!
