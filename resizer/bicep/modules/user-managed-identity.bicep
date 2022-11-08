
param baseName string
param location string

resource id 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
  name: 'id-${baseName}'
  location: location
}

// outputs
output resourceId string = id.id
output resourceName string = id.name
output clientId string = id.properties.clientId
output principalId string = id.properties.principalId
