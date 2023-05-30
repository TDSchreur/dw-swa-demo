param project_name string
param location string
param application_insights_instrumentation_key string
param application_insights_connection_string string
param mapskey string
param sku_name string
param sku_tier string

@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
])
param storageAccountType string = 'Standard_LRS'

var storage_name = 'stor${replace(project_name, '-', '')}'
var function_name = 'func-${project_name}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: storage_name
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'Storage'
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: function_name
  location: location
  kind: 'linux'
  sku: {
    name: sku_name
    tier: sku_tier
  }
  properties: {
    reserved: true // required for using linux
  }
}

resource functionApp 'Microsoft.Web/sites@2018-11-01' = {
  name: function_name
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|7.0'
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }

  resource appsettings 'config@2021-01-15' = {
    name: 'appsettings'
    properties: {
      AzureWebJobsStorage: 'DefaultEndpointsProtocol=https;AccountName=${storage_name};EndpointSuffix=${az.environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: 'DefaultEndpointsProtocol=https;AccountName=${storage_name};EndpointSuffix=${az.environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
      FUNCTIONS_EXTENSION_VERSION: '~4'
      WEBSITE_RUN_FROM_PACKAGE: '1'
      WEBSITE_CONTENTSHARE: toLower(function_name)
      APPINSIGHTS_INSTRUMENTATIONKEY: application_insights_instrumentation_key
      APPLICATIONINSIGHTS_CONNECTION_STRING: application_insights_connection_string
      MapsKey: mapskey
    }
  }
}

output functionAppId string = functionApp.id
