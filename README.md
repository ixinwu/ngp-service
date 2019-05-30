# ngp-service
后端开发工程，包含脚手架功能，配置驱动数据处理，各种中间件集成
用于快速搭建应用服务

# docker 发布
- 在根目录下打开 powershell
- 执行命令 docker build -t aspnetapp .
- 执行命令运行容器 docker run -d -p 8099:80 --name ngp aspnetapp

# 以下是每个步骤的结果
- 步骤1/10：从microsoft / dotnet：sdk AS build-env 
  从docker hub上的Microsoft / dotnet注册表中用标签“sdk”拉出图像 - https://hub.docker.com/r/microsoft/dotnet/tags
  “build-env”名称可以赋予新的构建阶段。这将用于在后面的步骤中复制文件。

- 步骤2/10：WORKDIR / app 
  WORKDIR指令设置Dockerfile中跟随它的任何RUN，CMD，ENTRYPOINT，COPY和ADD指令的工作目录。如果WORKDIR不存在，即使它未在任何后续Dockerfile指令中使用，也将创建它。
  
- 步骤3/10：COPY * .csproj ./将
  所有CS项目文件复制到容器内的工作目录。
  
- 步骤4/10：运行dotnet还原
  这是.NET命令行，它可以恢复构建项目所需的所有Nuget包。
  
- 步骤5/10：复制。./ 
  这会将Visual Studio项目的所有内容复制到工作目录中。
 
- 步骤6/10：运行dotnet发布-c Release -o out 
  此步骤使用.NET CLI创建发布版本，输出目录为“out”
  
- 步骤7/10：微软/ DOTNET：aspnetcore运行时
  与微软/ DOTNET注册表标签“aspnetcore运行时”拉动泊坞窗枢纽的形象- https://hub.docker.com/r/microsoft/dotnet/tags这提供运行ASP.NET Core Web应用程序的最少组件。
  
- 步骤8/10：WORKDIR / app 
  与上面的步骤2相同。
  
- 步骤9/10：COPY -from = build-env / app / out。
  这会将/ app / out文件夹的内容从“build-env”复制到此网络容器。
  
- 步骤10/10：ENTRYPOINT [“dotnet”，“aspnetapp.dll”] 
  这允许容器作为可执行文件运行。
