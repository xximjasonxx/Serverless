
param location string = resourceGroup().location
param identityPrincipalId string
param containers array = []

resource sa 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: 'stserverlessimagesjx01'
  location: location
  sku: {
    name: 'Standard_LRS'
  }

  kind: 'StorageV2'
  properties: {
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2022-05-01' = {
  name: 'default'
  parent: sa
  properties: {
  }
}

// assigned reader
resource saReaderAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(identityPrincipalId, 'acdd72a7-3385-48ef-bd42-f606fba81ae7', resourceGroup().name)
  scope: sa
  properties: {
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', 'acdd72a7-3385-48ef-bd42-f606fba81ae7')
    principalId: identityPrincipalId
  }
}

// create containers
resource created_containers 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = [for name in containers: {
  name: name
  parent: blobService
  properties: {
    publicAccess: 'None'
  }
}]

// outputs
output storageAccountName string = sa.name

// do not do this in production (this is visible in Deployments)
output storageAccountKey string = listKeys(sa.id, sa.apiVersion).keys[0].value
