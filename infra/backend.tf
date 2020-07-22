terraform {
  backend "azurerm" {
    resource_group_name  = "martalist"
    storage_account_name = "martaliststate"
    container_name       = "state"
    key                  = "main.tfstate"
  }
}