FROM node:14.4-alpine

WORKDIR /azure-storage/table

RUN npm i -g azurite@2.7.1

ENTRYPOINT [ "azurite-table" ]