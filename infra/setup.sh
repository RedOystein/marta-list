#!/bin/bash

# This script creates the resource group, azure data storage (for tf state) and service principal (for CD)

RG_NAME="martalist"
RG_LOCATION="westeurope"
STORAGE_ACCOUNT_NAME="martaliststate"
CONTAINER_NAME="state"

echo "Creating resource group ${RG_NAME} in $RG_LOCATION.."
az group create -n $RG_NAME -l $RG_LOCATION
echo "Completed creation of resource group ${RG_NAME}"

echo "Creating storage account ${STORAGE_ACCOUNT_NAME}, this will take a while.."
az storage account create -g $RG_NAME -n $STORAGE_ACCOUNT_NAME --sku Standard_LRS --encryption-services blob
echo "Completed creation of storage account ${STORAGE_ACCOUNT_NAME}"

echo "Setting ARM_ACCESS_KEY environment variable.."
ARM_ACCESS_KEY=$(az storage account keys list -g $RG_NAME --account-name $STORAGE_ACCOUNT_NAME --query [0].value -o tsv)

echo "Creating container ${CONTAINER_NAME} for storing tfstate files"
az storage container create -n $CONTAINER_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $ARM_ACCESS_KEY

RG_ID=$(az group show -n ${RG_NAME} --query 'id' -o tsv)
SP_INFO=$(az ad sp create-for-rbac -n "${RG_NAME}-deployer" --role contributor --scopes $RG_ID)

cat << EOF
  ARM_CLIENT_ID: $(echo $SP_INFO | jq '.appId')
  ARM_CLIENT_SECRET: $(echo $SP_INFO | jq '.password')
  ARM_TENANT_ID: $(echo $SP_INFO | jq '.tenant')
  ARM_ACCESS_KEY: $(echo $ARM_ACCESS_KEY)
EOF