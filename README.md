# 基于abp vnext vue的一个测试项目，开发中，逐步完善
[![.netcore](https://img.shields.io/badge/.net-5.0-green.svg)](https://docs.microsoft.com/zh-cn/dotnet/core/compatibility/5.0?toc=/dotnet/fundamentals/toc.json&bc=/dotnet/breadcrumb/toc.json)
[![abp-vnext](https://img.shields.io/badge/abp--vnext-4.2-red.svg)](https://docs.abp.io/en/abp/4.2)
[![vue-element-admin](https://img.shields.io/badge/vue--element--admin-4.3.1-brightgreen.svg)](https://github.com/PanJiaChen/vue-element-admin)
![license](https://img.shields.io/github/license/mashape/apistatus.svg)

# 安装初始化
### 后端项目
```bash 
# 进入数据库迁移项目目录
cd AbpVue/aspnet-core/src/AbpVue.DbMigrator
# 启动迁移项目
dotnet run
-- 也可以使用vs打开解决方案，还原nuget包，运行AbpVue.DbMigrator项目进行初始化迁移数据库

# 进入Host项目目录
cd AbpVue/aspnet-core/src/AbpVue.HttpApi.Host
# 启动Host项目
dotnet run
-- 也可以使用vs打开解决方案，还原nuget包，运行AbpVue.HttpApi.Host项目启动网站
```
### 前端项目 最好用yarn代替npm
```bash 
# 进入vue项目目录
cd AbpVue/vue-element-admin
# 安装依赖
yarn install
# 启动项目
yarn run dev
#构建项目
yarn run build:prod
```

# 功能截图
## 登录
![登录](screenshot/1.png)

## 功能概览
![功能概览](screenshot/2.png)

## 个人中心
![个人中心](screenshot/3.png)

## 角色权限
![角色权限](screenshot/4.png)

## 用户权限
![用户权限](screenshot/5.png)

## 审计日志
![审计日志](screenshot/6.png)

## 安全日志
![安全日志](screenshot/7.png)

## 多语言
![多语言](screenshot/8.png)
