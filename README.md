# ngp-service
后端开发工程，包含脚手架功能，配置驱动数据处理，各种中间件集成.用于快速搭建应用服务
***
## 脚手架
***
## DSL
***
## 数据接口使用
***
## 其他说明
### Quartz.net 配置
#### Cron表达式范例：
- 每隔5秒执行一次：*/5 * * * * ?
- 每隔1分钟执行一次：0 */1 * * * ?
- 每天23点执行一次：0 0 23 * * ?
- 每天凌晨1点执行一次：0 0 1 * * ?
- 每月1号凌晨1点执行一次：0 0 1 1 * ?
- 每月最后一天23点执行一次：0 0 23 L * ?
- 每周星期天凌晨1点实行一次：0 0 1 ? * L
- 在26分、29分、33分执行一次：0 26,29,33 * * * ?
- 每天的0点、13点、18点、21点都执行一次：0 0 0,13,18,21 * * ?

#### 由7段构成：秒 分 时 日 月 星期 年（可选）
- "-" ：表示范围  MON-WED表示星期一到星期三
- "," ：表示列举 MON,WEB表示星期一和星期三
- "*" ：表是“每”，每月，每天，每周，每年等
- 对于星期里面，SUN=1  SAT=6
- "/" :表示增量：0/15（处于分钟段里面） 每15分钟，在0分以后开始，3/20 每20分钟，从3分钟以后开始
- "?" :只能出现在日，星期段里面，表示不指定具体的值
- "L" :只能出现在日，星期段里面，是Last的缩写，一个月的最后一天，一个星期的最后一天（星期六）
- "W" :表示工作日，距离给定值最近的工作日
- "#" :表示一个月的第几个星期几，例如："6#3"表示每个月的第三个星期五（1=SUN...6=FRI,7=SAT）

#### 示例：
- 0 0/5 * * * ? :每5分钟触发一次，从0秒以后开始
- 10 0/5 * * * ? :每5分钟触发一次，从10秒以后开始。（10:00:10am, 10:05:10am)
- 0 30 10-13 ? * WED,FRI :表示10:30,11:30,12:30,13:30，每一个星期三和星期五执行
- 0 0/30 8-9 5,20 * ? :表示每个月5号和20号 8:00, 8:30, 9:00, 9:30执行
***
## 部署
- 在根目录下打开 powershell
- 执行命令<pre><code>docker build -t aspnetapp .</code></pre>
- 执行命令运行容器 <pre><code>docker run -d -p 8099:80 --name ngp aspnetapp</code></pre>

### 以下是每个步骤的说明
- 步骤1/10：FROM microsoft/dotnet:sdk AS build-env

  从docker hub上的Microsoft / dotnet注册表中用标签“sdk”拉出图像 - https://hub.docker.com/r/microsoft/dotnet/tags
  “build-env”名称可以赋予新的构建阶段。这将用于在后面的步骤中复制文件。
  
- 步骤2/10：WORKDIR / app

  WORKDIR指令设置Dockerfile中跟随它的任何RUN，CMD，ENTRYPOINT，COPY和ADD指令的工作目录。如果WORKDIR不存在，即使它未在任何后续Dockerfile指令中使用，也将创建它。
  
- 步骤3/10：COPY . ./aspnetapp/

  文件复制到容器内的工作目录。
  
- 步骤4/10：COPY ./Hosts/NGP.WebApi/App_Data/ /app/App_Data/

  拷贝app_data的目录到容器app根目录
  
- 步骤5/10：WORKDIR /app/aspnetapp

  设定工作目录
 
- 步骤6/10：RUN dotnet publish -c Release -o out

  此步骤使用.NET CLI创建发布版本，输出目录为“out”
  
- 步骤7/10：FROM microsoft/dotnet:aspnetcore-runtime AS runtime

  提供运行ASP.NET Core Web应用程序的最少组件。
  
- 步骤8/10：WORKDIR /app

  与上面的步骤2相同。
  
- 步骤9/10：COPY --from=build-env /app/aspnetapp/Hosts/NGP.WebApi/out ./

  这会将out文件夹的内容从“build-env”复制到此网络容器。
  
- 步骤10/10：ENTRYPOINT ["dotnet", "NGP.WebApi.dll"]

  这允许容器作为可执行文件运行。
  
 
