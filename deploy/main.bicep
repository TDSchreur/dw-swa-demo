param location string = resourceGroup().location
param project_name string = 'dw-demo'

param swa_sku string

param law_sku string
param law_retentionInDays int
param law_dailyQuotaGb int

param maps_kind string
param maps_sku string

param func_sku_name string
param func_sku_tier string

param clientid string
@secure()
param clientsecret string

module maps './modules/maps.bicep' = {
  name: '${project_name}-maps'
  params: {
    project_name: project_name
    location: location
    kind: maps_kind
    sku: maps_sku
  }
}

module ai './modules/ai.bicep' = {
  name: '${project_name}-insights'
  params: {
    project_name: project_name
    location: location
    dailyQuotaGb: law_dailyQuotaGb
    retentionInDays: law_retentionInDays
    sku: law_sku
  }
}

module func 'modules/func.bicep' = {
  name: '${project_name}-func'
  params: {
    project_name: project_name
    location: location
    application_insights_instrumentation_key: ai.outputs.instrumentationKey
    application_insights_connection_string: ai.outputs.connectionString
    sku_name: func_sku_name
    sku_tier: func_sku_tier
  }
}

module swa './modules/swa.bicep' = {
  name: '${project_name}-swa'
  params: {
    project_name: project_name
    location: location
    mapskey: maps.outputs.primaryKey
    sku: swa_sku
    application_insights_instrumentation_key: ai.outputs.instrumentationKey
    application_insights_connection_string: ai.outputs.connectionString
    clientid: clientid
    clientsecret: clientsecret
    functionAppid: func.outputs.functionAppId
  }
}
