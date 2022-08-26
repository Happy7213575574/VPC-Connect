# Portal API

## Summary

* The staging portal API has a [swagger](https://vpc-app-endpoint.stimulize.co.uk/swagger/ui/index) view of the various endpoints.
* These endpoints require submission of data as a collection of separate POST parameters - not as a JSON object.
* When using the API, you must provide the following header: `API-Access: <api access key>`

NB. The API access key is not a strong security feature, but it has  helped to protect the API from unauthorised use during development. See [dev notes](dev-notes.md) for more information about `SensitiveConstants`.

## Portal embedded view

To view the portal website in an embedded view, you must provide this header:

```
User-Agent: phonegap/mvp-connect 1.0
```

NB. This is not required by the current app. Portal pages are displayed in a separate browser instance.
