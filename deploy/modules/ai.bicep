param project_name string
param location string
param sku string
param retentionInDays int
param dailyQuotaGb int

var name = 'ai-${project_name}'
var law_name = 'law-${project_name}'

resource law 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: law_name
  location: location
  properties: {
    sku: {
      name: sku
    }
    retentionInDays: retentionInDays
    workspaceCapping: {
      dailyQuotaGb: dailyQuotaGb
    }
  }
}

resource insights 'microsoft.insights/components@2020-02-02' = {
  name: name
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: law.id
  }
}

output instrumentationKey string = insights.properties.InstrumentationKey
output connectionString string = insights.properties.ConnectionString
output insights_id string = insights.id
