
targetScope = 'resourceGroup'
param location string = resourceGroup().location

resource cogText 'Microsoft.CognitiveServices/accounts@2022-03-01' = {
  name: 'cog-lang-serverless-jx02'
  location: location
  sku: {
    name: 'S0'
  }

  identity: {
    type: 'SystemAssigned'
  }

  kind: 'CognitiveServices'

  properties: {
    apiProperties: {}
    customSubDomainName: 'cog-serverless-jx02'
    networkAcls: {
        defaultAction: 'Allow'
        virtualNetworkRules: []
        ipRules: []
    }
    publicNetworkAccess: 'Enabled'
  }
}
