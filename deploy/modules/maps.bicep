param project_name string
param location string
param kind string
param sku string

var name = 'ai-${project_name}'

resource maps 'Microsoft.Maps/accounts@2021-12-01-preview' = {
  name: name
  location: location
  kind: kind
  sku: {
    name: sku
  }
  properties: {
    disableLocalAuth: false
    cors: {
      corsRules: [
        {
          allowedOrigins: []
        }
      ]
    }
  }
}

output primaryKey string = maps.listKeys().primaryKey
