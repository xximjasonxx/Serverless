
param baseName string
param location string = resourceGroup().location
param rbacAssignments array = []
param containers array = []

@allowed([
  'BlobStorage'
  'StorageV2'
])
param kind string = 'StorageV2'

var storageAccountName = length('st${baseName}') > 25 ? substring('st${baseName}', 0, 24) : 'st${baseName}'
resource sa 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }

  kind: kind
  properties: {
  }
}

// do assignments
resource rbac_assigns 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for assignment in rbacAssignments: {
  name: guid(assignment.principalId, assignment.roleDefinitionId, resourceGroup().name)
  properties: {
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', assignment.roleDefinitionId)
    principalId: assignment.principalId
  }
}]

module created_containers 'br:crbicepmodulesjx01.azurecr.io/microsoft.storage/accounts/blob-container:1.0.1' = [for container in containers: {
  name: container.name
  params: {
    storageAccountName: sa.name
    containerName: container.name
    rbacAssignments: contains(container, 'rbacAssignments') ? container.rbacAssignments : []
  }
}]

// outputs
output storageAccountName string = sa.name
