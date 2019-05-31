FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./aspnetapp/
COPY ./Hosts/NGP.WebApi/App_Data/ /app/App_Data/
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/aspnetapp/Hosts/NGP.WebApi/out ./
ENTRYPOINT ["dotnet", "NGP.WebApi.dll"]