# MoneyGram Auth Integration Guide

The purpose of this guide is to detail the steps required to integrate with the MoneyGram authentication/authorization repository.

## Authentication Middleware
The OpenAmAuthenticationMiddleware ensures that HTTP requests attempting to access an API are accompanied by a bearer token representing a valid session in OpenAM.

### Integration Steps
1. Add references to the MoneyGram.Common.Authorization.Http and MoneyGram.Common.Authorization.Http.Models projects to your WebAPI project
2. In your startup class, add the following block (substituting the OpenAmUrl and Realm for your configuration):
```
// Validates that each request contains a valid bearer token
app.Use<OpenAmAuthenticationMiddleware>(new OpenAmAuthenticationConfig
{
    OpenAmUrl = "http://dmnanlx7207.ad.moneygram.com:8080/",
    Realm = "mgiagents"
});
```