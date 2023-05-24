param location string

resource maps 'Microsoft.Maps/accounts@2021-12-01-preview' = {
  kind: 'Gen2'
  sku: {
    name: 'G2'
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
  name: 'maps'
  location: location
  identity: {
    type: 'None'
  }
}

output primaryKey string = maps.listKeys().primaryKey
