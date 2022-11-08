
param storageAccountName string
param containerName string
param rbacAssignments array = []

resource sa 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
  name: storageAccountName
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2022-05-01' = {
  name: 'default'
  parent: sa
  properties: {
  }
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  name: containerName
  parent: blobService
  properties: {
  }
}

resource roleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for assignment in rbacAssignments: {
  name: guid(assignment.principalId, assignment.roleDefinitionId, resourceGroup().name, containerName)
  scope: container
  properties: {
    principalType: 'ServicePrincipal'
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', assignment.roleDefinitionId)
    principalId: assignment.principalId
  }
}]
