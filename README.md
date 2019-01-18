# Dynu.com 自动更新程序
## 1.配置
在App.config里有三个配置项
  ```xml
  <add key="time_span" value="30"/>
  <!--更新时间间隔，分钟-->
  ```
  ```xml
  <add key="my_domain" value="test.dynu.net"/>
  <!--要更新的域名-->
  ```
  ```xml
  <add key="my_password" value="test"/>
  <!--更新用密码-->
  ```
  ```xml
  <add key="my_proxy" value="127.0.0.1:1080"/>
  <!--更新时的代理地址-->
  ```
## 2.使用
  配置好了双击即可。