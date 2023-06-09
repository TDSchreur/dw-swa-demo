param project_name string
param location string
param sku string
// param functionAppid string
param mapskey string
param application_insights_instrumentation_key string
param application_insights_connection_string string

var name = 'swa-${project_name}'

resource swa 'Microsoft.Web/staticSites@2021-01-15' = {
  name: name
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {}
  sku: {
    name: sku
    size: sku
  }

  resource staticWebAppSettings 'config@2021-01-15' = {
    name: 'appsettings'
    properties: {
      APPINSIGHTS_INSTRUMENTATIONKEY: application_insights_instrumentation_key
      APPLICATIONINSIGHTS_CONNECTION_STRING: application_insights_connection_string
      MapsKey: mapskey
    }
  }
}

output identity string = swa.identity.principalId

#disable-next-line outputs-should-not-contain-secrets
output deployment_token string = listSecrets(swa.id, swa.apiVersion).properties.apiKey
