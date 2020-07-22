provider "azurerm" {
  version = "~>2.19"
  features {}
}

locals {
  name                  = "martalist"
  service_plan_sku_tier = "Basic"
  service_plan_sku_size = "B1"
}

data "azurerm_resource_group" "main" {
  name = local.name
}

resource "azurerm_storage_account" "main" {
  name                     = local.name
  resource_group_name      = data.azurerm_resource_group.main.name
  location                 = data.azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_application_insights" "main" {
  name                = local.name
  location            = data.azurerm_resource_group.main.location
  resource_group_name = data.azurerm_resource_group.main.name
  application_type    = "web"
}

resource "azurerm_app_service_plan" "main" {
  name                = local.name
  location            = data.azurerm_resource_group.main.location
  resource_group_name = data.azurerm_resource_group.main.name
  kind                = "Linux"
  reserved            = true


  sku {
    tier = local.service_plan_sku_tier
    size = local.service_plan_sku_size
  }
}

resource "azurerm_app_service" "main" {
  name                = local.name
  location            = data.azurerm_resource_group.main.location
  resource_group_name = data.azurerm_resource_group.main.name
  app_service_plan_id = azurerm_app_service_plan.main.id

  client_affinity_enabled = false
  https_only              = true

  app_settings = {
    "ConnectionStrings__AzureTable" = azurerm_storage_account.main.primary_connection_string
    "APPINSIGHTS_KEY"               = azurerm_application_insights.main.instrumentation_key
  }

  # site_config {
  #   linux_fx_version = "DOCKER|appsvcsample/static-site:latest"
  #   always_on        = "true"
  # }

  identity {
    type = "SystemAssigned"
  }
}
